# Coding Style Rules

## C# Style

- Use PascalCase for public members, methods, classes
- Use _camelCase for private fields
- Use camelCase for local variables
- Prefix interfaces with "I" (e.g., IUserService)
- Use file-scoped namespaces
- Insert newline before opening curly brace

## Formatting

- Apply code-formatting style defined in `.editorconfig`
- Use pattern matching and switch expressions wherever possible
- Use `nameof` instead of string literals when referring to member names
- Ensure that XML doc comments are created for any public APIs

## Nullable Reference Types

- Declare variables non-nullable, and check for `null` at entry points
- Always use `is null` or `is not null` instead of `== null` or `!= null`
- Trust the C# null annotations

