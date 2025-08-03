variable "environment" {
  type = string
}

variable "location" {
  type    = string
  default = "West Europe"
}

variable "rabbitmq_docker_image_version" {
  description = "The docker image version of the RabbitMQ servicebus"
  type        = string
  default     = "latest"
}

variable "meilisearch_docker_image_version" {
  description = "The docker image version of the Meilisearch index"
  type        = string
  default     = "latest"
}
