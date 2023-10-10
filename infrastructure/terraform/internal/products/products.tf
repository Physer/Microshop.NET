locals {
  resource_group_name = "rg-microshop-${var.environment}"
  products_secrets = [
    { name = (data.terraform_remote_state.generic_state.outputs.rabbitmq_username_reference), value = data.terraform_remote_state.generic_state.outputs.rabbitmq_username },
    { name = (data.terraform_remote_state.generic_state.outputs.rabbitmq_password_reference), value = data.terraform_remote_state.generic_state.outputs.rabbitmq_password },
  ]
  products_appsettings = [
    { name = "Servicebus__BaseUrl", value = data.azurerm_container_app.ca_rabbitmq.name },
    { name = "Servicebus__Port", value = 5672 },
    { name = "Servicebus__ManagementUsername", secretRef = data.terraform_remote_state.generic_state.outputs.rabbitmq_username_reference },
    { name = "Servicebus__ManagementPassword", secretRef = data.terraform_remote_state.generic_state.outputs.rabbitmq_password_reference },
  ]
}

data "terraform_remote_state" "generic_state" {
  backend = "azurerm"
  config = {
    resource_group_name  = "rg-tfstate"
    storage_account_name = "stmicroshopnetstate"
    container_name       = "generic-provisioning"
    key                  = "terraform.tfstate"
  }
}

data "azurerm_resource_group" "rg_microshop" {
  name = local.resource_group_name
}

data "azurerm_container_app_environment" "cae_microshop" {
  resource_group_name = local.resource_group_name
  name                = "cae-${var.environment}"
}

data "azurerm_container_app" "ca_rabbitmq" {
  resource_group_name = local.resource_group_name
  name                = "ca-rabbitmq"
}

data "azurerm_container_app" "ca_meilisearch" {
  resource_group_name = local.resource_group_name
  name                = "ca-meilisearch"
}

module "products_app" {
  source                       = "../../modules/container-app"
  application_name             = "products"
  container_app_environment_id = data.azurerm_container_app_environment.cae_microshop.id
  image_name                   = "physer/microshop-products:main"
  resource_group_id            = data.azurerm_resource_group.rg_microshop.id
  secrets                      = local.products_secrets
  appsettings                  = local.products_appsettings
  location                     = data.azurerm_resource_group.rg_microshop.location
}
