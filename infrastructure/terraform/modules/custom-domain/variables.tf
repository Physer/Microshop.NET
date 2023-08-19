variable "zone_id" {
  description = "The Zone ID for the Cloudflare account"
  sensitive   = true
  type        = string
}

variable "application_names" {
  description = "The names of the applications to create the CNAME for"
  type        = set(string)
}

variable "application_fqdn" {
  description = "The FQDN to use as the CNAME target"
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

variable "secrets" {
  description = "The (existing) application secrets of the container"
  type = list(object({
    name  = string
    value = string
  }))
  default = []
}
