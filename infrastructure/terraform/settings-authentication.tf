locals {
  database_name = "supertokens"

  authentication_database_secrets = [
    { name = (local.authentication_database_user), value = random_password.authentication_database_user.result },
    { name = (local.authentication_database_password), value = random_password.authentication_database_password.result },
  ]
  authentication_database_appsettings = [
    { name = "POSTGRES_USER", secretRef = local.authentication_database_user },
    { name = "POSTGRES_PASSWORD", secretRef = local.authentication_database_password },
    { name = "POSTGRES_DB", value = local.database_name },
  ]

  authentication_core_secrets = [
    { name = (local.authentication_database_connectionstring), value = "postgresql://${random_password.authentication_database_user.result}:${random_password.authentication_database_password.result}@${module.authentication_database.name}:5432/${local.database_name}" },
  ]
  authentication_core_appsettings = [
    { name = "POSTGRESQL_CONNECTION_URI", secretRef = local.authentication_database_connectionstring },
  ]

  authentication_service_appsettings = [
    { name = "AUTHENTICATION_CORE_URL", value = "https://${module.authentication_core.fqdn}" },
    { name = "AUTHENTICATION_BACKEND_PORT", value = 80 },
    { name = "GATEWAY_URL", value = "https://${module.gateway_app.fqdn}" },
    { name = "WEBSITE_URL", value = "http://localhost:3000" },
  ]
}
