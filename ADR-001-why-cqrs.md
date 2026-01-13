# ADR-001: Why CQRS

---

## Status: Accepted



### Context

The Smart Logistics platform must support complex order workflows, high read volume, and future extensibility toward event-driven and distributed architectures. Traditional CRUD-based service design tends to couple read and write concerns, making validation, scalability, and change isolation harder over time. The system is expected to evolve with additional read models, reporting needs, and integration events.



### Decision

We adopt the Command Query Responsibility Segregation (CQRS) pattern for the Orders bounded context. Commands are used exclusively for state-changing operations and enforce business rules, while queries are used only for data retrieval and are optimized independently. Commands and queries are handled via dedicated handlers using a mediator-based approach.



##### Consequences

###### Positive:



Clear separation of intent (commands vs queries)



Improved maintainability and testability



Easier introduction of validation, logging, and cross-cutting concerns



Natural alignment with event-driven and outbox patterns



###### Negative:



Increased number of classes and files



Higher learning curve for developers unfamiliar with CQRS



Requires discipline to prevent logic leakage into queries


