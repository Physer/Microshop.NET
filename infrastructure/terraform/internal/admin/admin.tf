locals {
  resource_group_name = "rg-microshop-${var.environment}"
}

data "azurerm_resource_group" "rg_microshop" {
  name = local.resource_group_name
}

data "azurerm_container_app_environment" "cae_microshop" {
  name                = "cae-${var.environment}"
  resource_group_name = local.resource_group_name
}

module "admin_ui_app" {
  source                       = "../../modules/container-app"
  application_name             = "admin"
  container_app_environment_id = data.azurerm_container_app_environment.cae_microshop.id
  image_name                   = "physer/microshop-admin:main"
  ingress_enabled              = true
  resource_group_id            = data.azurerm_resource_group.rg_microshop.id
  location                     = data.azurerm_resource_group.rg_microshop.location
}
