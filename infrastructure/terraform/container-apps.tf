resource "azurerm_container_app_environment" "cae_microshop" {
  name                       = "cae-microshop-${var.environment}"
  location                   = azurerm_resource_group.rg_microshop.location
  resource_group_name        = azurerm_resource_group.rg_microshop.name
  log_analytics_workspace_id = azurerm_log_analytics_workspace.log_analytics.id
}

module "rabbitmq_app" {
  source                       = "./modules/container-app"
  application_name             = "rabbitmq"
  container_app_environment_id = azurerm_container_app_environment.cae_microshop.id
  image_name                   = "masstransit/rabbitmq:latest"
  resource_group_id            = azurerm_resource_group.rg_microshop.id
  port                         = 5672
  transport                    = "tcp"
  secrets                      = local.rabbitmq_secrets
  appsettings                  = local.rabbitmq_appsettings
  ingress_enabled              = true
  revision_suffix              = random_pet.revision_suffix.id
}

module "meilisearch_app" {
  source                       = "./modules/container-app"
  application_name             = "meilisearch"
  container_app_environment_id = azurerm_container_app_environment.cae_microshop.id
  image_name                   = "getmeili/meilisearch:latest"
  resource_group_id            = azurerm_resource_group.rg_microshop.id
  port                         = 7700
  secrets                      = local.meilisearch_secrets
  appsettings                  = local.meilisearch_appsettings
  ingress_enabled              = true
  allow_external_traffic       = true
  revision_suffix              = random_pet.revision_suffix.id
}

module "gateway_app" {
  source                       = "./modules/container-app"
  application_name             = "gateway"
  container_app_environment_id = azurerm_container_app_environment.cae_microshop.id
  image_name                   = "physer/microshop-gateway:main"
  resource_group_id            = azurerm_resource_group.rg_microshop.id
  ingress_enabled              = true
  allow_external_traffic       = true
  secrets                      = local.gateway_secrets
  appsettings                  = local.gateway_appsettings
  revision_suffix              = random_pet.revision_suffix.id
}

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

module "authentication_database" {
  source                       = "./modules/container-app"
  application_name             = "authentication-db"
  container_app_environment_id = azurerm_container_app_environment.cae_microshop.id
  image_name                   = "postgres:latest"
  resource_group_id            = azurerm_resource_group.rg_microshop.id
  secrets                      = local.authentication_database_secrets
  appsettings                  = local.authentication_database_appsettings
  revision_suffix              = random_pet.revision_suffix.id
}

module "authentication_core" {
  source                       = "./modules/container-app"
  application_name             = "authentication-core"
  container_app_environment_id = azurerm_container_app_environment.cae_microshop.id
  image_name                   = "registry.supertokens.io/supertokens/supertokens-postgresql:6.0"
  resource_group_id            = azurerm_resource_group.rg_microshop.id
  ingress_enabled              = true
  secrets                      = local.authentication_core_secrets
  appsettings                  = local.authentication_core_appsettings
  revision_suffix              = random_pet.revision_suffix.id
}

module "authentication_service" {
  source                       = "./modules/container-app"
  application_name             = "authentication-svc"
  container_app_environment_id = azurerm_container_app_environment.cae_microshop.id
  image_name                   = "physer/microshop-authentication:main"
  resource_group_id            = azurerm_resource_group.rg_microshop.id
  appsettings                  = local.authentication_service_appsettings
  revision_suffix              = random_pet.revision_suffix.id
}
