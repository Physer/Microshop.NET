locals {
  custom_hostname    = var.environment == "production" ? var.application_name : "${var.environment}-${var.application_name}"
  custom_domain_name = "${local.custom_hostname}.microshop.rocks"
}

resource "cloudflare_record" "cname_record" {
  zone_id = var.zone_id
  name    = local.custom_hostname
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

resource "azapi_update_resource" "container_app_hostname" {
  type        = "Microsoft.App/containerApps@2023-04-01-preview"
  resource_id = var.container_app_id
  body = jsonencode({
    properties = {
      configuration = {
        ingress = {
          customDomains = [
            {
              name        = local.custom_domain_name
              bindingType = "Disabled"
            }
          ]
        }
      }
    }
  })
}

resource "azapi_resource" "managed_tls_certificate" {
  depends_on = [azapi_update_resource.container_app_hostname]
  type       = "Microsoft.App/managedEnvironments/managedCertificates@2023-04-01-preview"
  name       = var.container_environment_name
  parent_id  = var.container_environment_id
  location   = var.location
  body = jsonencode({
    properties = {
      domainControlValidation = "HTTP"
      subjectName             = local.custom_domain_name
  } })
}

resource "azapi_update_resource" "container_app_hostname_binding" {
  depends_on = [
    azapi_resource.managed_tls_certificate,
    azapi_update_resource.container_app_hostname
  ]
  type        = "Microsoft.App/containerApps@2023-04-01-preview"
  resource_id = var.container_app_id
  body = jsonencode({
    properties = {
      configuration = {
        ingress = {
          customDomains = [
            {
              name          = local.custom_domain_name
              bindingType   = "SniEnabled"
              certificateId = azapi_resource.managed_tls_certificate.id
            }
          ]
        }
      }
    }
  })
}
