locals {
  rabbitmq_secrets = [
    { name = (local.rabbitmq_username), value = random_string.rabbitmq_username.result },
    { name = (local.rabbitmq_password), value = random_password.rabbitmq_password.result }
  ]
  rabbitmq_appsettings = [
    { name = "RABBITMQ_DEFAULT_USER", secretRef = local.rabbitmq_username },
    { name = "RABBITMQ_DEFAULT_PASS", secretRef = local.rabbitmq_password }
  ]
}