module "products_app" {
  source                       = "./modules/container-app"
  application_name             = "products"
  container_app_environment_id = azurerm_container_app_environment.cae_microshop.id
  image_name                   = "physer/microshop-products:main"
  resource_group_id            = azurerm_resource_group.rg_microshop.id
  secrets                      = local.products_secrets
  appsettings                  = local.products_appsettings
  revision_suffix              = random_pet.revision_suffix.id
}