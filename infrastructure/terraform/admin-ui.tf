module "admin_ui_app" {
  source                       = "./modules/container-app"
  application_name             = "admin-ui"
  container_app_environment_id = azurerm_container_app_environment.cae_microshop.id
  image_name                   = "physer/microshop-admin-ui:main"
  ingress_enabled              = true
  resource_group_id            = azurerm_resource_group.rg_microshop.id
  revision_suffix              = random_pet.revision_suffix.id
  location                     = azurerm_resource_group.rg_microshop.location
}
