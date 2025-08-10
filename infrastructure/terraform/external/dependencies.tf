locals {
  resource_group_name = "rg-microshop-${var.environment}"
}

data "terraform_remote_state" "generic_state" {
  backend = "azurerm"
  config = {
    resource_group_name  = "rg-tfstate"
    storage_account_name = "stmicroshopnetstate"
    container_name       = "generic-provisioning"
    key                  = "generic-services.tfstate"
  }
}

data "azurerm_resource_group" "rg_microshop" {
  name = local.resource_group_name
}

data "azurerm_container_app_environment" "cae_microshop" {
  name                = "cae-${var.environment}"
  resource_group_name = local.resource_group_name
}

data "azurerm_container_app" "ca_rabbitmq" {
  resource_group_name = local.resource_group_name
  name                = "ca-rabbitmq"
}

data "azurerm_container_app" "ca_meilisearch" {
  resource_group_name = local.resource_group_name
  name                = "ca-meilisearch"
}
