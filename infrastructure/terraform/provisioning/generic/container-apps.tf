resource "random_string" "rabbitmq_username" {
  length  = 16
  special = false
}

resource "random_password" "rabbitmq_password" {
  length = 16
}

resource "random_password" "meilisearch_api_key" {
  length = 16
}

locals {
  rabbitmq_username = "rabbitmq-management-username"
  rabbitmq_password = "rabbitmq-management-password"
  rabbitmq_secrets = [
    { name = (local.rabbitmq_username), value = random_string.rabbitmq_username.result },
    { name = (local.rabbitmq_password), value = random_password.rabbitmq_password.result }
  ]
  rabbitmq_appsettings = [
    { name = "RABBITMQ_DEFAULT_USER", secretRef = local.rabbitmq_username },
    { name = "RABBITMQ_DEFAULT_PASS", secretRef = local.rabbitmq_password }
  ]

  meilisearch_api_key = "meilisearch-api-key"
  meilisearch_secrets = [
    { name = (local.meilisearch_api_key), value = random_password.meilisearch_api_key.result }
  ]
  meilisearch_appsettings = [
    { name = "MEILI_MASTER_KEY", secretRef = local.meilisearch_api_key }
  ]
}

module "rabbitmq_app" {
  source                       = "../../modules/container-app"
  application_name             = "rabbitmq"
  container_app_environment_id = azurerm_container_app_environment.cae_microshop.id
  image_name                   = "masstransit/rabbitmq:latest"
  resource_group_id            = azurerm_resource_group.rg_microshop.id
  port                         = 5672
  transport                    = "Tcp"
  secrets                      = local.rabbitmq_secrets
  appsettings                  = local.rabbitmq_appsettings
  ingress_enabled              = true
  location                     = azurerm_resource_group.rg_microshop.location
  scale_min                    = 1
}

module "meilisearch_app" {
  source                       = "../../modules/container-app"
  application_name             = "meilisearch"
  container_app_environment_id = azurerm_container_app_environment.cae_microshop.id
  image_name                   = "getmeili/meilisearch:latest"
  resource_group_id            = azurerm_resource_group.rg_microshop.id
  port                         = 7700
  secrets                      = local.meilisearch_secrets
  appsettings                  = local.meilisearch_appsettings
  ingress_enabled              = true
  location                     = azurerm_resource_group.rg_microshop.location
}
