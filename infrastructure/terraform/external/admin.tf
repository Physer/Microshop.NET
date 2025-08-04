locals {
  admin_appsettings = [
    { name = "Authentication__BaseUrl", value = "https://${module.authentication_service.fqdn}" },
    { name = "UserManagement__BaseUrl", value = "https://${module.authentication_core.fqdn}" },
    { name = "DataManagement__BaseUrl", value = "https://${module.api_app.fqdn}" },
  ]
}

module "admin_app" {
  source                       = "../modules/container-app"
  application_name             = "admin"
  container_app_environment_id = data.azurerm_container_app_environment.cae_microshop.id
  image_name                   = "physer/microshop-admin:main"
  ingress_enabled              = true
  resource_group_name          = data.azurerm_resource_group.rg_microshop.name
  location                     = data.azurerm_resource_group.rg_microshop.location
  appsettings                  = local.admin_appsettings
}
