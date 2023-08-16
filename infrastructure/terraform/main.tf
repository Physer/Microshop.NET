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

provider "azurerm" {
  features {}
}

provider "cloudflare" {
  api_token = var.cloudflare_api_token
}

resource "azurerm_resource_group" "rg_microshop" {
  name     = "rg-microshop-${var.environment}"
  location = var.location
}

resource "azurerm_container_app_environment" "cae_microshop" {
  name                       = "cae-microshop-${var.environment}"
  location                   = azurerm_resource_group.rg_microshop.location
  resource_group_name        = azurerm_resource_group.rg_microshop.name
  log_analytics_workspace_id = azurerm_log_analytics_workspace.log_analytics.id
}
