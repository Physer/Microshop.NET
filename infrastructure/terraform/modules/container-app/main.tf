terraform {
  required_providers {
    azapi = {
      source = "Azure/azapi"
    }
  }
}

resource "azapi_resource" "microshop_container_app" {
  type      = "Microsoft.App/containerApps@2022-11-01-preview"
  name      = "ca-${var.application_name}"
  location  = var.location
  parent_id = var.resource_group_id
  response_export_values = [
    "name",
    "properties.configuration.ingress.fqdn",
    "properties.customDomainVerificationId",
    "id"
  ]
  body = jsonencode({
    properties = {
      configuration = {
        activeRevisionsMode = "Single",
        ingress = !var.ingress_enabled ? null : {
          allowInsecure = false,
          external      = var.allow_external_traffic
          targetPort    = var.port,
          exposedPort   = var.transport == "Tcp" ? var.port : 0,
          traffic = [{
            latestRevision = true,
            weight         = 100
          }]
          transport = var.transport
        }
        secrets = var.secrets
      }
      environmentId = var.container_app_environment_id,
      template = {
        containers = [{
          image = var.image_name,
          name  = "ca-${var.application_name}-container",
          resources = {
            cpu    = 0.25,
            memory = "0.5Gi",
          }
          env = var.appsettings
        }]
        scale = {
          maxReplicas = var.scale_max
          minReplicas = var.scale_min
        }
      }
    }
  })
}
