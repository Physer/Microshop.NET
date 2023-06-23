terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.61.0"
    }
    azuread = {
      source  = "hashicorp/azuread"
      version = "~> 2.15.0"
    }
    random = {
      source  = "hashicorp/random"
      version = "3.5.1"
    }
  }
  backend "azurerm" {
    key = "terraform.tfstate"
  }
}

data "azurerm_client_config" "client_config" {}
data "azuread_service_principal" "service_principal" {
  display_name = "schouls_enterprise_mpn"
}
data "azuread_user" "alex" {
  user_principal_name = "alex.schouls@valtech.com"
}

provider "azurerm" {
  features {
    key_vault {
      purge_soft_delete_on_destroy = true
    }
  }
}

provider "azuread" {
  tenant_id = data.azurerm_client_config.client_config.tenant_id
}

resource "azurerm_resource_group" "rg_microshop" {
  name     = "rg-microshop-${var.environment}"
  location = var.location
}
