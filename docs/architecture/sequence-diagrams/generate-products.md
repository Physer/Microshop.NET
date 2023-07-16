```mermaid
sequenceDiagram  
  activate API
  activate Products service
  API->>Products service: POST /products
  deactivate API

  Products service->>Products service: GenerateProducts()
  deactivate Products service
  
  activate Servicebus
  Products service->>Servicebus: ProductsGenerated
  
  activate Indexing service
  Servicebus->>Indexing service: ProductsGenerated
  Indexing service->>Indexing service: IndexAsync(products)
  deactivate Indexing service

  activate Pricing service
  Servicebus->>Pricing service: ProductsGenerated
  Pricing service->>Pricing service: GeneratePrices(products)
  Pricing service->>Servicebus: PricesGenerated
  deactivate Pricing service

  activate Indexing service
  Servicebus->>Indexing service: PricesGenerated
  Indexing service->>Indexing service: IndexAsync(prices)
  deactivate Indexing service
  
  deactivate Servicebus
```