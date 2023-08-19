resource "azurerm_log_analytics_workspace" "log_analytics" {
  name                = "log-microshop-${var.environment}"
  location            = azurerm_resource_group.rg_microshop.location
  resource_group_name = azurerm_resource_group.rg_microshop.name
  sku                 = "PerGB2018"
}
