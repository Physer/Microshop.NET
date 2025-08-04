terraform {
  required_providers {
    cloudflare = {
      source  = "cloudflare/cloudflare"
      version = "5.8.2"
    }
    azapi = {
      source  = "Azure/azapi"
      version = "2.5.0"
    }
  }
}
