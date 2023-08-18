terraform {
  required_providers {
    azapi = {
      source  = "Azure/azapi"
      version = ">= 1.8.0"
    }
  }
}

module "microshop_container_app" {
  source                       = "../../modules/container-app"
  allow_external_traffic       = var.allow_external_traffic
  application_name             = var.application_name
  appsettings                  = var.appsettings
  container_app_environment_id = var.container_app_environment_id
  image_name                   = var.image_name
  ingress_enabled              = var.ingress_enabled
  location                     = var.location
  port                         = var.port
  resource_group_id            = var.resource_group_id
  scale_max                    = var.scale_max
  scale_min                    = var.scale_min
  secrets                      = var.secrets
  transport                    = var.transport
}
