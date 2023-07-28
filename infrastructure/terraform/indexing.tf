module "indexing_app" {
  source                       = "./modules/container-app"
  application_name             = "indexing"
  container_app_environment_id = azurerm_container_app_environment.cae_microshop.id
  image_name                   = "physer/microshop-indexing:main"
  resource_group_id            = azurerm_resource_group.rg_microshop.id
  secrets                      = local.indexing_secrets
  appsettings                  = local.indexing_appsettings
  revision_suffix              = random_pet.revision_suffix.id
}