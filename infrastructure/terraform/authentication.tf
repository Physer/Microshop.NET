module "authentication_database" {
  source                       = "./modules/container-app"
  application_name             = "authentication-db"
  container_app_environment_id = azurerm_container_app_environment.cae_microshop.id
  image_name                   = "postgres:latest"
  port                         = 5432
  transport                    = "tcp"
  ingress_enabled              = true
  resource_group_id            = azurerm_resource_group.rg_microshop.id
  secrets                      = local.authentication_database_secrets
  appsettings                  = local.authentication_database_appsettings
  revision_suffix              = random_pet.revision_suffix.id
}

module "authentication_core" {
  source                       = "./modules/container-app"
  application_name             = "authentication-core"
  container_app_environment_id = azurerm_container_app_environment.cae_microshop.id
  image_name                   = "registry.supertokens.io/supertokens/supertokens-postgresql:6.0"
  resource_group_id            = azurerm_resource_group.rg_microshop.id
  port                         = 3567
  ingress_enabled              = true
  secrets                      = local.authentication_core_secrets
  appsettings                  = local.authentication_core_appsettings
  revision_suffix              = random_pet.revision_suffix.id
}

module "authentication_service" {
  source                       = "./modules/container-app"
  application_name             = "authentication-svc"
  container_app_environment_id = azurerm_container_app_environment.cae_microshop.id
  image_name                   = "physer/microshop-authentication:main"
  resource_group_id            = azurerm_resource_group.rg_microshop.id
  ingress_enabled              = true
  appsettings                  = local.authentication_service_appsettings
  revision_suffix              = random_pet.revision_suffix.id
}

