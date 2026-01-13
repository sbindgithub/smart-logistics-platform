# ADR-003: Why MediatR Is Used for Application Flow

---

## Status
Accepted

---

## Context
The application requires a consistent mechanism to handle commands, queries, and cross-cutting concerns such as validation, logging, and transaction management. Direct service-to-service invocation leads to tightly coupled code and makes it difficult to apply behaviors consistently across requests.

---

## Decision
MediatR is used as an in-process mediator to decouple request initiation from request handling.  
All commands and queries flow through MediatR, enforcing a single entry point into application logic.  
Pipeline behaviors are used to implement cross-cutting concerns such as validation, logging, and transaction management.

MediatR is treated as an implementation detail rather than a hard architectural dependency. The overall architecture remains valid even if MediatR is replaced.

---

## Consequences

### Positive
- Loose coupling between controllers and application logic
- Centralized handling of cross-cutting concerns
- Improved testability of handlers in isolation
- Clean enforcement of CQRS boundaries

### Negative
- Additional dependency
- Slight performance overhead (negligible for typical workloads)
- Requires discipline to prevent business logic from leaking into handlers
