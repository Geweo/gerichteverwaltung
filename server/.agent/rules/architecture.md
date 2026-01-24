# Architecture Rules

## Hexagonal Architecture

The application uses hexagonal architecture (Ports and Adapters):

- **Parts** (Ernaehrbar.Parts): Core domain and application logic
  - Contains Ports (interfaces) that define what the application needs
  - Contains Commands and CommandHandlers (MediatR; Implementierung der Anwendungsfälle)
  - Has NO dependencies on Adapters

- **Adapters.Infrastructure** (Ernaehrbar.Adapters.Infrastructure): Infrastructure implementations
  - Implements ports like IRecipeStorage, IBringExporter, ILLMService
  - Uses EF Core, PostgreSQL, external services
  - Depends on Parts (to implement ports)

- **Adapters.Api** (Ernaehrbar.Adapters.Api): API layer
  - Controllers, Middleware
  - Depends on Parts (sends Commands via MediatR)
  - Depends on Adapters.Infrastructure (to wire up implementations)

- **Api** (Ernaehrbar.Api): Entry point
  - Program.cs, dependency injection setup
  - Wires everything together

## Dependency Direction

Dependencies always point inward:
- Parts ← Adapters.Infrastructure
- Parts ← Adapters.Api
- Adapters.Api ← Api

Parts should NEVER depend on Adapters.

