# ADR-002: Why EF Core Is Restricted to Infrastructure Layer

---

## Status
Accepted

---

## Context
The system follows Clean Architecture principles where business logic must remain independent of external frameworks and technologies. Entity Framework Core is a persistence framework that introduces infrastructure-specific concerns such as change tracking, migrations, SQL translation, and transaction management. Allowing EF Core to leak into the Domain or Application layers would tightly couple business logic to a specific ORM and weaken architectural boundaries.

---

## Decision
Entity Framework Core is used exclusively within the Infrastructure layer.  
The Domain layer contains only plain domain entities and value objects with no EF Core dependencies.  
The Application layer interacts with persistence through abstractions such as repositories and unit-of-work interfaces, without any knowledge of EF Core or database-specific details.  
Infrastructure owns database schema evolution, migrations, and transaction boundaries.

---

## Consequences

### Positive
- Persistence-agnostic business logic
- Easier replacement or modification of data access technology
- Improved testability using mocks or in-memory implementations
- Clear and enforceable architectural boundaries

### Negative
- Additional abstraction layers to maintain
- Slightly more upfront design effort
