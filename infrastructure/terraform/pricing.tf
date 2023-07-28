module "pricing_app" {
  source                       = "./modules/container-app"
  application_name             = "pricing"
  container_app_environment_id = azurerm_container_app_environment.cae_microshop.id
  image_name                   = "physer/microshop-pricing:main"
  resource_group_id            = azurerm_resource_group.rg_microshop.id
  secrets                      = local.pricing_secrets
  appsettings                  = local.pricing_appsettings
  revision_suffix              = random_pet.revision_suffix.id
}
