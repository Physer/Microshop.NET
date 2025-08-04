locals {
  api_secrets = [
    { name = (data.terraform_remote_state.generic_state.outputs.rabbitmq_username_reference), value = data.terraform_remote_state.generic_state.outputs.rabbitmq_username },
    { name = (data.terraform_remote_state.generic_state.outputs.rabbitmq_password_reference), value = data.terraform_remote_state.generic_state.outputs.rabbitmq_password },
  ]
  api_appsettings = [
    { name = "Servicebus__BaseUrl", value = data.azurerm_container_app.ca_rabbitmq.name },
    { name = "Servicebus__Port", value = 5672 },
    { name = "Servicebus__ManagementUsername", secretRef = data.terraform_remote_state.generic_state.outputs.rabbitmq_username_reference },
    { name = "Servicebus__ManagementPassword", secretRef = data.terraform_remote_state.generic_state.outputs.rabbitmq_password_reference },
    { name = "Authentication__BaseUrl", value = "https://${module.authentication_service.fqdn}" },
    { name = "Authentication__Issuer", value = "https://${module.gateway_app.fqdn}/auth" },
  ]
}

module "api_app" {
  source                       = "../modules/container-app"
  application_name             = "api"
  container_app_environment_id = data.azurerm_container_app_environment.cae_microshop.id
  image_name                   = "physer/microshop-api:main"
  resource_group_name          = data.azurerm_resource_group.rg_microshop.name
  ingress_enabled              = true
  allow_external_traffic       = true
  location                     = data.azurerm_resource_group.rg_microshop.location
  appsettings                  = local.api_appsettings
  secrets                      = local.api_secrets
}
