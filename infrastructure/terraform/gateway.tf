module "gateway_app" {
  source                       = "./modules/container-app"
  application_name             = "gateway"
  container_app_environment_id = azurerm_container_app_environment.cae_microshop.id
  image_name                   = "physer/microshop-gateway:main"
  resource_group_id            = azurerm_resource_group.rg_microshop.id
  ingress_enabled              = true
  allow_external_traffic       = true
  revision_suffix              = random_pet.revision_suffix.id
}

module "gateway_app_settings" {
  source           = "./modules/container-app-settings"
  container_app_id = module.gateway_app.id
  secrets          = local.gateway_secrets
  appsettings      = local.gateway_appsettings
}
