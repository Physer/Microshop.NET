terraform {
  required_providers {
    azapi = {
      source  = "Azure/azapi"
      version = ">= 1.8.0"
    }
  }
  backend "azurerm" {
    key = "terraform.tfstate"
  }
}
