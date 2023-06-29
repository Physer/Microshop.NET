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

## Products service

### Overview

The Products service is responsible for storing and accessing product data.
This only includes directly related product data, so things like prices and stock information is not part of this service.

The Products service generates a pre-configured amount of fake products when the service starts and stores these in-memory.
It then listens to messages on the servicebus when product data is requested.
If a message comes in, it will retrieve the product data from the in-memory database and send it back to the servicebus. The Products service is a console application.

### Tests:

- Unit Tests

### Libraries used:

- Autobogus
- Autofixture
- Automapper
- FluentAssertions
- MassTransit (RabbitMQ)
- NSubstitute
- XUnit

## Indexing service

### Overview

The Indexing service is responsible for retrieving product data and storing the data in a search index.

The Indexing service is a [background service](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-7.0&tabs=visual-studio) that is triggered periodically by a configured timed interval. Upon triggering, the service will request all product data through the servicebus and, once received, store the data in an external search index.

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

The infrastructure is being provisioned automatically through the use of our [Github Actions](https://github.com/Physer/Microshop.NET/actions).

# Deployments

Whenever a push is done to the `main` branch, all services are being built, tested and published to the Docker Hub.
These actions are done in parallel per service.
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

# Questions and comments

Do you have any questions or comments about this project?
Feel free to open [an issue](https://github.com/Physer/Microshop.NET/issues).
You can always contact me at [alex_schouls@live.com](mailto:alex_schouls@live.com).

https://alexschouls.com