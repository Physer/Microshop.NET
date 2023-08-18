variable "allow_external_traffic" {
  type = bool
}

variable "application_name" {
  type = string
}

variable "appsettings" {
  type = list(object({
    name      = string,
    value     = optional(string),
    secretRef = optional(string)
  }))
  default = []
}

variable "container_app_environment_id" {
  type = string
}

variable "image_name" {
  type = string
}

variable "ingress_enabled" {
  type    = bool
  default = false
}

variable "location" {
  type    = string
  default = "West Europe"
}

variable "port" {
  type    = number
  default = 80
}

variable "resource_group_id" {
  type = string
}

variable "scale_max" {
  type    = number
  default = 2
}

variable "scale_min" {
  type    = number
  default = 0
}

variable "secrets" {
  type = list(object({
    name  = string
    value = string
  }))
  default = []
}

variable "transport" {
  type    = string
  default = "Auto"
}
