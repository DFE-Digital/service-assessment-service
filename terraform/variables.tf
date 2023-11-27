###########
# General #
###########
variable "project_code" {
  description = "project code, usually in the format of `s000`"
  type        = string
}

variable "project_name" {
  description = "project name, used along with `environment` as a prefix for most resources (e.g., VeyVault specifically needs a shorter name)"
  type        = string
}

variable "project_short_name" {
  description = "project name, used along with `environment_short_name` as a prefix for resources where a short name is specifically required (e.g., KeyVault)"
  type        = string
}

variable "environment" {
  description = "Environment name, used along with `project_name` as a prefix for most resources (e.g., VeyVault specifically needs a shorter name)"
  type        = string
}

variable "environment_short_name" {
  description = "Environment short name, used along with `project_short_name` as a prefix for resources where a short name is specifically required (e.g., KeyVault)"
  type        = string
}

variable "azure_location" {
  description = "Resource location"
  type        = string
  default     = "UK South"
}

variable "az_tag_environment" {
  description = "Environment tag to be applied to all resources"
  type        = string
}

variable "az_tag_product" {
  description = "Product tag to be applied to all resources"
  type        = string
}


############
# Identity #
############
variable "msi_id" {
  type        = string
  description = "The Managed Service Identity ID. If this value isn't null (the default), 'data.azurerm_client_config.current.object_id' will be set to this value."
  default     = null
}

# ##############
# # Front Door #
# ##############
# variable "cdn_frontdoor_origin_host_header_override" {
#   description = "Override the frontdoor origin host header"
#   type        = string
# }

# #############
# # Azure SQL #
# #############
# variable "az_sql_admin_password" {
#   description = "Azure SQL admin password"
#   type        = string
# }

# variable "az_sql_admin_userid_postfix" {
#   description = "Azure SQL admin userid postfix, used with `project_name` and `environment` to build userid"
#   type        = string
# }

# ############
# # KeyVault #
# ############
# variable "key_type" {
#   description = "The JsonWebKeyType of the key to be created."
#   default     = "RSA"
#   type        = string
#   validation {
#     condition     = contains(["EC", "EC-HSM", "RSA", "RSA-HSM"], var.key_type)
#     error_message = "The key_type must be one of the following: EC, EC-HSM, RSA, RSA-HSM."
#   }
# }

# variable "key_ops" {
#   type        = list(string)
#   description = "The permitted JSON web key operations of the key to be created."
#   default     = ["decrypt", "encrypt", "sign", "unwrapKey", "verify", "wrapKey"]
# }

# variable "key_size" {
#   type        = number
#   description = "The size in bits of the key to be created."
#   default     = 2048
# }

# #######################
# # Azure App Container #
# #######################
# variable "az_app_kestrel_endpoint" {
#   description = "Endpoint for Kestrel setup"
#   type        = string
# }