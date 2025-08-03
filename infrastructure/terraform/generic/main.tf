terraform {
  required_providers {
    random = {
      source  = "hashicorp/random"
      version = ">= 3.7.2"
    }
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "4.38.1"
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
