Event name
Trigger
Payload (high level, no code yet)

OrderCreated

Trigger: Order instantiated in Draft state

Purpose: Audit, analytics

Consumers: Reporting, Monitoring

OrderSubmitted

Trigger: User submits a draft order

Purpose: Start validation workflows

Consumers: Validation services

OrderConfirmed

Trigger: Order successfully validated and confirmed

Purpose: Initiate downstream logistics processes

Consumers: Shipment, Inventory, Billing

OrderCancelled

Trigger: Order cancelled before completion

Purpose: Stop downstream processing

Consumers: All dependent contexts

Event Design Rule
Events describe what happened, not what to do.
No event contains downstream logic assumptions.