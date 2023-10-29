

// Infrastructure vault - long-lived, whose lifetime extends beyond the app
// For example, we should be able to tear down and rebuild/deploy the entire app, but retain and then reapply SSL certificates.
// Similarly, third-party API keys have lifetimes independent of the app/deployment.
resource "azurerm_key_vault" "infr-vault" {
  name                       = local.kv_name_infr
  location                   = local.azure_location
  resource_group_name        = local.resource_group_name_infr
  tenant_id                  = data.azurerm_client_config.current.tenant_id // fetched from currently logged-in user (e.g., az cli)(caveat: incomplete understanding, details TBC)
  sku_name                   = "standard"
  soft_delete_retention_days = 7 // TODO: When happy with configuration (and/or deploying to prod), change to a much longer timeframe (e.g., 90days)
  enable_rbac_authorization  = false
  tags                       = local.tags
  purge_protection_enabled   = true

  depends_on = [
    azurerm_resource_group.rg-infr
  ]
}


// Application vault - keys/secrets whose lifetime is directly linked to the application's lifetime.
// For example, database and internal API secrets are refreshed/rotated/rebuilt when the resource group/resources are destroyed and rebuilt.
resource "azurerm_key_vault" "app-vault" {
  name                       = local.kv_name_app
  location                   = local.azure_location
  resource_group_name        = local.resource_group_name_app
  tenant_id                  = data.azurerm_client_config.current.tenant_id // fetched from currently logged-in user (e.g., az cli)(caveat: incomplete understanding, details TBC)
  sku_name                   = "standard"
  soft_delete_retention_days = 7 // TODO: When happy with configuration (and/or deploying to prod), change to a much longer timeframe (e.g., 90days)
  enable_rbac_authorization  = false
  tags                       = local.tags
  purge_protection_enabled   = true

  depends_on = [
    azurerm_resource_group.rg-app
  ]
}
