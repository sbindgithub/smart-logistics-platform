# Order Invariants

This document defines the **non-negotiable rules** that must **always** hold true for an Order.  
These invariants are enforced by the **Order aggregate** and cannot be bypassed by the API, application services, or external systems.

Without explicit invariants, the domain becomes **descriptive** rather than **authoritative**.

---

## What Must Always Be True

- An Order **MUST have at least one** OrderItem
- An Order **MUST have a valid OrderType**
- An Order **MUST have a positive total quantity and total value**
- An Order **MUST be created in a valid initial state** (Draft)
- An Order **MUST transition through states explicitly**

---

## What Is Forbidden

- An Order **MUST NOT be shipped before confirmation**
- An Order **MUST NOT be cancelled after shipment**
- An Order **MUST NOT allow modification of OrderItems after confirmation**
- An Order **MUST NOT bypass validation rules**
- An Order **MUST NOT be modified directly outside the aggregate**

---

## Rules That Cannot Be Bypassed

- Only the **Order aggregate** owns and controls state transitions
- All state changes **MUST occur through aggregate behaviors**
- Invalid state transitions **MUST be rejected at the domain layer**
- Business rules **MUST NOT be enforced in the API layer**
- Persistence mechanisms **MUST NOT override domain decisions**

---

## State Transition Ownership

- The **Order aggregate** is the single authority for:
  - State transitions
  - Invariant enforcement
  - Domain event emission
- Application services may **request** changes
- The aggregate decides whether the change is allowed

---

## Why This Matters

- Prevents inconsistent order states
- Eliminates hidden business logic
- Enables safe evolution toward event-driven architecture
- Guarantees system correctness under change

These invariants are **foundational**.  
If any invariant is violated, the operation **must fail immediately**.
