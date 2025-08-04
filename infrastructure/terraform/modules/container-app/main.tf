resource "azurerm_container_app" "microshop_container_app" {
  name                         = "ca-${var.application_name}"
  container_app_environment_id = var.container_app_environment_id
  resource_group_name          = var.resource_group_name
  revision_mode                = "Single"
  dynamic "secret" {
    for_each = var.secrets
    content {
      name  = secret.value["name"]
      value = secret.value["value"]
    }
  }
  dynamic "ingress" {
    for_each = var.ingress_enabled ? [1] : []
    content {
      allow_insecure_connections = false
      external_enabled           = var.allow_external_traffic
      target_port                = var.port
      exposed_port               = var.transport == "tcp" ? var.port : null
      transport                  = var.transport
      traffic_weight {
        latest_revision = true
        percentage      = 100
      }
    }
  }
  template {
    container {
      name   = "ca-${var.application_name}-container"
      image  = var.image_name
      cpu    = 0.25
      memory = "0.5Gi"
      dynamic "env" {
        for_each = var.appsettings
        content {
          name        = env.value["name"]
          value       = env.value["value"]
          secret_name = env.value["secretRef"]
        }
      }
    }
    min_replicas = var.scale_min
    max_replicas = var.scale_max
  }
}
