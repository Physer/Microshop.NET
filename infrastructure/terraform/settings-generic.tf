resource "random_password" "meilisearch_api_key" {
  length = 16
}

resource "random_string" "rabbitmq_username" {
  length  = 16
  special = false
}

resource "random_password" "rabbitmq_password" {
  length = 16
}

resource "random_password" "authentication_database_user" {
  length  = 16
  special = false
}

resource "random_password" "authentication_database_password" {
  length  = 16
  special = false
}

resource "random_pet" "revision_suffix" {
  keepers = {
    uuid = uuid()
  }
}

locals {
  rabbitmq_username                        = "rabbitmq-management-username"
  rabbitmq_password                        = "rabbitmq-management-password"
  meilisearch_api_key                      = "meilisearch-api-key"
  authentication_database_connectionstring = "authentication-database-connectionstring"
  authentication_database_user             = "authentication-database-user"
  authentication_database_password         = "authentication-database-password"
}
