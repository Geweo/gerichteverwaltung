# Implementation Rules

## Data Access

- Use Entity Framework Core with PostgreSQL (npgsql) for data access
- Enforce usage of port services (like FileStorage) in application layer
- Use efficient query patterns to avoid common performance issues

## Logging

- Use Serilog for structured logging
- Log at appropriate levels (Debug, Information, Warning, Error)
- Include context in log messages

## Error Handling

- Handle edge cases and write clear exception handling
- Make only high confidence suggestions when reviewing code changes
- Write code with good maintainability practices, including comments on why certain design decisions were made

