locals {
  gateway_appsettings = [
    { name = "ReverseProxy__Routes__authentication__Match__Hosts__0", value = var.environment == "production" ? "authentication.microshop.rocks" : "${var.environment}-authentication.microshop.rocks" },
    { name = "ReverseProxy__Clusters__authentication__Destinations__authentication__Address", value = "https://${module.authentication_service.fqdn}" },
    { name = "ReverseProxy__Routes__admin__Match__Hosts__0", value = var.environment == "production" ? "admin.microshop.rocks" : "${var.environment}-admin.microshop.rocks" },
    { name = "ReverseProxy__Clusters__admin__Destinations__admin__Address", value = "https://${module.admin_app.fqdn}" },
    { name = "ReverseProxy__Routes__api__Match__Hosts__0", value = var.environment == "production" ? "api.microshop.rocks" : "${var.environment}-api.microshop.rocks" },
    { name = "ReverseProxy__Clusters__api__Destinations__api__Address", value = "https://${module.api_app.fqdn}" },
    { name = "ReverseProxy__Clusters__index__Destinations__index__Address", value = "https://${data.azurerm_container_app.ca_meilisearch.ingress[0].fqdn}" },
    { name = "ReverseProxy__Routes__index__Match__Hosts__0", value = var.environment == "production" ? "index.microshop.rocks" : "${var.environment}-index.microshop.rocks" }
  ]
}

module "gateway_app" {
  source                       = "../modules/container-app"
  application_name             = "gateway"
  container_app_environment_id = data.azurerm_container_app_environment.cae_microshop.id
  image_name                   = "physer/microshop-gateway:main"
  resource_group_id            = data.azurerm_resource_group.rg_microshop.id
  ingress_enabled              = true
  allow_external_traffic       = true
  location                     = data.azurerm_resource_group.rg_microshop.location
}

module "gateway_app_settings" {
  source           = "../modules/container-app-settings"
  container_app_id = module.gateway_app.id
  appsettings      = local.gateway_appsettings
}

module "gateway_domain_settings" {
  depends_on                 = [module.gateway_app_settings]
  source                     = "../modules/custom-domain"
  zone_id                    = var.cloudflare_zone_id
  environment                = var.environment
  application_names          = var.application_names
  application_fqdn           = module.gateway_app.fqdn
  domain_identifier          = module.gateway_app.custom_domain_verification_id
  container_app_id           = module.gateway_app.id
  container_environment_id   = data.azurerm_container_app_environment.cae_microshop.id
  container_environment_name = data.azurerm_container_app_environment.cae_microshop.name
  resource_group_id          = data.azurerm_resource_group.rg_microshop.id
}
