---
trigger: always_on
---

# client-project-rules.md

## Purpose

This file is for AI coding agents working on this project.

Follow the rules below when reading, editing, or adding code to this repository.
When in doubt, **prefer existing patterns and utilities over inventing new ones.**

---

## Project overview

- Type: Single Page Application (SPA)
- Framework: React 19 + TypeScript (strict mode)
- Routing: [TanStack Router](https://tanstack.com/router)
- Data fetching & caching: [TanStack Query](https://tanstack.com/query)
- API contracts:
  - Generated TypeScript types and clients live in `src/generated/api`
  - All network requests must go through these generated clients, not raw `fetch` or custom HTTP wrappers.

### Project structure

- `src/` - main application source
  - `components/` - application-wide, business agnostic components (no features)
    - `charts/` - chart components / wrappers
    - `custom/` - custom components
    - `hooks/` - custom hooks
    - `ui/` - unmodified [shadcn/ui](https://ui.shadcn.com/llms.txt) components
  - `features/` - feature / pod specific components etc.
    - `name/` or `group/name/` - name of the feature / scope
      - `components/` - feature specific components
      - `hooks` - feature specific hooks
  - `generated` - generated code
    - `api` - schemas / hooks for the main api
  - `routes` - tanstack router routes

Agents:

- Respect the above structure
- When adding new code, place it in the most appropriate existing folder.
- If unsure, prefer creating or extending a **feature module** under `src/features`.

---

## Setup & commands

Always use 'pnpm' instead of 'npm'. Use the following commands when working on this project:

- Install dependencies:
  - `pnpm install`
- Start dev server:
  - `pnpm dev`
- Build for production:
  - `pnpm build`
- Run tests:
  - `pnpm test`
- Run linting & typecheck:
  - `pnpm lint`
  - `pnpm typecheck`

Agents MUST ensure that any proposed changes are compatible with these commands.
Prefer changes that keep or improve build, lint, typecheck, and test health.

---

## TypeScript & coding style

- **TypeScript**
  - Assume `strict` mode is enabled.
  - Avoid `any` unless absolutely necessary and clearly justified with comments.
  - Prefer explicit return types on exported functions and components.
  - Use discriminated unions and enums for domain-specific states instead of magic strings.

- **React**
  - Use **function components** and **hooks** only.
  - Prefer composition over inheritance.
  - Ensure components remain as small and focused as practical.
  - Keep side effects in hooks like `useEffect`, not in render logic.
  - Use shadcn/ui primitives whenever possible
  - Don't use arbitrary colors, use the default shadcn/ui design tokens (defined in `src/index.css)

- **General conventions**
  - Follow existing ESLint/Prettier rules; do not introduce conflicting styles.
  - Naming:
    - Components: `PascalCase`
    - Hooks: `useCamelCase`
    - Files: follow existing convention (e.g., `kebab-case.tsx` or `index.tsx` in folders).

If you need to choose between multiple valid styles, copy the majority style already present in the repository.

---

## Routing (TanStack Router)

- Use TanStack Router for **all** client-side route definitions and navigation.
- Do **not** introduce other routing libraries.
- Prefer:
  - Route definitions colocated with route components (or following the existing pattern in this repo).
  - Strongly-typed route params and search params using the router's type helpers.
    - Use valibot (instead of zod) for validations
- Navigation:
  - Use the router's `<Link>` or route helpers, not `window.location` or hard-coded URLs.
- Data loading:
  - Use hooks from `src/generated/api` to load appropriate data

When adding a new page or route:

1. Define the route with TanStack Router in the appropriate route tree.
2. Ensure the route is typed correctly (params, search, loader data where applicable).
3. Use existing layout patterns (nested routes, layout components, etc.).

---

## Data fetching & state (TanStack Query)

- **All server communication must go through TanStack Query** using query/mutation hooks.
- Prefer:
  - `useQuery` for read operations.
  - `useMutation` for write operations.
  - Centralized query keys (e.g., in a `queryKeys` helper) to avoid duplication.
- **Do not**:
  - Use raw `fetch` or `axios` directly in components.
  - Store server-sourced data in global state libraries unless explicitly required.
- Caching & invalidation:
  - Use query key patterns like `['todo', id]` for entity-specific caches.
  - Invalidate or update relevant queries after mutations instead of forcing a full page reload.

When implementing a new feature that needs data:

1. Check if a contract/client already exists in `src/generated/api`.
2. If it does, create a wrapper hook (e.g., `useTodosQuery`) that uses:
   - The contract client for the request.
   - TanStack Query for caching and error handling.
3. If it does not, assume the contract will be generated; do NOT hand-code API client types.

---

## API contracts (generated in `src/generated/api`)

- Files in `src/generated/api` are generated. **Do not edit them manually.**
- When you need types or clients:
  - Prefer reusing existing contract exports.
  - If a type is missing, check for similar types before defining new ones.
- When consuming contracts:
  - Use contract types for function parameters and return values instead of ad-hoc interfaces.
  - Avoid duplicating shapes already defined in contract types.

If adding a new endpoint:

- Assume the contract will add:
  - Type-safe client functions
  - Request and response types

