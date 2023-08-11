terraform {
  required_providers {
    cloudflare = {
      source = "cloudflare/cloudflare"
    }
    azapi = {
      source = "Azure/azapi"
    }
  }
}

resource "cloudflare_record" "cname_record" {
  for_each = var.application_names
  zone_id  = var.zone_id
  name     = var.environment == "production" ? each.key : "${var.environment}-${each.key}"
  value    = var.application_fqdn
  type     = "CNAME"
  ttl      = 3600
}

resource "cloudflare_record" "txt_record" {
  for_each = var.application_names
  zone_id  = var.zone_id
  name     = var.environment == "production" ? "asuid.${each.key}" : "asuid.${var.environment}-${each.key}"
  value    = var.domain_identifier
  type     = "TXT"
  ttl      = 3600
}

resource "time_sleep" "wait_for_dns" {
  depends_on = [
    cloudflare_record.cname_record,
    cloudflare_record.txt_record
  ]
  create_duration = "30s"
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
          customDomains = [for name in var.application_names : {
            name        = var.environment == "production" ? "${name}.microshop.rocks" : "${var.environment}-${name}.microshop.rocks"
            bindingType = "Disabled"
          }]
        }
      }
    }
  })
}

resource "azapi_resource" "managed_tls_certificate" {
  depends_on = [azapi_update_resource.container_app_hostname]
  for_each   = var.application_names
  type       = "Microsoft.App/managedEnvironments/managedCertificates@2023-04-01-preview"
  name       = "${var.container_environment_name}-${each.key}"
  parent_id  = var.container_environment_id
  location   = var.location
  body = jsonencode({
    properties = {
      domainControlValidation = "CNAME"
      subjectName             = var.environment == "production" ? "${each.key}.microshop.rocks" : "${var.environment}-${each.key}.microshop.rocks"
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
          customDomains = [for name in var.application_names : {
            name          = var.environment == "production" ? "${name}.microshop.rocks" : "${var.environment}-${name}.microshop.rocks"
            bindingType   = "SniEnabled"
            certificateId = azapi_resource.managed_tls_certificate[name].id
          }]
        }
      }
    }
  })
}

resource "time_sleep" "wait_to_proxy" {
  depends_on      = [azapi_update_resource.container_app_hostname_binding]
  create_duration = "30s"
  triggers = {
    "uuid" = uuid()
  }
}

resource "cloudflare_record" "proxied_cname_record" {
  depends_on      = [time_sleep.wait_to_proxy, cloudflare_record.cname_record]
  for_each        = var.application_names
  zone_id         = var.zone_id
  name            = var.environment == "production" ? each.key : "${var.environment}-${each.key}"
  value           = var.application_fqdn
  type            = "CNAME"
  ttl             = 1
  proxied         = true
  allow_overwrite = true
}
