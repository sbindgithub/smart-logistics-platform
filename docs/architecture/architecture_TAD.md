# Technical Architecture Document (TAD)

## 1. Purpose
This document describes the overall technical architecture of the Smart Logistics & Order Management Platform.  
It serves as a single reference point for understanding system structure, responsibilities, and design principles.

---

## 2. Architectural Style
The system follows:

- Clean Architecture
- Domain-Driven Design (DDD)
- CQRS (Command Query Responsibility Segregation)
- Event-driven design (via Outbox pattern)

The architecture prioritizes:
- Clear separation of concerns
- Business rule protection
- Long-term maintainability over short-term convenience

---

## 3. High-Level Structure

```text
┌────────────┐
│   Client   │
└─────┬──────┘
      │ HTTP
┌─────▼──────┐
│     API    │  → Controllers, DTOs, Auth
└─────┬──────┘
      │ MediatR
┌─────▼──────┐
│ Application│  → Commands, Queries, Use cases
└─────┬──────┘
      │ Domain Calls
┌─────▼──────┐
│   Domain   │  → Aggregates, Invariants, Events
└─────┬──────┘
      │ Abstractions
┌─────▼──────┐
│Infrastructure│ → EF Core, DB, Outbox, Messaging
└────────────┘
```

## 4. Layer Responsibilities

### API Layer
- Accepts HTTP requests
- Performs request validation and authorization
- Converts HTTP requests into application commands and queries
- **Never contains business rules**

### Application Layer
- Orchestrates use cases
- Coordinates domain operations
- Contains no persistence or framework-specific logic
- Serves as the entry point via MediatR

### Domain Layer
- Contains core business logic
- Aggregates enforce all invariants
- Emits domain events
- Has **zero dependency on infrastructure**

### Infrastructure Layer
- Implements persistence and external integrations
- Owns EF Core, database schema, and transaction management
- Implements Outbox pattern and messaging
- Replaceable without impacting business logic

## 5. Constraints & Non-Goals

### Constraints
- Cross-aggregate transactions are not supported
- Eventual consistency is accepted
- Orders bounded context is the authoritative source of order state

### Non-Goals
- Real-time analytics
- Distributed transactions
- UI concerns beyond the API boundary

