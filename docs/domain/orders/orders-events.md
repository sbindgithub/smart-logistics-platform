# Order Events

This document defines the **order-related events** emitted by the Orders bounded context.  
Events are expressed in **business language** and represent **facts that have already occurred**.

---

## Event Design Rules

- Events describe **what happened**, not **what should happen next**
- Events must be **immutable**
- Events must not contain assumptions about downstream processing
- Domain events are internal to the bounded context
- Only integration events are published outside the bounded context

---

## Domain Events

### 1. OrderCreated

**Trigger**  
An Order is instantiated in the **Draft** state.

**Purpose**
- Audit trail
- Operational analytics
- Internal tracking

**Typical Consumers**
- Reporting
- Monitoring

**Notes**
This event signals the existence of a new Order but does not imply readiness for processing.

---

### 2. OrderSubmitted

**Trigger**  
A user submits a Draft Order for processing.

**Purpose**
- Initiate validation workflows
- Signal intent to proceed with order processing

**Typical Consumers**
- Validation services
- Policy enforcement components

**Notes**
Submission does not guarantee validity or confirmation.

---

### 3. OrderConfirmed

**Trigger**  
An Order is successfully validated and transitions to the **Confirmed** state.

**Purpose**
- Initiate downstream logistics and commercial processes

**Typical Consumers**
- Shipment
- Inventory
- Billing

**Notes**
This is a **business-significant milestone** event.

---

### 4. OrderCancelled

**Trigger**  
An Order is cancelled before completion.

**Purpose**
- Stop or compensate downstream processing
- Ensure consistency across dependent systems

**Typical Consumers**
- All dependent bounded contexts

**Notes**
Cancellation is a terminal state and must be respected by all consumers.

---

## Integration Events

Integration events are derived from domain events and are published outside the Orders bounded context.

### Integration Events Emitted
- `OrderCreatedIntegrationEvent`
- `OrderConfirmedIntegrationEvent`
- `OrderCancelledIntegrationEvent`

Only integration events cross bounded context boundaries.  
Domain events remain internal and must not be consumed externally.

---

## Boundary Statement
The Orders bounded context communicates with other systems **only through events**.  
No external system is allowed to directly query or modify order state.
