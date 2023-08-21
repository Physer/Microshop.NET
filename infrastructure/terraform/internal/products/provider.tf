terraform {
  required_providers {
    azapi = {
      source  = "Azure/azapi"
      version = ">= 1.8.0"
    }
    azurerm = {
      source  = "hashicorp/azurerm"
      version = ">= 3.70.0"
    }
  }
  backend "azurerm" {
    key = "terraform.tfstate"
  }
}

provider "azurerm" {
  features {

  }
}
