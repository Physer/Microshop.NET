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

variable "application_name" {
  description = "The name of the application to create the CNAME for"
  type        = string
}

variable "application_fqdn" {
  description = "The FQDN to point the CNAME to"
  type        = string
}

variable "domain_identifier" {
  description = "The domain identifier for the TXT record (ASUID)"
  type        = string
}

variable "environment" {
  description = "The environment to deploy to"
  type        = string
}
