variable "application_name" {
  type = string
}

variable "container_app_environment_id" {
  type = string
}

variable "resource_group_name" {
  type = string
}

variable "image_name" {
  type = string
}

variable "appsettings" {
  type    = map(string)
  default = {}
}

variable "secrets" {
  type    = map(string)
  default = {}
}
