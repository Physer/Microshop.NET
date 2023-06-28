locals {
  indexing_secrets = [
    { name = (local.rabbitmq_username), value = random_string.rabbitmq_username.result },
    { name = (local.rabbitmq_password), value = random_password.rabbitmq_password.result },
    { name = (local.meilisearch_api_key), value = random_password.meilisearch_api_key.result }
  ]
  indexing_appsettings = [
    { name = "Servicebus__BaseUrl", value = module.rabbitmq_app.revision_name },
    { name = "Servicebus__Port", value = 5672 },
    { name = "Indexing__BaseUrl", value = "http://${module.meilisearch_app.revision_fqdn}/" },
    { name = "Indexing__IndexingIntervalInSeconds", value = 3600 },
    { name = "Servicebus__ManagementUsername", secretRef = local.rabbitmq_username },
    { name = "Servicebus__ManagementPassword", secretRef = local.rabbitmq_password },
    { name = "Indexing__ApiKey", secretRef = local.meilisearch_api_key },
  ]
}
