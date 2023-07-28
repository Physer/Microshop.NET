locals {
  pricing_secrets = [
    { name = (local.rabbitmq_username), value = random_string.rabbitmq_username.result },
    { name = (local.rabbitmq_password), value = random_password.rabbitmq_password.result },
  ]
  pricing_appsettings = [
    { name = "Servicebus__BaseUrl", value = module.rabbitmq_app.name },
    { name = "Servicebus__Port", value = 5672 },
    { name = "Servicebus__ManagementUsername", secretRef = local.rabbitmq_username },
    { name = "Servicebus__ManagementPassword", secretRef = local.rabbitmq_password },
  ]
}


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
