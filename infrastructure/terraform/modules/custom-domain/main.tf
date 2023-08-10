resource "cloudflare_record" "cname_record" {
  zone_id         = var.zone_id
  name            = var.cname
  value           = var.application_fqdn
  type            = "CNAME"
  ttl             = 3600
  allow_overwrite = true
}

resource "cloudflare_record" "txt_record" {
  zone_id         = var.zone_id
  name            = var.environment == "production" ? "asuid.${var.application_name}" : "asuid.${var.environment}-${var.application_name}"
  value           = var.domain_identifier
  type            = "TXT"
  ttl             = 3600
  allow_overwrite = true
}

resource "time_sleep" "wait_for_dns" {
  depends_on = [
    cloudflare_record.cname_record,
    cloudflare_record.txt_record
  ]
  create_duration = "120s"
  triggers = {
    "uuid" = uuid()
  }
}

resource "azapi_update_resource" "container_app_hostname" {
  depends_on  = [time_sleep.wait_for_dns]
  type        = "Microsoft.App/containerApps@2023-04-01-preview"
  resource_id = var.container_app_id
  body = jsonencode({
    properties = {
      configuration = {
        secrets = var.secrets
        ingress = {
          customDomains = [
            {
              name        = var.domain_name
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
  name       = "${var.container_environment_name}-${var.application_name}"
  parent_id  = var.container_environment_id
  location   = var.location
  body = jsonencode({
    properties = {
      domainControlValidation = "CNAME"
      subjectName             = var.domain_name
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
        secrets = var.secrets
        ingress = {
          customDomains = [
            {
              name          = var.domain_name
              bindingType   = "SniEnabled"
              certificateId = azapi_resource.managed_tls_certificate.id
            }
          ]
        }
      }
    }
  })
}

resource "time_sleep" "wait_to_proxy" {
  depends_on      = [azapi_update_resource.container_app_hostname_binding]
  create_duration = "120s"
  triggers = {
    "uuid" = uuid()
  }
}

resource "cloudflare_record" "proxied_cname_record" {
  depends_on      = [time_sleep.wait_to_proxy]
  zone_id         = var.zone_id
  name            = var.cname
  value           = var.application_fqdn
  type            = "CNAME"
  ttl             = 1
  allow_overwrite = true
  proxied         = true
}
