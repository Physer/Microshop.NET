locals {
  admin_ui_application_name = "admin"
  admin_ui_cname            = var.environment == "production" ? local.admin_ui_application_name : "${var.environment}-${local.admin_ui_application_name}"
  admin_ui_domain_name      = "${local.admin_ui_cname}.microshop.rocks"

  authentication_application_name = "authentication"
  authentication_cname            = var.environment == "production" ? local.authentication_application_name : "${var.environment}-${local.authentication_application_name}"
  authentication_domain_name      = "${local.authentication_cname}.microshop.rocks"

  gateway_secrets = [
    { name = (local.rabbitmq_username), value = random_string.rabbitmq_username.result },
    { name = (local.rabbitmq_password), value = random_password.rabbitmq_password.result },
  ]
  gateway_appsettings = [
    { name = "Servicebus__BaseUrl", value = module.rabbitmq_app.name },
    { name = "Servicebus__Port", value = 5672 },
    { name = "Servicebus__ManagementUsername", secretRef = local.rabbitmq_username },
    { name = "Servicebus__ManagementPassword", secretRef = local.rabbitmq_password },
    { name = "ReverseProxy__Routes__authentication__Match__Hosts__0", value = local.authentication_domain_name },
    { name = "ReverseProxy__Clusters__authentication__Destinations__authentication__Address", value = "https://${module.authentication_service.fqdn}" },
    { name = "ReverseProxy__Routes__admin__Match__Hosts__0", value = local.admin_ui_domain_name },
    { name = "ReverseProxy__Clusters__admin__Destinations__admin__Address", value = "https://${module.admin_ui_app.fqdn}" },
    { name = "Authentication__BaseUrl", value = "https://${module.authentication_service.fqdn}" },
    { name = "Authentication__Issuer", value = "https://${module.gateway_app.fqdn}/auth" },
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

module "gateway_domain_settings_admin_ui" {
  source                     = "./modules/custom-domain"
  api_token                  = var.cloudflare_api_token
  zone_id                    = var.cloudflare_zone_id
  environment                = var.environment
  resource_group_id          = azurerm_resource_group.rg_microshop.id
  application_fqdn           = module.gateway_app.fqdn
  domain_identifier          = module.gateway_app.custom_domain_verification_id
  container_app_id           = module.gateway_app.id
  container_environment_id   = azurerm_container_app_environment.cae_microshop.id
  container_environment_name = azurerm_container_app_environment.cae_microshop.name
  cname                      = local.admin_ui_cname
  domain_name                = local.admin_ui_domain_name
  application_name           = local.admin_ui_application_name
  secrets                    = local.gateway_secrets
}

# module "gateway_domain_settings_authentication_service" {
#   source                     = "./modules/custom-domain"
#   api_token                  = var.cloudflare_api_token
#   zone_id                    = var.cloudflare_zone_id
#   environment                = var.environment
#   resource_group_id          = azurerm_resource_group.rg_microshop.id
#   application_fqdn           = module.gateway_app.fqdn
#   domain_identifier          = module.gateway_app.custom_domain_verification_id
#   container_app_id           = module.gateway_app.id
#   container_environment_id   = azurerm_container_app_environment.cae_microshop.id
#   container_environment_name = azurerm_container_app_environment.cae_microshop.name
#   cname                      = local.authentication_cname
#   domain_name                = local.authentication_domain_name
#   application_name           = local.authentication_application_name
#   secrets                    = local.gateway_secrets
# }
