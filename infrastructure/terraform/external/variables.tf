variable "environment" {
  type = string
}

variable "location" {
  type    = string
  default = "West Europe"
}

variable "cloudflare_api_token" {
  description = "The Cloudflare API token for interfacing with the DNS zone API"
  sensitive   = true
  type        = string
}

variable "cloudflare_zone_id" {
  description = "The Zone ID for the Cloudflare account"
  sensitive   = true
  type        = string
}

variable "application_names" {
  description = "A set of application names to create DNS records for"
  type        = set(string)
}

variable "supertokens_core_docker_image_version" {
  description = "The docker image version of the Supertokens Core application"
  type        = string
}

variable "postgres_docker_image_version" {
  description = "The docker image version of the Postgres SQL database"
  type        = string
}
