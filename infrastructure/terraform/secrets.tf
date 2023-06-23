resource "random_password" "meilisearch_api_key" {
  length = 16
}

resource "random_password" "rabbitmq_username" {
  length = 16
}

resource "random_password" "rabbitmq_password" {
  length = 16
}

locals {
  rabbitmq_username   = "rabbitmq-management-username"
  rabbitmq_password   = "rabbitmq-management-password"
  meilisearch_api_key = "meilisearch-api-key"
}
