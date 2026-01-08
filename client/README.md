# Ernährbär Client

Frontend für den Rezept- & Zutatenplaner mit Bring-Anbindung.

## Tech Stack

- React 19
- TypeScript (strict mode)
- TanStack Router
- TanStack Query
- shadcn/ui
- Vite
- pnpm
- Orval (API Client Generation)

## Setup

1. Install dependencies:
```bash
pnpm install
```

2. Copy `.env.example` to `.env` and fill in your credentials:
   - `VITE_SUPABASE_URL` - Your Supabase project URL
   - `VITE_SUPABASE_ANON_KEY` - Your Supabase anon key
   - `VITE_API_URL` - Backend API URL (default: http://localhost:5000)

3. Generate API clients from backend:
```bash
# Make sure the backend is running first
pnpm generate:api
```

4. Start dev server:
```bash
pnpm dev
```

## API Client Generation

Die TypeScript API Clients werden automatisch aus der OpenAPI/Swagger Spec des Backends generiert:

1. Backend muss laufen und Swagger unter `/swagger/v1/swagger.json` bereitstellen
2. `pnpm generate:api` ausführen
3. Generierte Clients befinden sich in `src/generated/api/`

Die generierten Clients:
- Verwenden TanStack Query Hooks (`useQuery`, `useMutation`)
- Sind vollständig typisiert
- Integrieren automatisch Supabase JWT Authentication
- Werden automatisch mit Prettier formatiert

## Project Structure

- `src/components/` - Application-wide, business agnostic components
  - `ui/` - shadcn/ui components
  - `custom/` - Custom components
  - `hooks/` - Custom hooks
  - `charts/` - Chart components
- `src/features/` - Feature-specific components and logic
- `src/generated/api/` - Generated API types and clients (do not edit manually)
- `src/routes/` - TanStack Router route definitions
- `src/lib/` - Utility functions and configurations
  - `api-client.ts` - Custom fetch instance with Supabase auth

## Frontend Details

- Auth via Supabase Auth
- Routen: Upload, Rezeptübersicht, Wochenplan, Einkaufsliste
- Jede Route lädt Daten über TanStack Query
- Kein globaler Zustand außer Auth
- Styling: shadcn/ui
- State & Query logic in `src/features/*/hooks`
