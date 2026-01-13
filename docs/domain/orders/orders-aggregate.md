# Order Aggregate

## Aggregate Root
**Order** is the aggregate root.  
All access to order-related data occurs exclusively through the `Order` entity.

External components must not modify order state directly and must interact only via commands handled by the aggregate.

---

## Aggregate Structure (Conceptual)

```text
Order
 ├── OrderId
 ├── CustomerId        (reference only)
 ├── OrderType
 ├── OrderStatus
 ├── Priority
 ├── SLA
 ├── CreatedDate
 └── OrderItems        (collection)
```

## Invariants (Must Always Hold True)

- An Order must contain at least one OrderItem
- OrderItems cannot be modified once the Order is Confirmed
- A Cancelled Order is immutable
- Order total quantity and value must be greater than zero
- SLA rules vary by OrderType and must be validated at submission
- If any invariant is violated, the operation must fail immediately.

## Order Lifecycle & State Transitions

- Allowed transitions only:
- Draft → Submitted
- Submitted → Validated
- Validated → Confirmed
- Draft / Submitted / Validated → Cancelled
- Invalid transitions must be rejected at the domain layer, not the API layer.

## Why this matters
- Status is not a database column to be “updated.”
- It is a state machine governed by business rules.

# Order Aggregate

## Aggregate Root
Order

## Rules
- Only Order aggregate may change state
- All invariants are enforced internally
- External systems interact via commands

## Example
```csharp
public void Confirm()
{
    if (Status != OrderStatus.Created)
        throw new DomainException("Invalid transition");

    Status = OrderStatus.Confirmed;
    AddDomainEvent(new OrderConfirmed(Id));
}
```
