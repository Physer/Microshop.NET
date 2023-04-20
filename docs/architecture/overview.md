```mermaid
flowchart TD
    user(((User))) --> frontend["Front-end application"]

    frontend <--> ingress["Ingress"]

    ingress --> authentication["Authentication service (Supertokens)"]

    ingress --> products["Products API"]
    ingress --> orders["Orders API"]
    ingress --> pricing["Pricing API"]

    products <--> servicebus>"Servicebus (RabbitMQ)"]
    orders <--> servicebus
    pricing <--> servicebus

    indexing["Indexing service (Meilisearch)"] <--> servicebus
```