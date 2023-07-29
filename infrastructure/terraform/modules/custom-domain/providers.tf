terraform {
  required_providers {
    cloudflare = {
      source  = "cloudflare/cloudflare"
      version = "~> 4.0"
    }
    azapi = {
      source  = "Azure/azapi"
      version = "~> 1.7.0"
    }
  }
}

provider "cloudflare" {
  api_token = var.api_token
}
