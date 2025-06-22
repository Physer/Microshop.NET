terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = ">= 4.34.0"
    }
    random = {
      source  = "hashicorp/random"
      version = ">= 3.7.2"
    }
    cloudflare = {
      source  = "cloudflare/cloudflare"
      version = ">= 5.6.0"
    }
    azapi = {
      source  = "Azure/azapi"
      version = ">= 2.4.0"
    }
  }
  backend "azurerm" {
    key = "terraform.tfstate"
  }
}

provider "cloudflare" {
  api_token = var.cloudflare_api_token
}

provider "azurerm" {
  features {

  }
}
