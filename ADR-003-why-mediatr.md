# ADR-003: Why MediatR Is Used for Application Flow

---

## Status: Accepted



### Context

The application requires a consistent way to handle commands, queries, and cross-cutting concerns such as validation, logging, and transaction management. Direct service-to-service invocation leads to tightly coupled code and makes it difficult to apply behaviors consistently across requests.



### Decision

We use MediatR as the in-process mediator to decouple request initiation from request handling. All commands and queries flow through MediatR, enabling the use of pipeline behaviors for cross-cutting concerns and enforcing a single entry point into application logic.



Consequences

Positive:



Loose coupling between controllers and application logic



Centralized handling of cross-cutting concerns



Improved testability of handlers in isolation



Clean enforcement of CQRS boundaries



Negative:



Additional dependency



Slight performance overhead (negligible for typical workloads)



Requires discipline to prevent business logic from moving into handlers incorrectly
