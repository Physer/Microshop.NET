variable "container_app_id" {
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