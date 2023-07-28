variable "api_token" {
  description = "The Cloudflare API token for interfacing with the DNS zone API"
  sensitive   = true
  type        = string
}

variable "zone_id" {
  description = "The Zone ID for the Cloudflare account"
  sensitive   = true
  type        = string
}

variable "record_name" {
  description = "The name of the Cloudflare record"
  type        = string
}

variable "record_target" {
  description = "The IP or FQDN of the Cloudflare record to point to"
  type        = string
}

variable "record_type" {
  description = "The type of the Cloudflare record (e.g. A, CNAME)"
  type        = string
}
