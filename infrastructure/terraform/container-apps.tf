resource "azurerm_container_app_environment" "cae_microshop" {
  name                       = "cae-microshop-${var.environment}"
  location                   = azurerm_resource_group.rg_microshop.location
  resource_group_name        = azurerm_resource_group.rg_microshop.name
  log_analytics_workspace_id = azurerm_log_analytics_workspace.log_analytics.id
}

module "indexing_app" {
  source                       = "./modules/container-app"
  application_name             = "indexing"
  container_app_environment_id = azurerm_container_app_environment.cae_microshop.id
  image_name                   = "physer/microshop-products:main"
  resource_group_name          = azurerm_resource_group.rg_microshop.name
  secrets                      = local.secret_map
  appsettings = {
    "Servicebus__BaseUrl" : "rabbitmq-service",
    "Servicebus__Port" : "5672"
    "Indexing__BaseUrl" : "http://meilisearch-service:7700/",
    "Indexing__IndexingIntervalInSeconds" : "3600"
  }
  secret_appsettings = {
    "Servicebus__ManagementUsername" : local.rabbitmq_username
    "Servicebus__ManagementPassword" : local.rabbitmq_password
    "Indexing__ApiKey" : local.meilisearch_api_key
  }
}
