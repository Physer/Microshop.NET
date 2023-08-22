resource "random_password" "authentication_database_user" {
  length  = 16
  special = false
}

resource "random_password" "authentication_database_password" {
  length  = 16
  special = false
}

resource "random_password" "authentication_dashboard_password" {
  length = 16
}

locals {
  authentication_database_name             = "supertokens"
  authentication_database_connectionstring = "authentication-database-connectionstring"
  authentication_database_user             = "authentication-database-user"
  authentication_database_password         = "authentication-database-password"
  authentication_dashboard_password        = "dashboard-user-password"

  authentication_database_secrets = [
    { name = (local.authentication_database_user), value = random_password.authentication_database_user.result },
    { name = (local.authentication_database_password), value = random_password.authentication_database_password.result },
  ]
  authentication_database_appsettings = [
    { name = "POSTGRES_USER", secretRef = local.authentication_database_user },
    { name = "POSTGRES_PASSWORD", secretRef = local.authentication_database_password },
    { name = "POSTGRES_DB", value = local.authentication_database_name },
  ]

  authentication_core_secrets = [
    { name = (local.authentication_database_connectionstring), value = "postgresql://${random_password.authentication_database_user.result}:${random_password.authentication_database_password.result}@${module.authentication_database.name}:5432/${local.authentication_database_name}" },
  ]
  authentication_core_appsettings = [
    { name = "POSTGRESQL_CONNECTION_URI", secretRef = local.authentication_database_connectionstring },
  ]

  authentication_service_secrets = [
    { name = (local.authentication_dashboard_password), value = random_password.authentication_dashboard_password.result },
  ]
  authentication_service_appsettings = [
    { name = "AUTHENTICATION_CORE_URL", value = "https://${module.authentication_core.fqdn}" },
    { name = "AUTHENTICATION_BACKEND_PORT", value = 80 },
    { name = "GATEWAY_URL", value = "https://${module.gateway_app.fqdn}" },
    { name = "WEBSITE_URL", value = "http://localhost:3000" },
    { name = "DASHBOARD_USER_EMAIL", value = "admin@microshop.rocks" },
    { name = "authentication_dashboard_password", secretRef = local.authentication_dashboard_password },
  ]
}

module "authentication_database" {
  source                       = "../modules/container-app"
  application_name             = "authentication-db"
  container_app_environment_id = data.azurerm_container_app_environment.cae_microshop.id
  image_name                   = "postgres:${var.postgres_docker_image_version}"
  port                         = 5432
  transport                    = "Tcp"
  ingress_enabled              = true
  resource_group_id            = data.azurerm_resource_group.rg_microshop.id
  secrets                      = local.authentication_database_secrets
  appsettings                  = local.authentication_database_appsettings
  location                     = data.azurerm_resource_group.rg_microshop.location
}

module "authentication_core" {
  source                       = "../modules/container-app"
  application_name             = "authentication-core"
  container_app_environment_id = data.azurerm_container_app_environment.cae_microshop.id
  image_name                   = "registry.supertokens.io/supertokens/supertokens-postgresql:${var.supertokens_core_docker_image_version}"
  resource_group_id            = data.azurerm_resource_group.rg_microshop.id
  port                         = 3567
  ingress_enabled              = true
  secrets                      = local.authentication_core_secrets
  appsettings                  = local.authentication_core_appsettings
  location                     = data.azurerm_resource_group.rg_microshop.location
}

module "authentication_service" {
  source                       = "../modules/container-app"
  application_name             = "authentication-svc"
  container_app_environment_id = data.azurerm_container_app_environment.cae_microshop.id
  image_name                   = "physer/microshop-authentication:main"
  resource_group_id            = data.azurerm_resource_group.rg_microshop.id
  ingress_enabled              = true
  secrets                      = local.authentication_service_secrets
  appsettings                  = local.authentication_service_appsettings
  location                     = data.azurerm_resource_group.rg_microshop.location
}

