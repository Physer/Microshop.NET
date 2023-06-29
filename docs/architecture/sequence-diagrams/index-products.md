```mermaid
sequenceDiagram
  activate Indexing Service
  activate Servicebus

  actor Cron
  Cron->>Indexing Service: Trigger

  Indexing Service->>Servicebus: GetProductsRequest

  activate Products Service
  Servicebus->>Products Service: GetProductsResponse
  Products Service-->>Servicebus: GetProductsResponse
  deactivate Products Service

  Servicebus-->>Indexing Service: GetProductsResponse

  activate Search index
  Indexing Service->>Search index: DeleteDocuments
  Indexing Service->>Search index: AddDocuments(products)

  deactivate Servicebus
  deactivate Indexing Service
  deactivate Search index
```
