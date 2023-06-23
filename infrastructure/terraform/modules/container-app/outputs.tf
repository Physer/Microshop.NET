output "fqdn" {
  description = "The FQDN of the latest revision of the container app"
  value       = azurerm_container_app.microshop_container_app.latest_revision_fqdn
}
