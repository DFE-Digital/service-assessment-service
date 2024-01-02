resource "azurerm_service_plan" "frontend-asp" {
  name                = "${local.resource_prefix}-frontend-asp"
  resource_group_name = local.resource_group_name_app
  location            = local.azure_location
  os_type             = "Linux"
  sku_name            = "B1"
  depends_on = [
    azurerm_subnet.frontend-subnet
  ]
}



resource "azurerm_service_plan" "backend-asp" {
  name                = "${local.resource_prefix}-backend-asp"
  resource_group_name = local.resource_group_name_app
  location            = local.azure_location
  os_type             = "Linux"
  sku_name            = "B1"
  depends_on = [
    azurerm_subnet.backend-subnet
  ]
}