resource "azurerm_resource_group" "rg_microshop" {
  name     = "rg-microshop-${var.environment}"
  location = var.location
}
