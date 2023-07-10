# Microshop.NET

# Introduction

Microshop.NET is a showcase for an e-commerce web shop with a MACH-based architecture.
The system consists out of multiple microservices that communicate asynchronously.

The purpose of this application is to demo a complete setup of a MACH architecture using .NET and containerized applications.
It will also contain different layers of testing and automated deployments, as well as automated resource provisioning.

# Service overview

## Microservice template

Every microservice in this project has been based of the same Visual Studio solution template.
This keeps the services consistent and allows for a clear structure.
The template used is [Microtemplate](https://github.com/Physer/Microtemplate).
Microtemplate has specifically been designed to make it easy to create a new .NET-based microservice on Clean Architecture.

More information about this choice can be found in an [Architecture Decision Record (ADR)](./docs/ADRs/microtemplate-for-project-setup.md)

## Gateway

### Overview

The Gateway is a reverse proxy that allows an API aggregation layer to communicate with different downstream services.
The Gateway is set-up using [Microsoft's YARP](https://microsoft.github.io/reverse-proxy/index.html)

All inbound traffic to Microshop.NET will go through the Gateway, never directly to downstream services.
Note that this does not apply to service-to-service communication, only for external communication (e.g. a front-end).

## Products service

### Overview

The Products service is responsible for storing and accessing product data.
This only includes directly related product data, so things like prices and stock information is not part of this service.

The Products service generates a pre-configured amount of fake products when the service starts.
When the products have been generated, the service publishes a message on the servicebus with all product data. The Products service is a console application.

### Tests:

- Unit Tests
- Integration Tests
  - Using [Testcontainers.NET](https://dotnet.testcontainers.org/) to simulate the complete environment.

### Libraries used:

- Autobogus
- Autofixture
- FluentAssertions
- MassTransit (RabbitMQ)
- NSubstitute
- Testcontainers
- XUnit

## Indexing service

### Overview

The Indexing service is responsible for retrieving product data and storing the data in a search index.

The Indexing service is a console application that listens for incoming messages. Once a message is received, the indexing service will store the data in an external search index.

### Tests:

- Unit Tests
- Integration Tests
  - Using [Testcontainers.NET](https://dotnet.testcontainers.org/) to simulate the complete environment.

### Libraries used:

- Autofixture
- Automapper
- FluentAssertions
- MassTransit (RabbitMQ)
- MeiliSearch
- NSubstitute
- Testcontainers
- XUnit

## Servicebus

The servicebus is a [RabbitMQ](https://www.rabbitmq.com/) application.
The applications interface with the RabbitMQ servicebus through the [MassTransit](https://masstransit.io/) framework.

## Search index

The search index is a [MeiliSearch](https://www.meilisearch.com/) index. The applications interface using their .NET SDK.

# Communication between services

All communication between services happen asynchronously to prevent tight coupling and cascading outages of services.
In order to achieve this, [a RabbitMQ servicebus](#servicebus) is used, leveraging the AMQP standard.

Services will remain agnostic of each other and will only ever know about the data they request, creating clear boundaries between services and reducing coupling.

For more information about this decision, please refer to the [Architecture Decision Record (ADR)](./docs/ADRs/amqp-for-interservices-communication.md)

# Tests

Every service has a complete set of automated tests, sufficient for complete confidence in automated deployments of the service without degrading quality.

Depending on the service, this might include Unit Tests, Integration Tests, End-to-End Tests and Visual Regression Tests.

# Containerization

Every application or tool used for this project is runnable through a container.
Every external application also has this prerequisite.

Microshop.NET's services are available as [Docker images on the Docker Hub](https://hub.docker.com/u/physer)

# Infrastructure

The infrastructure is hosted in Azure through [Azure Container Apps](https://learn.microsoft.com/en-us/azure/container-apps/overview).
Azure Container Apps is a managed solution for hosting microservices on a serverless platform. It's using Kubernetes and related technologies under the hood but they don't require intervention or managing.

Using Azure Container Apps over Kubernetes (or Azure Kubernetes Services) was a deliberate choice and the reasoning can be found in an [Architecture Decision Record (ADR)](./docs/ADRs/deploying-azure-container-apps.md)

All infrastructure is setup as Infrastructure-as-Code by using [Terraform](https://www.terraform.io/).

The infrastructure is being provisioned automatically through the use of [Github Actions](https://github.com/Physer/Microshop.NET/actions).

# Deployments

Whenever a push is done to the `main` branch, all necessary services are being built, tested and published to the Docker Hub.
These actions are done in parallel per service.
Whether or not a new version of the service is built and deployed depends on the changes files in the commit.
See the [deploy.yaml workflow file](./.github/workflows/deploy.yaml) for more information.

After these have all succeeded, the Azure Container Apps are provisioned with the latest Docker images through Terraform.

# Running the platform locally

If you wish to run the platform locally, follow these steps:

1. Clone the repository.
2. Navigate to the root folder (`~`).
3. Copy `.env.example` and rename it to `.env.`
4. Fill in the necessary values in the `.env` file
5. Run `docker-compose up`
6. See the Docker container overview and their logs for the results

Upon the start of the platform, products will be generated and indexed in the Search Index.
If you so desire, you can change the MeiliSearch docker container to expose port `7700` to view the indexed data.

# Deploying the platform to your own Azure tenant

If you want to host this platform on your own Azure tenant, follow these steps:

_Note: Microshop.NET uses Terraform remote state in an Azure Storage Account. These steps assume you're able to set up such an account and its containers accordingly._
1. Navigate to the `~/infrastructure/terraform` directory
2. Open the `config.azure.tfbackend` file
3. Either set up an Azure Storage Account with the specified Terraform state data or change the data to match your remote state   storage in Azure
4. Initialize the Terraform state by running `terraform init -backend-config="config.azure.tfbackend"` (or omit the argument if not leveraging remote state)
5. Plan the Terraform deployment by executing `terraform plan`
6. Apply the Terraform deployment by executing `terraform apply`

Optionally you can add a file `terraform.tfvars` to this folder that includes predefined variable values, so you don't have to specify them when you run the `terraform` commands.

# Questions and comments

Do you have any questions or comments about this project?
Feel free to open [an issue](https://github.com/Physer/Microshop.NET/issues).
You can always contact me at [alex_schouls@live.com](mailto:alex_schouls@live.com).

https://alexschouls.com
