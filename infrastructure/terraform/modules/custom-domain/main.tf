locals {
  custom_domain_name = var.environment == "production" ? var.application_name : "${var.environment}-${var.application_name}"
}

resource "cloudflare_record" "cname_record" {
  zone_id = var.zone_id
  name    = local.custom_domain_name
  value   = var.application_fqdn
  type    = "CNAME"
  ttl     = 3600
}

resource "cloudflare_record" "txt_record" {
  zone_id = var.zone_id
  name    = var.environment == "production" ? "asuid.${var.application_name}" : "asuid.${var.environment}-${var.application_name}"
  value   = var.domain_identifier
  type    = "TXT"
  ttl     = 3600
}

# resource "azapi_update_resource" "microshop_container_app_custom_domain" {
#   type        = "Microsoft.App/containerApps@2022-11-01-preview"
#   resource_id = var.container_app_id
#   body = jsonencode({
#     properties = {
#       configuration = {
#         ingress = {
#           customDomains = [
#             {
#               name = local.custom_domain_name
#             }
#           ]
#         }
#       }
#     }
#   })
# }

