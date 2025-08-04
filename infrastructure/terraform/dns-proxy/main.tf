data "azurerm_container_app" "ca_gateway" {
  resource_group_name = "rg-microshop-${var.environment}"
  name                = "ca-gateway"
}

resource "cloudflare_record" "cname_record" {
  for_each        = var.application_names
  zone_id         = var.cloudflare_zone_id
  name            = var.environment == "production" ? each.key : "${var.environment}-${each.key}"
  value           = data.azurerm_container_app.ca_gateway.ingress[0].fqdn
  type            = "CNAME"
  ttl             = 1
  proxied         = true
  allow_overwrite = true
}
