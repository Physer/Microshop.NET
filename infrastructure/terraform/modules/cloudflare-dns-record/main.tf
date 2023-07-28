resource "cloudflare_record" "dns_record" {
  zone_id = var.zone_id
  name    = var.record_name
  value   = var.record_target
  type    = var.record_type
  ttl     = 3600
}
