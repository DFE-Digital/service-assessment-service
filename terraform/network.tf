# Create a virtual network
resource "azurerm_virtual_network" "vnet" {
  name                = "app-vnet-dev" // todo: variables
  address_space       = ["10.0.0.0/16"]
  location            = local.azure_location           // todo: consider if this should be pulled directly from the resource group?
  resource_group_name = local.resource_group_name_infr // todo: consider if this should be pulled directly from the resource group?
}


#get output variables
output "resource_group_id" {
  value = local.resource_group_name_infr
}

#Create subnets
resource "azurerm_subnet" "frontend-subnet" {
  name                 = "frontend-subnet"
  resource_group_name  = local.resource_group_name_infr
  virtual_network_name = azurerm_virtual_network.vnet.name
  address_prefixes     = ["10.0.0.0/26"] // 10.0.0.0/26 (range: 10.0.0.0 - 10.0.0.63) (usable: 10.0.0.1 - 10.0.0.62)
  service_endpoints    = ["Microsoft.Web"]

  delegation {
    name = "delegation"
    service_delegation {
      name = "Microsoft.Web/serverFarms"
      actions = [
        "Microsoft.Network/virtualNetworks/subnets/action",
        "Microsoft.Network/virtualNetworks/subnets/join/action",
        "Microsoft.Network/virtualNetworks/subnets/prepareNetworkPolicies/action"
      ]
    }
  }

  lifecycle {
    ignore_changes = [
      delegation,
    ]
  }
}

#Create subnets
resource "azurerm_subnet" "backend-subnet" {
  name                 = "backend-subnet"
  resource_group_name  = local.resource_group_name_infr
  virtual_network_name = azurerm_virtual_network.vnet.name
  address_prefixes     = ["10.0.0.64/26"] // 10.0.0.64/26 (range: 10.0.0.64 - 10.0.0.127) (usable: 10.0.0.65 - 10.0.0.126)
  service_endpoints    = ["Microsoft.Sql"]

  delegation {
    name = "delegation"
    service_delegation {
      name = "Microsoft.Web/serverFarms"
      actions = [
        "Microsoft.Network/virtualNetworks/subnets/action",
        "Microsoft.Network/virtualNetworks/subnets/join/action",
        "Microsoft.Network/virtualNetworks/subnets/prepareNetworkPolicies/action"
      ]
    }
  }

  lifecycle {
    ignore_changes = [
      delegation,
    ]
  }
}