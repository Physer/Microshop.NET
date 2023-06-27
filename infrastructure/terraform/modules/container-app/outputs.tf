output "name" {
  description = "The name of the Azure Container App"
  value       = jsondecode(azapi_resource.microshop_container_app.output).name
}
