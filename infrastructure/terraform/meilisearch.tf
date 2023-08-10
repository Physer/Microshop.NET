resource "random_password" "meilisearch_api_key" {
  length = 16
}

locals {
  meilisearch_api_key = "meilisearch-api-key"
  meilisearch_secrets = [
    { name = (local.meilisearch_api_key), value = random_password.meilisearch_api_key.result }
  ]
  meilisearch_appsettings = [
    { name = "MEILI_MASTER_KEY", secretRef = local.meilisearch_api_key }
  ]
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
  revision_suffix              = random_pet.revision_suffix.id
  location                     = azurerm_resource_group.rg_microshop.location
}
