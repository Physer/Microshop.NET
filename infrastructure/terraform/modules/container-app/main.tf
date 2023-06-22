resource "azurerm_container_app" "microshop_container_app" {
  name                         = "ca-microshop-${var.application_name}"
  container_app_environment_id = var.container_app_environment_id
  resource_group_name          = var.resource_group_name
  revision_mode                = "Single"

  template {
    container {
      name   = "ca-microshop-${var.application_name}-container"
      image  = var.image_name
      cpu    = 0.25
      memory = "0.5Gi"
    }
  }
}
