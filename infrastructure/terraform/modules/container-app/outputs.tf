output "name" {
  description = "The name of the container app"
  value       = jsondecode(azapi_resource.microshop_container_app.output).name
}
