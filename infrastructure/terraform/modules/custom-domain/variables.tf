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

variable "container_app_id" {
  description = "The ID of the Container App"
  type        = string
}

variable "container_environment_id" {
  description = "The ID of the Container App Environment"
  type        = string
}

variable "container_environment_name" {
  description = "The name of the Container App Environment"
  type        = string
}


variable "resource_group_id" {
  description = "The ID of the resource group to create the managed TLS certficate in"
  type        = string
}

variable "location" {
  type    = string
  default = "West Europe"
}

variable "cname" {
  description = "The CNAME of the custom domain"
  type        = string
}

variable "domain_name" {
  description = "The friendly domain name for use on the World Wide Web"
  type        = string
}

variable "secrets" {
  description = "The (existing) application secrets of the container"
  type = list(object({
    name  = string
    value = string
  }))
  default = []
}
