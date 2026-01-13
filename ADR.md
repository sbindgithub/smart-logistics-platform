**ADR-001: Why CQRS**

Status: Accepted

Context
The Smart Logistics platform must support complex order workflows, high read volume, and future extensibility toward event-driven and distributed architectures. Traditional CRUD-based service design tends to couple read and write concerns, making validation, scalability, and change isolation harder over time. The system is expected to evolve with additional read models, reporting needs, and integration events.

Decision
We adopt the Command Query Responsibility Segregation (CQRS) pattern for the Orders bounded context. Commands are used exclusively for state-changing operations and enforce business rules, while queries are used only for data retrieval and are optimized independently. Commands and queries are handled via dedicated handlers using a mediator-based approach.

Consequences
Positive:

Clear separation of intent (commands vs queries)

Improved maintainability and testability

Easier introduction of validation, logging, and cross-cutting concerns

Natural alignment with event-driven and outbox patterns

Negative:

Increased number of classes and files

Higher learning curve for developers unfamiliar with CQRS

Requires discipline to prevent logic leakage into queries

ADR-002: Why EF Core Is Restricted to Infrastructure Layer

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

ADR-003: Why MediatR Is Used for Application Flow

Status: Accepted

Context
The application requires a consistent way to handle commands, queries, and cross-cutting concerns such as validation, logging, and transaction management. Direct service-to-service invocation leads to tightly coupled code and makes it difficult to apply behaviors consistently across requests.

Decision
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