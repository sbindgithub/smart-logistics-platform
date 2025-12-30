Purpose of Orders BC
Responsibilities
Explicit exclusions
Glossary (Order, OrderItem, SLA, etc.)

Purpose
The Orders bounded context is responsible for managing the complete lifecycle of customer orders, from creation through confirmation or cancellation. It represents the commercial intent to move goods and acts as the initiating context for downstream logistics processes.

Business Responsibility
This context owns:

Order creation and validation

Order lifecycle and state transitions

Enforcement of order-related business rules

Emission of order domain events

This context does not execute logistics operations.

Explicit Exclusions
The following concerns are intentionally outside this bounded context:

Shipment planning or execution

Inventory reservation or allocation

Billing and invoicing

Customer notifications

These are handled by separate bounded contexts that react to order domain events.

Ubiquitous Language (Glossary)

Order: A commercial request to move goods under defined conditions

OrderItem: A line item representing a unit of goods within an order

OrderStatus: The current lifecycle state of an order

OrderType: Classification such as Inbound or Outbound

SLA: Service-level commitment associated with an order

Priority: Business importance of the order