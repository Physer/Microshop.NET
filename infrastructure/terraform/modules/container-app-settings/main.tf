resource "azapi_update_resource" "microshop_container_app_settings" {
  type        = "Microsoft.App/containerApps@2022-11-01-preview"
  resource_id = var.container_app_id
  body = {
    properties = {
      configuration = {
        secrets = var.secrets
      }
      template = {
        containers = [{
          env = var.appsettings
        }]
      }
    }
  }
}
