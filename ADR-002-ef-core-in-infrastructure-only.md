# ADR-002: Why EF Core Is Restricted to Infrastructure Layer

---

Status: Accepted



Context

The system follows Clean Architecture principles where business logic must remain independent of external frameworks and technologies. Entity Framework Core is a persistence framework that introduces infrastructure-specific concerns such as tracking, migrations, and SQL translation. Allowing EF Core to leak into Domain or Application layers would tightly couple business logic to a specific ORM.



Decision

Entity Framework Core is used exclusively within the Infrastructure layer. The Domain layer contains only plain domain entities and value objects without EF Core dependencies. The Application layer interacts with persistence through abstractions (repositories and unit-of-work interfaces) without knowledge of EF Core or database details.



Consequences

Positive:



Persistence-agnostic business logic



Easier replacement or modification of data access technology



Improved testability using mocks or in-memory implementations



Clear architectural boundaries



Negative:



Additional abstraction layers to maintain



Slightly more upfront design effort