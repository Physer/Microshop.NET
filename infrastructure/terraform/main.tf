terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.61.0"
    }
    random = {
      source  = "hashicorp/random"
      version = "~> 3.5.1"
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
