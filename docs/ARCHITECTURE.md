# Architektur & Infrastruktur – Ernährbär

## 1. Hexagonale Architektur (Backend)

Die Anwendung folgt **Ports & Adapters (Hexagonal Architecture)**. Abhängigkeiten zeigen immer nach innen (zum Kern).

```
                    ┌─────────────────────────────────────────┐
                    │              Ernaehrbar.Api              │
                    │  (Entry Point: Program.cs, DI, Wiring)   │
                    └─────────────────────┬───────────────────┘
                                          │
                                          ▼
                    ┌─────────────────────────────────────────┐
                    │         Ernaehrbar.Adapters.Api          │
                    │  Controller, Middleware, Models,         │
                    │  ServiceCollectionExtensions             │
                    └───────────┬─────────────────┬───────────┘
                                │                 │
                ┌───────────────┘                 └───────────────┐
                ▼                                                 ▼
┌───────────────────────────────┐               ┌───────────────────────────────┐
│ Ernaehrbar.Adapters.Infrastructure │           │        Ernaehrbar.Parts        │
│  - DbContext, Entities, Migrations │           │  Ports, Commands/Handlers      │
│  - RecipeStorageAdapter            │           │  (keine Abhängigkeit zu        │
│  - BringExporterAdapter            │           │   Adapters)                    │
│  - OllamaAdapter (ILLMService)     │           └───────────────────────────────┘
└───────────────────┬───────────────┘
                    │
                    └──────────────────────────► Parts (Port-Implementierungen)
```

### Schichten – Zuordnung zu Ordnern

| Schicht | Projekt | Enthält |
|---------|---------|---------|
| **Parts** | `server/Ernaehrbar.Parts` | `Ports/`, `Commands/`, `Handlers/` (MediatR), `Models/`, `Validation/` |
| **Adapters.Infrastructure** | `server/Ernaehrbar.Adapters.Infrastructure` | `Data/` (DbContext, Entities, Migrations), `Storage/`, `Bring/`, `LLM/`, `ServiceCollectionExtensions` |
| **Adapters.Api** | `server/Ernaehrbar.Adapters.Api` | `Controllers/`, `Middleware/`, `Models/`, `ServiceCollectionExtensions` |
| **Api** | `server/Ernaehrbar.Api` | `Program.cs`, `appsettings.*` |

### Abhängigkeiten (Dependency Direction)

- **Parts** ← Adapters.Infrastructure  
- **Parts** ← Adapters.Api  
- **Adapters.Api** ← Api  
- **Parts** darf nicht von Adapters abhängen.

---

## 2. Umgebungen: Local vs. Dev vs. Prod

| Komponente | **Local** (nur auf deinem Rechner) | **Dev / Prod** (deployt) |
|------------|-----------------------------------|---------------------------|
| **Supabase** | Lokal: `supabase start` (Auth, ggf. Storage) | **Supabase Cloud** (Auth, Storage) |
| **PDF/Datei-Upload** | **LocalStack S3** (Docker, Port 4566) | **Supabase Storage** |
| **PostgreSQL (App)** | Docker `postgres` (5432) | Gehostete DB (z.B. Supabase DB, RDS, …) |
| **Ollama (LLM)** | Docker `ollama` (11434) oder lokal | Externer LLM (OpenAI, Anthropic, …) oder weggelassen |

- **Local:** `appsettings.Local.json` + Profil `Local` (ASPNETCORE_ENVIRONMENT=Local). Supabase-URLs zeigen auf `http://127.0.0.1:54321`, Storage auf LocalStack `http://localhost:4566`.
- **Dev/Prod:** `appsettings.json` (bzw. Staging/Production-Overrides). Supabase Cloud, **kein** LocalStack.

---

## 3. Docker (nur für Local)

### In `docker-compose.yml`

| Service | Image | Port | Zweck |
|---------|-------|------|-------|
| **postgres** | `postgres:16-alpine` | 5432 | App-DB `ernaehrbar` |
| **ollama** | `ollama/ollama` | 11434 | LLM (Rezepte, Tags) |
| **localstack** | `localstack/localstack` | 4566 | **S3 für PDF-Upload** (Docker-Container; nur Local; Dev/Prod: Supabase Storage) |

Alle genannten Services laufen als **Docker-Container** in `docker-compose`. Starten:

```bash
docker-compose up -d
```

### Supabase lokal (nur Local)

Supabase **nicht** in `docker-compose`; eigener Stack via [Supabase CLI](https://supabase.com/docs/guides/cli):

```bash
cd ops/local/supabase
pnpm install
pnpm run start
```

Oder verwende das Helper-Script:
```bash
./supabase-restart.sh
```

Nutzt `ops/local/supabase/config.toml`; startet Auth, Studio, Storage, PostgREST, etc. in Docker. Die App-DB (`ernaehrbar`, 5432) bleibt getrennt; Supabase nutzt eigene Postgres (z.B. 54322).

**Dev/Prod:** Supabase **Cloud** – keine lokale Supabase-Instanz.

---

## 4. Kurzüberblick: Was läuft wo?

| Komponente | Local | Dev/Prod |
|------------|-------|----------|
| PostgreSQL (App) | Docker `postgres` | Gehostete DB |
| Ollama | Docker `ollama` | Extern/kein |
| Supabase Auth | `supabase start` (127.0.0.1:54321) | Supabase Cloud |
| PDF/Datei (IFileStorage) | **LocalStack S3** (localhost:4566) | **Supabase Storage** |
| Bring.com | Extern (Stub) | Extern |

---

## 5. Nächste Schritte (Architektur & Infrastruktur)

- [ ] **User aus JWT (`sub`)** in eigener User-Tabelle (PostgreSQL) anlegen/abgleichen (Middleware oder Service).
- [ ] **IRecipeStorage** mit CRUD definieren und **RecipeStorageAdapter** implementieren.
- [ ] **IFileStorage**-Port: Adapter für **LocalStack S3** (Local) und **Supabase Storage** (Dev/Prod).
- [ ] **IBringExporter** / **ExportToBring** implementieren.
