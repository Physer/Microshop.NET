# Microshop.NET

# Introduction

Microshop.NET is a showcase for an e-commerce web shop with a MACH-based architecture.
The system consists out of multiple microservices that communicate asynchronously.

The purpose of this application is to demo a complete setup of a MACH architecture using .NET and containerized applications.
It will also contain different layers of testing and automated deployments, as well as automated resource provisioning.

# Service overview

## Service requirements

Generally services are ASP.NET Core APIs. Even if they will not have endpoints, API projects are preferred due to their integration with testing utilities and their splendid Cloud support and containerization.

All communication between services happen asynchronously to prevent tight coupling and cascading outages of services.
In order to achieve this, [a RabbitMQ servicebus](#servicebus) is used, leveraging the AMQP standard. Services will remain agnostic of each other and will only ever know about the data they request, creating clear boundaries between services and reducing coupling. For more information about this decision, please refer to the [Architecture Decision Record (ADR)](./docs/ADRs/amqp-for-interservices-communication.md)

### Microservice template

Every microservice in this project has been based of the same Visual Studio solution template.
This keeps the services consistent and allows for a clear structure.
The template used is [Microtemplate](https://github.com/Physer/Microtemplate).
Microtemplate has specifically been designed to make it easy to create a new .NET-based microservice on Clean Architecture.

More information about this choice can be found in an [Architecture Decision Record (ADR)](./docs/ADRs/microtemplate-for-project-setup.md)

### Testing

Every service has the requirement of at least 70% code coverage for both lines and branches. This is verified upon push. The automatically triggered Github Action will fail if any of the relevant services does not adhere to the 70% minimum.

Additionally, every service has both Unit Tests and Integration Tests. These Test projects are written using [XUnit](https://xunit.net/) as a testing framework and [NSubstitute](https://nsubstitute.github.io/) as a mocking framework.

In order to quickly view the test coverage locally, a Powershell script is available in the root (`~`) folder of the repository: `generateTestCoverageReport.ps1`. This file requires you to have the [dotnet-coverage utility tool](https://learn.microsoft.com/en-us/dotnet/core/additional-tools/dotnet-coverage) installed, as well as the [ReportGenerator utility tool](https://www.nuget.org/packages/dotnet-reportgenerator-globaltool). To run the command, supply the name of a service as an argument, e.g. `./generateTestsCoverageReport.ps1 products`.

### Unit Tests

Unit Tests should test the smallest blocks of code possible. These tests can mock their dependencies.

### Integration Tests

Integration Tests should test the application's flow as close to real-life as possible. To facilitate this, the Integration Tests use [Testcontainers .NET](https://dotnet.testcontainers.org/). These `Testcontainers` allow the tests to spin up Docker containers and use them in their tests.

## Gateway service

### Overview

The Gateway is a reverse proxy that acts as an API aggregation layer to communicate with different downstream services.
The Gateway is set-up using [Microsoft's YARP](https://microsoft.github.io/reverse-proxy/index.html)

All inbound traffic to Microshop.NET will go through the Gateway, never directly to downstream services.
Note that this does not apply to service-to-service communication, only for external communication (e.g. a front-end).

## Admin portal

### Overview

The Admin portal is a ASP.NET Razor Pages application that is responsible for interfacing with the API service and Authentication service to handle administrative actions (e.g. generate data or create a user).

## Authentication service

### Overview

The Authentication is an application written in [Go](https://go.dev/) as a backend to SuperTokens, a self-hosted open-source authentication service. To read more about this choice, please refer to the [Architecture Decision Record (ADR)](./docs/ADRs/supertokens-for-authentication.md).

This service is responsible for bootstrapping the authentication dashboard as well as all the authentication and authorization API endpoints that are being exposed through the [YARP Gateway](#gateway-service).

## API

### Overview

The API is responsible for any user interaction with the downstream services. For instance, this API is used to let the Admin portal interface with the downstream services using events.

### Supported messages

#### Consuming

- None

#### Publishing

- GenerateProducts

## Products service

### Overview

The Products service is responsible for generating and storing product data.
This only includes directly related product data, so things like prices and stock information is not part of this service.

### Supported messages

#### Consuming

- GenerateProducts

#### Publishing

- ProductsGenerated

## Indexing service

### Overview

The Indexing service is responsible for receiving data and indexing it into the search index. The Indexing service listens for incoming messages. Once a message is received, the indexing service will store the data in an external search index.

### Supported messages

#### Consuming

- ProductsGenerated
- PricesGenerated

#### Publishing

- None

## Price service

### Overview

The Price service is responsible for generating and storing price data.
This includes price data only, so no other product assets - besides the product's identifier are available to this service.

The Price service listens to an incoming `ProductsGenerated` message and uses the products inside that message to generate prices for. After the prices are generated, the price data is published as a `PricesGenerated` message, including the price data, on the servicebus.

### Supported messages

#### Consuming

- ProductsGenerated

#### Publishing

- PricesGenerated

## Servicebus

The servicebus is a [RabbitMQ](https://www.rabbitmq.com/) application.
The applications interface with the RabbitMQ servicebus through the [MassTransit](https://masstransit.io/) framework.

## Search index

The search index is a [MeiliSearch](https://www.meilisearch.com/) index. The applications interface using their .NET SDK.

# Message overview

## GenerateProducts

The GenerateProducts message is sent out when the API endpoint to generate product data is called. This message doesn't contain any data directly but instead is used as a trigger for other services.

Example payload:

```json
{
  "messageId": "58010000-eace-e070-f3b8-08dba31489f0",
  "requestId": null,
  "correlationId": null,
  "conversationId": "58010000-eace-e070-fb23-08dba31489f0",
  "initiatorId": null,
  "sourceAddress": "rabbitmq://localhost/NL524_API_bus_myyoyy8k35o8ywk1bdp4gr6rns?temporary=true",
  "destinationAddress": "rabbitmq://localhost/Messaging.Messages:GenerateProducts",
  "responseAddress": null,
  "faultAddress": null,
  "messageType": ["urn:message:Messaging.Messages:GenerateProducts"],
  "message": {}
}
```

## ProductsGenerated

The ProductsGenerated message is sent out whenever product master data is generated. This message contains all products and their data.

Example payload:

```json
{
  "messageId": "07000000-ac12-0242-86c8-08db862bddb2",
  "requestId": null,
  "correlationId": null,
  "conversationId": "07000000-ac12-0242-8ea9-08db862bddb2",
  "initiatorId": null,
  "sourceAddress": "rabbitmq://rabbitmq/76d617f6222b_API_bus_yhyyyyfcnebrfufqbdpack7wyh?temporary=true",
  "destinationAddress": "rabbitmq://rabbitmq/Messaging.Messages:ProductsGenerated",
  "responseAddress": null,
  "faultAddress": null,
  "messageType": ["urn:message:Messaging.Messages:ProductsGenerated"],
  "message": {
    "products": [
      {
        "code": "9132327080890",
        "name": "Refined Steel Shoes",
        "description": "The beautiful range of Apple Naturalé that has an exciting mix of natural ingredients. With the Goodness of 100% Natural Ingredients"
      }
    ]
  }
}
```

## PricesGenerated

The PricesGenerated message is sent out whenever price data is generated. This message contains all prices per product.

Example payload:

```json
{
  "messageId": "07000000-ac12-0242-4d6b-08db862c2c2c",
  "requestId": null,
  "correlationId": null,
  "conversationId": "07000000-ac12-0242-22d0-08db862c2c2b",
  "initiatorId": null,
  "sourceAddress": "rabbitmq://rabbitmq/pricing_products_generated",
  "destinationAddress": "rabbitmq://rabbitmq/Messaging.Messages:PricesGenerated",
  "responseAddress": null,
  "faultAddress": null,
  "messageType": ["urn:message:Messaging.Messages:PricesGenerated"],
  "message": {
    "prices": [
      {
        "productCode": "9132327080890",
        "value": "595.32",
        "currency": "EUR"
      }
    ]
  }
}
```

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

The YARP gateway is now available with the following URLs:
- `http://auth.localhost` - routing to the Authentication service
- `http://api.localhost` - routing to the API
- `http://admin.localhost` - routing to the Admin portal web interface
- `http://index.localhost` - routing to the Meilisearch Index web interface

# Deploying the platform to your own Azure tenant

If you want to host this platform on your own Azure tenant, follow these steps:

**Prerequisites:**
- A Cloudflare account with a website to provision DNS records for
- An Azure account with a pre-configured storage account for storing Terraform state in

If these details are not provided, the applications won't fully be provisioned.

---
_WIP_ A useful script to kickstart this locally is under development.

# Questions and comments

Do you have any questions or comments about this project?
Feel free to open [an issue](https://github.com/Physer/Microshop.NET/issues).
You can always contact me at [alex_schouls@live.com](mailto:alex_schouls@live.com).

https://alexschouls.com
