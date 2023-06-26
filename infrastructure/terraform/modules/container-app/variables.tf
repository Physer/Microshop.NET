variable "application_name" {
  type = string
}

variable "container_app_environment_id" {
  type = string
}

variable "resource_group_id" {
  type = string
}

variable "location" {
  type    = string
  default = "West Europe"
}

variable "image_name" {
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

variable "secrets" {
  type = list(object({
    name  = string
    value = string
  }))
  default = []
}

variable "is_external" {
  type    = bool
  default = false
}

variable "target_port" {
  type    = number
  default = 443
}

variable "transport" {
  type    = string
  default = "auto"
}

variable "scale_max" {
  type    = number
  default = 10
}

variable "scale_min" {
  type    = number
  default = 0
}
