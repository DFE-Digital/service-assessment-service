# We strongly recommend using the required_providers block to set the
# Azure Provider source and version being used
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=3.79.0"
    }
  }
}

# Configure the Microsoft Azure Provider
provider "azurerm" {
  skip_provider_registration = true # This is only required when the User, Service Principal, or Identity running Terraform lacks the permissions to register Azure Resource Providers.
  features {}
}

// Create a resource group
// Infrastructure resources outlive the application and are persistent even when the application is destroyed (e.g., networks)
resource "azurerm_resource_group" "rg-infr" {
  name     = local.resource_group_name_infr
  location = var.azure_location
  tags     = local.tags
}

// Create a resource group
// The application resource group(s) should be able to be completely destroyed then rebuilt
// TODO: Consider impact of custom domains...?
// TODO: KeyVaults, by default, can't be destroyed and immediately recreated thus might not align with this plan?
resource "azurerm_resource_group" "rg-app" {
  name     = local.resource_group_name_app
  location = var.azure_location
  tags     = local.tags
}

# # Create a virtual network within the resource group
# resource "azurerm_virtual_network" "example" {
#   name                = "example-network"
#   resource_group_name = azurerm_resource_group.example-rg.name
#   location            = azurerm_resource_group.example-rg.location
#   address_space       = ["10.0.0.0/16"]
# }

# module "infrastructure_secrets" {
#   source = "git::https://github.com/DFE-Digital/terraform-modules.git//aks/secrets?ref=stable"

#   azure_resource_prefix = "s000d01"
#   service_short         = "sas"
#   config_short          = "dv"
#   key_vault_short       = "inf" # infrastructure secrets (kept separate from application secrets)
# }

# module "web_application" {
#   source = "git::https://github.com/DFE-Digital/terraform-modules.git//aks/application?ref=stable"

#   name   = "web"
#   is_web = true

#   namespace    = var.namespace
#   environment  = local.environment
#   service_name = local.service_name

#   cluster_configuration_map = module.cluster_data.configuration_map

#   kubernetes_config_map_name = module.application_configuration.kubernetes_config_map_name
#   kubernetes_secret_name     = module.application_configuration.kubernetes_secret_name

#   docker_image = var.docker_image
# }

