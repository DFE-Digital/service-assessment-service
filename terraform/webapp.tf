# Frontend Web-App
# Create the web app, pass in the App Service Plan ID
resource "azurerm_linux_web_app" "frontend-webapp" {
  name                = "${local.resource_prefix}-frontend-webapp"
  resource_group_name = local.resource_group_name_app
  location            = local.azure_location
  service_plan_id     = azurerm_service_plan.frontend-asp.id
  https_only          = true
  site_config {
    minimum_tls_version = "1.2"
    always_on           = false

    application_stack {
      dotnet_version = "7.0"
    }
  }

  app_settings = {
    "APPINSIGHTS_INSTRUMENTATIONKEY"             = azurerm_application_insights.fg-appinsights.instrumentation_key
    "APPINSIGHTS_PROFILERFEATURE_VERSION"        = "1.0.0"
    "ApplicationInsightsAgent_EXTENSION_VERSION" = "~2"
  }

  depends_on = [
    azurerm_service_plan.frontend-asp, azurerm_application_insights.fg-appinsights
  ]
}

# Backend API
# Create the web app, pass in the App Service Plan ID
resource "azurerm_linux_web_app" "backend-webapp" {
  name                = "${local.resource_prefix}-backend-api"
  resource_group_name = local.resource_group_name_app
  location            = local.azure_location
  service_plan_id     = azurerm_service_plan.backend-asp.id
  https_only          = true
  site_config {
    minimum_tls_version = "1.2"
    always_on           = false

    application_stack {
      dotnet_version = "7.0"
    }
  }

  app_settings = {
    "APPINSIGHTS_INSTRUMENTATIONKEY"             = azurerm_application_insights.fg-appinsights.instrumentation_key
    "APPINSIGHTS_PROFILERFEATURE_VERSION"        = "1.0.0"
    "ApplicationInsightsAgent_EXTENSION_VERSION" = "~2"
  }

  depends_on = [
    azurerm_service_plan.backend-asp, azurerm_application_insights.fg-appinsights
  ]
}


output "frontend_url" {
  value = "https://${azurerm_linux_web_app.frontend-webapp.name}.azurewebsites.net"
}


output "backend_url" {
  value = "https://${azurerm_linux_web_app.backend-webapp.name}.azurewebsites.net"
}