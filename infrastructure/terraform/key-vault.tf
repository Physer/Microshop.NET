resource "azurerm_key_vault" "key_vault" {
  name                        = "kv-microshop-${var.environment}"
  location                    = azurerm_resource_group.rg_microshop.location
  resource_group_name         = azurerm_resource_group.rg_microshop.name
  enabled_for_disk_encryption = true
  tenant_id                   = data.azurerm_client_config.client_config.tenant_id
  soft_delete_retention_days  = 7
  purge_protection_enabled    = false

  sku_name = "standard"

  access_policy {
    tenant_id = data.azurerm_client_config.client_config.tenant_id
    object_id = data.azurerm_client_config.client_config.object_id

    key_permissions = [
      "Get",
    ]

    secret_permissions = [
      "Get",
    ]

    storage_permissions = [
      "Get",
    ]
  }
}