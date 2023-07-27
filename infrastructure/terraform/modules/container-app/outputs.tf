output "name" {
  description = "The name of the container app"
  value       = jsondecode(azapi_resource.microshop_container_app.output).name
}

output "fqdn" {
  description = "The FQDN of the container app"
  value       = jsondecode(azapi_resource.microshop_container_app.output).properties.configuration.ingress.fqdn
}