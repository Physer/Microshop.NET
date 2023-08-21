terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = ">= 3.70.0"
    }
    random = {
      source  = "hashicorp/random"
      version = ">= 3.5.1"
    }
    cloudflare = {
      source  = "cloudflare/cloudflare"
      version = ">= 4.12.0"
    }
    azapi = {
      source  = "Azure/azapi"
      version = ">= 1.8.0"
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
