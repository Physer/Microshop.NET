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

variable "ingress_enabled" {
  type    = bool
  default = false
}

variable "port" {
  type    = number
  default = null
}

variable "transport" {
  type    = string
  default = "auto"
}

variable "scale_max" {
  type    = number
  default = 1
}

variable "scale_min" {
  type    = number
  default = 1
}

variable "allow_insecure" {
  type    = bool
  default = false
}

variable "revision_suffix" {
  type    = string
  default = null
}

variable "allow_external_traffic" {
  type    = bool
  default = false
}
