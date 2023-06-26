output "fqdn" {
  description = "The FQDN of the latest revision of the container app"
  value       = jsondecode(azapi_resource.microshop_container_app.output).properties.latestRevisionFqdn
}
