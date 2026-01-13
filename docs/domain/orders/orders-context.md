# Orders Bounded Context

## Purpose
The Orders bounded context is responsible for managing the complete lifecycle of customer orders, from creation through confirmation or cancellation.  
It represents the **commercial intent to move goods** and acts as the initiating context for downstream logistics processes.

Orders define *what* needs to happen; they do not execute *how* it happens operationally.

---

## Business Responsibilities
This bounded context owns:

- Order creation and validation
- Order lifecycle and state transitions
- Enforcement of order-related business rules
- Emission of order domain events

The Orders bounded context is the **authoritative source of truth** for order state.

---

## Explicit Exclusions
The following concerns are intentionally **out of scope** for this bounded context:

- Shipment planning or execution
- Inventory reservation or allocation
- Billing and invoicing
- Customer notifications

These responsibilities are handled by **separate bounded contexts** that react to order domain or integration events.

---

## Ubiquitous Language (Glossary)

- **Order**  
  A commercial request to move goods under defined business conditions.

- **OrderItem**  
  A line item representing a unit of goods within an order.

- **OrderStatus**  
  The current lifecycle state of an order (e.g., Draft, Submitted, Confirmed).

- **OrderType**  
  Classification of the order, such as Inbound or Outbound.

- **SLA (Service Level Agreement)**  
  A service-level commitment associated with an order, defining time or quality constraints.

- **Priority**  
  The business importance assigned to an order, influencing processing behavior.

---

## Boundary Statement
The Orders bounded context defines **intent and commitment**, not execution.  
Any system that requires order information must integrate via published events rather than direct access.
