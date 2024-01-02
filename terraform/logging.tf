resource "azurerm_log_analytics_workspace" "fg-loganalytics" {
  name                = "fg-la-workspace"
  resource_group_name = local.resource_group_name_infr
  location            = local.azure_location
  sku                 = "PerGB2018"
  retention_in_days   = 30
}

resource "azurerm_application_insights" "fg-appinsights" {
  name                = "fg-appinsights"
  resource_group_name = local.resource_group_name_infr
  location            = local.azure_location
  workspace_id        = azurerm_log_analytics_workspace.fg-loganalytics.id
  application_type    = "web"
  depends_on = [
    azurerm_log_analytics_workspace.fg-loganalytics
  ]
}

output "instrumentation_key" {
  value = azurerm_application_insights.fg-appinsights.instrumentation_key
  sensitive = true
}

output "app_id" {
  value = azurerm_application_insights.fg-appinsights.id
  sensitive = true
}
