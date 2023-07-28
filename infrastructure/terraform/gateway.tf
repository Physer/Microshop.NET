locals {
  gateway_secrets = [
    { name = (local.rabbitmq_username), value = random_string.rabbitmq_username.result },
    { name = (local.rabbitmq_password), value = random_password.rabbitmq_password.result },
  ]
  gateway_appsettings = [
    { name = "Servicebus__BaseUrl", value = module.rabbitmq_app.name },
    { name = "Servicebus__Port", value = 5672 },
    { name = "Servicebus__ManagementUsername", secretRef = local.rabbitmq_username },
    { name = "Servicebus__ManagementPassword", secretRef = local.rabbitmq_password },
    { name = "ReverseProxy__Clusters__authentication__Destinations__authentication__Address", value = "https://${module.authentication_service.fqdn}" },
  ]
}

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
