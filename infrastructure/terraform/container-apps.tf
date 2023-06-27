locals {
  rabbitmq_secrets = tolist([
    { name = (local.rabbitmq_username), value = random_string.rabbitmq_username.result },
    { name = (local.rabbitmq_password), value = random_password.rabbitmq_password.result }
  ])
  rabbitmq_appsettings = tolist([
    { name = "RABBITMQ_DEFAULT_USER", secretRef = local.rabbitmq_username },
    { name = "RABBITMQ_DEFAULT_PASS", secretRef = local.rabbitmq_password }
  ])

  meilisearch_secrets = tolist([
    { name = (local.meilisearch_api_key), value = random_password.meilisearch_api_key.result }
  ])
  meilisearch_appsettings = tolist([
    { name = "MEILI_MASTER_KEY", secretRef = local.meilisearch_api_key }
  ])

  indexing_secrets = tolist([
    { name = (local.rabbitmq_username), value = random_string.rabbitmq_username.result },
    { name = (local.rabbitmq_password), value = random_password.rabbitmq_password.result },
    { name = (local.meilisearch_api_key), value = random_password.meilisearch_api_key.result }
  ])
  indexing_appsettings = tolist([
    { name = "Servicebus__BaseUrl", value = module.rabbitmq_app.name },
    { name = "Servicebus__Port", value = 5672 },
    { name = "Indexing__BaseUrl", value = "http://${module.meilisearch_app.name}:7700/" },
    { name = "Indexing__IndexingIntervalInSeconds", value = 120 },
    { name = "Servicebus__ManagementUsername", secretRef = local.rabbitmq_username },
    { name = "Servicebus__ManagementPassword", secretRef = local.rabbitmq_password },
    { name = "Indexing__ApiKey", secretRef = local.meilisearch_api_key },
  ])

  products_secrets = tolist([
    { name = (local.rabbitmq_username), value = random_string.rabbitmq_username.result },
    { name = (local.rabbitmq_password), value = random_password.rabbitmq_password.result },
  ])
  products_appsettings = tolist([
    { name = "Servicebus__BaseUrl", value = module.rabbitmq_app.name },
    { name = "Servicebus__Port", value = 5672 },
    { name = "Servicebus__ManagementUsername", secretRef = local.rabbitmq_username },
    { name = "Servicebus__ManagementPassword", secretRef = local.rabbitmq_password },
  ])
}

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
  target_port                  = 5672
  exposed_port                 = 5672
  transport                    = "tcp"
  secrets                      = local.rabbitmq_secrets
  appsettings                  = local.rabbitmq_appsettings
  ingress_enabled              = true
  scale_min                    = 1
}

module "meilisearch_app" {
  source                       = "./modules/container-app"
  application_name             = "meilisearch"
  container_app_environment_id = azurerm_container_app_environment.cae_microshop.id
  image_name                   = "getmeili/meilisearch:latest"
  resource_group_id            = azurerm_resource_group.rg_microshop.id
  target_port                  = 7700
  secrets                      = local.meilisearch_secrets
  appsettings                  = local.meilisearch_appsettings
  ingress_enabled              = true
}

module "indexing_app" {
  source                       = "./modules/container-app"
  application_name             = "indexing"
  container_app_environment_id = azurerm_container_app_environment.cae_microshop.id
  image_name                   = "physer/microshop-indexing:main"
  resource_group_id            = azurerm_resource_group.rg_microshop.id
  secrets                      = local.indexing_secrets
  appsettings                  = local.indexing_appsettings
}

module "products_app" {
  source                       = "./modules/container-app"
  application_name             = "products"
  container_app_environment_id = azurerm_container_app_environment.cae_microshop.id
  image_name                   = "physer/microshop-products:main"
  resource_group_id            = azurerm_resource_group.rg_microshop.id
  secrets                      = local.products_secrets
  appsettings                  = local.products_appsettings
}
