resource "azurerm_container_app" "microshop_container_app" {
  name                         = "ca-microshop-${var.application_name}"
  container_app_environment_id = var.container_app_environment_id
  resource_group_name          = var.resource_group_name
  revision_mode                = "Single"
  ingress {
    allow_insecure_connections = false
    external_enabled           = var.is_external
    target_port                = var.target_port
    transport                  = "tcp"
    traffic_weight {
      percentage      = 100
      latest_revision = true
    }
  }
  dynamic "secret" {
    for_each = var.secrets
    content {
      name  = secret.key
      value = secret.value
    }
  }

  template {
    container {
      name   = "ca-microshop-${var.application_name}-container"
      image  = var.image_name
      cpu    = 0.25
      memory = "0.5Gi"

      dynamic "env" {
        for_each = var.appsettings
        content {
          name  = env.key
          value = env.value
        }
      }
      dynamic "env" {
        for_each = var.secret_appsettings
        content {
          name        = env.key
          secret_name = env.value
        }
      }
    }
  }
}
