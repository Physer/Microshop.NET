locals {
  indexing_secrets = [
    { name = (local.rabbitmq_username), value = random_string.rabbitmq_username.result },
    { name = (local.rabbitmq_password), value = random_password.rabbitmq_password.result },
    { name = (local.meilisearch_api_key), value = random_password.meilisearch_api_key.result }
  ]
  indexing_appsettings = [
    { name = "Servicebus__BaseUrl", value = module.rabbitmq_app.name },
    { name = "Servicebus__Port", value = 5672 },
    { name = "Indexing__BaseUrl", value = "http://${module.meilisearch_app.name}/" },
    { name = "Indexing__IndexingIntervalInSeconds", value = 3600 },
    { name = "Servicebus__ManagementUsername", secretRef = local.rabbitmq_username },
    { name = "Servicebus__ManagementPassword", secretRef = local.rabbitmq_password },
    { name = "Indexing__ApiKey", secretRef = local.meilisearch_api_key },
  ]
}


module "indexing_app" {
  source                       = "../../modules/container-app"
  application_name             = "indexing"
  container_app_environment_id = azurerm_container_app_environment.cae_microshop.id
  image_name                   = "physer/microshop-indexing:main"
  resource_group_id            = azurerm_resource_group.rg_microshop.id
  secrets                      = local.indexing_secrets
  appsettings                  = local.indexing_appsettings
  location                     = azurerm_resource_group.rg_microshop.location
}
