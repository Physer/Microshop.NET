output "name" {
  description = "The name of the container app"
  value       = azurerm_container_app.microshop_container_app.name
}

output "fqdn" {
  description = "The FQDN of the container app"
  value       = azurerm_container_app.microshop_container_app.latest_revision_fqdn
}

output "id" {
  description = "The ID of the container app"
  value       = azurerm_container_app.microshop_container_app.id
}

output "custom_domain_verification_id" {
  description = "The Custom Domain Verification ID to be used in DNS providers"
  value       = azurerm_container_app.microshop_container_app.custom_domain_verification_id
}

output "application_name" {
  description = "The application name"
  value       = var.application_name
}
