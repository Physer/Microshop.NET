terraform {
  required_providers {
    random = {
      source  = "hashicorp/random"
      version = ">= 3.7.2"
    }
    azapi = {
      source  = "Azure/azapi"
      version = ">= 2.4.0"
    }
    azurerm = {
      source  = "hashicorp/azurerm"
      version = ">= 3.69.0"
    }
  }
  backend "azurerm" {
    key = "terraform.tfstate"
  }
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "rg_microshop" {
  name     = "rg-microshop-${var.environment}"
  location = var.location
}
