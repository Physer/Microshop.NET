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
  resource_group_name          = azurerm_resource_group.rg_microshop.name
  target_port                  = 5672
}

module "meilisearch_app" {
  source                       = "./modules/container-app"
  application_name             = "meilisearch"
  container_app_environment_id = azurerm_container_app_environment.cae_microshop.id
  image_name                   = "getmeili/meilisearch:latest"
  resource_group_name          = azurerm_resource_group.rg_microshop.name
  target_port                  = 7700
  secrets = tomap({
    (local.meilisearch_api_key) = random_password.meilisearch_api_key.result
  })
  secret_appsettings = {
    "MEILI_MASTER_KEY" : local.meilisearch_api_key
  }
}

module "indexing_app" {
  source                       = "./modules/container-app"
  application_name             = "indexing"
  container_app_environment_id = azurerm_container_app_environment.cae_microshop.id
  image_name                   = "physer/microshop-products:main"
  resource_group_name          = azurerm_resource_group.rg_microshop.name
  secrets = tomap({
    (local.rabbitmq_username)   = random_password.rabbitmq_username.result
    (local.rabbitmq_password)   = random_password.rabbitmq_password.result
    (local.meilisearch_api_key) = random_password.meilisearch_api_key.result
  })
  appsettings = {
    "Servicebus__BaseUrl" : module.rabbitmq_app.fqdn,
    "Servicebus__Port" : "5672"
    "Indexing__BaseUrl" : "http://${module.meilisearch_app.fqdn}:7700/",
    "Indexing__IndexingIntervalInSeconds" : "3600"
  }
  secret_appsettings = {
    "Servicebus__ManagementUsername" : local.rabbitmq_username
    "Servicebus__ManagementPassword" : local.rabbitmq_password
    "Indexing__ApiKey" : local.meilisearch_api_key
  }
}
