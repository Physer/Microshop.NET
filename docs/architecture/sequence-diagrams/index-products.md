```mermaid
sequenceDiagram
  actor Servicebus
  activate Message Consumer
  activate Indexing Service
  Servicebus->>Message Consumer: Consume message (ProductsGenerated)
  Message Consumer->>Indexing Service: IndexProducts(products)
  deactivate Message Consumer

  activate Search index
  Indexing Service->>Search index: DeleteDocuments
  Indexing Service->>Search index: AddDocuments(products)

  deactivate Indexing Service
  deactivate Search index
```
