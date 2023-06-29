```mermaid
sequenceDiagram
  actor Products Service

  activate Startup
  Products Service->>Startup: Start application

  activate Products Generator
  Startup->>Products Generator: GenerateProducts(amount)
  Products Generator-->>Startup: Fake Products
  deactivate Products Generator

  activate Products Repository
  Startup->>Products Repository: CreateProducts(products)
  Note right of Products Repository: Stored in-memory
  deactivate Products Repository

  deactivate Startup
```
