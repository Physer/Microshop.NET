locals {
  api_secrets = [
    { name = (local.rabbitmq_username), value = random_string.rabbitmq_username.result },
    { name = (local.rabbitmq_password), value = random_password.rabbitmq_password.result },
  ]
  api_appsettings = [
    { name = "Servicebus__BaseUrl", value = module.rabbitmq_app.name },
    { name = "Servicebus__Port", value = 5672 },
    { name = "Servicebus__ManagementUsername", secretRef = local.rabbitmq_username },
    { name = "Servicebus__ManagementPassword", secretRef = local.rabbitmq_password },
    { name = "Authentication__BaseUrl", value = "https://${module.authentication_service.fqdn}" },
    { name = "Authentication__Issuer", value = "https://${module.gateway_app.fqdn}/auth" },
  ]
}

module "api_app" {
  source                       = "./modules/container-app"
  application_name             = "api"
  container_app_environment_id = azurerm_container_app_environment.cae_microshop.id
  image_name                   = "physer/microshop-api:main"
  resource_group_id            = azurerm_resource_group.rg_microshop.id
  ingress_enabled              = true
  allow_external_traffic       = true
  location                     = azurerm_resource_group.rg_microshop.location
  appsettings                  = local.api_appsettings
  secrets                      = local.api_secrets
}
