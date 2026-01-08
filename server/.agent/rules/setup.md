# Setup Rules

## Project Structure

- Use hexagonal architecture (Parts and Adapters)
- Parts contain domain logic and use cases
- Adapters implement ports for infrastructure and API
- Keep dependencies pointing inward (Parts have no dependencies on Adapters)

## Dependencies

- Always use the latest .NET 10 features
- Use C# 14 language features
- Prefer file-scoped namespaces
- Use implicit usings

