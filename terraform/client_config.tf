
// This is a data source, providing information about the currently-logged-in user (e.g., via az cli).
// It can be used to pull information such as tenant ID (where resources will be deployed to),
// ... without needing to explicitly specify it within variables.
data "azurerm_client_config" "current" {}