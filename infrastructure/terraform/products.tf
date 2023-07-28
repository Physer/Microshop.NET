locals {
  products_secrets = [
    { name = (local.rabbitmq_username), value = random_string.rabbitmq_username.result },
    { name = (local.rabbitmq_password), value = random_password.rabbitmq_password.result },
  ]
  products_appsettings = [
    { name = "Servicebus__BaseUrl", value = module.rabbitmq_app.name },
    { name = "Servicebus__Port", value = 5672 },
    { name = "Servicebus__ManagementUsername", secretRef = local.rabbitmq_username },
    { name = "Servicebus__ManagementPassword", secretRef = local.rabbitmq_password },
  ]
}

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
