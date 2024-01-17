// When performing a `terraform apply`, output can be displayed in the terminal.
// This is useful to output an app service's URL and other IDs, for example.


output "environment" {
  value = local.environment
}
output "environment_short_name" {
  value = local.environment_short_name
}
output "project_code" {
  value = local.project_code
}
output "resource_group_name_infr" {
  value = local.resource_group_name_infr
}
output "resource_group_name_app" {
  value = local.resource_group_name_app
}



# output "hostname" {
#   value = local.hostname
# }

# output "url" {
#   value = "https://${local.hostname}"
# }

# output "probe_url" {
#   value = var.probe_path != null ? "https://${local.hostname}${var.probe_path}" : null
# }