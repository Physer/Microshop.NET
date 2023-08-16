output "name" {
  description = "The name of the container app"
  value       = jsondecode(azapi_resource.microshop_container_app.output).name
}

output "fqdn" {
  description = "The FQDN of the container app"
  value       = try(jsondecode(azapi_resource.microshop_container_app.output).properties.configuration.ingress.fqdn, null)
}

output "id" {
  description = "The ID of the container app"
  value       = jsondecode(azapi_resource.microshop_container_app.output).id
}

output "custom_domain_verification_id" {
  description = "The Custom Domain Verification ID to be used in DNS providers"
  value       = jsondecode(azapi_resource.microshop_container_app.output).properties.customDomainVerificationId
}

output "application_name" {
  description = "The application name"
  value       = var.application_name
}
