terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = ">= 3.61.0"
    }
    random = {
      source  = "hashicorp/random"
      version = ">= 3.5.1"
    }
    cloudflare = {
      source  = "cloudflare/cloudflare"
      version = ">= 4.0"
    }
    azapi = {
      source  = "Azure/azapi"
      version = ">= 1.7.0"
    }
  }
  backend "azurerm" {
    key = "terraform.tfstate"
  }
}