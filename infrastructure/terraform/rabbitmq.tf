module "rabbitmq_app" {
  source                       = "./modules/container-app"
  application_name             = "rabbitmq"
  container_app_environment_id = azurerm_container_app_environment.cae_microshop.id
  image_name                   = "masstransit/rabbitmq:latest"
  resource_group_id            = azurerm_resource_group.rg_microshop.id
  port                         = 5672
  transport                    = "tcp"
  secrets                      = local.rabbitmq_secrets
  appsettings                  = local.rabbitmq_appsettings
  ingress_enabled              = true
  revision_suffix              = random_pet.revision_suffix.id
}