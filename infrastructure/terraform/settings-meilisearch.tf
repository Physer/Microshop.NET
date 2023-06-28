locals {
  meilisearch_secrets = [
    { name = (local.meilisearch_api_key), value = random_password.meilisearch_api_key.result }
  ]
  meilisearch_appsettings = [
    { name = "MEILI_MASTER_KEY", secretRef = local.meilisearch_api_key }
  ]
}
