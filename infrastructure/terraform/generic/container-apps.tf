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
  source                       = "../modules/container-app"
  application_name             = "rabbitmq"
  container_app_environment_id = azurerm_container_app_environment.cae_microshop.id
  image_name                   = "masstransit/rabbitmq:${var.rabbitmq_docker_image_version}"
  resource_group_name          = azurerm_resource_group.rg_microshop.name
  port                         = 5672
  transport                    = "tcp"
  secrets                      = local.rabbitmq_secrets
  appsettings                  = local.rabbitmq_appsettings
  ingress_enabled              = true
  location                     = azurerm_resource_group.rg_microshop.location
}

module "meilisearch_app" {
  source                       = "../modules/container-app"
  application_name             = "meilisearch"
  container_app_environment_id = azurerm_container_app_environment.cae_microshop.id
  image_name                   = "getmeili/meilisearch:${var.meilisearch_docker_image_version}"
  resource_group_name          = azurerm_resource_group.rg_microshop.name
  port                         = 7700
  secrets                      = local.meilisearch_secrets
  appsettings                  = local.meilisearch_appsettings
  ingress_enabled              = true
  location                     = azurerm_resource_group.rg_microshop.location
}

output "rabbitmq_username_reference" {
  value = local.rabbitmq_username
}

output "rabbitmq_username" {
  value = random_string.rabbitmq_username.result
}

output "rabbitmq_password_reference" {
  value = local.rabbitmq_password
}

output "rabbitmq_password" {
  value     = random_password.rabbitmq_password.result
  sensitive = true
}

output "meilisearch_api_key_reference" {
  value = local.meilisearch_api_key
}

output "meilisearch_api_key" {
  value     = random_password.meilisearch_api_key.result
  sensitive = true
}
