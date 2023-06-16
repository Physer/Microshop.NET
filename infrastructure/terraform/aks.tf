resource "azurerm_kubernetes_cluster" "aks_cluster" {
  name                = "aks-microshop-${var.environment}"
  location            = azurerm_resource_group.rg_microshop.location
  resource_group_name = azurerm_resource_group.rg_microshop.name
  dns_prefix          = "microshop-${var.environment}"
  sku_tier            = "Free"

  default_node_pool {
    name       = "default"
    node_count = 1
    vm_size    = var.cluster_size
  }

  identity {
    type = "SystemAssigned"
  }
}
