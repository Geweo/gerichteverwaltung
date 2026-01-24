# Ernährbär

Rezept- & Zutatenplaner mit Bring-Anbindung.

## Projektstruktur

```
Ernährbär/
├── client/            # React Frontend
├── server/            # C# Backend (Hexagonale Architektur)
├── ops/               # Operations & Infrastructure
│   ├── local/         # Lokale Konfigurationen
│   │   ├── supabase/  # Lokales Supabase (supabase start), nur für Local
│   │   ├── postgres/  # Postgres Init Scripts
│   │   └── localstack/ # LocalStack S3 Config
│   ├── cloud/         # Cloud Infrastructure (Pulumi IaC - für später)
│   ├── pipeline/      # CI/CD Pipeline Configs
│   └── scripts/       # Utility Scripts (z.B. ensure-localstack-bucket)
├── tests/             # Tests
│   └── e2e/           # E2E Tests (Cypress - für später)
├── docs/
│   ├── ARCHITECTURE.md   # Schichten, Local vs Dev/Prod, Docker
│   └── LOCAL_SETUP.md    # Schritt-für-Schritt für lokale Entwicklung
└── docker-compose.yml   # Postgres, Ollama, LocalStack (nur Local)
```

**Local:** Supabase lokal (`supabase start`), LocalStack S3 für PDF-Upload, `appsettings.Local.json`.  
**Dev/Prod:** Supabase Cloud, Supabase Storage, **kein** LocalStack.  
→ [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md), [docs/LOCAL_SETUP.md](docs/LOCAL_SETUP.md)

## Tech Stack

### Frontend
- React 19
- TypeScript (strict mode)
- TanStack Router
- TanStack Query
- shadcn/ui
- Vite
- pnpm

### Backend
- .NET 9.0
- C# 14
- ASP.NET Core 9.0
- Entity Framework Core 9.0
- PostgreSQL
- Serilog
- xunit + NSubstitute

### Externe Services
- Supabase (Auth + Storage)
- OpenAI GPT (OCR & Rezeptvorschläge)
- Bring.com (Einkaufsliste)

## Quick Start

### 1. Lokale Dienste (Docker: Postgres, Ollama, LocalStack)
```bash
docker-compose up -d
```
- **postgres** (5432), **ollama** (11434), **localstack** (4566, S3 für PDF-Upload)  
Für **volles Local** (Supabase + LocalStack): [docs/LOCAL_SETUP.md](docs/LOCAL_SETUP.md).  
Für **Dev/Prod** mit Supabase Cloud: keine lokale Supabase, kein LocalStack.

### 2. Backend Setup
```bash
cd server
dotnet restore
dotnet tool install --global dotnet-ef  # Falls noch nicht installiert
dotnet ef migrations add InitialCreate --project Ernaehrbar.Adapters.Infrastructure --startup-project Ernaehrbar.Api
dotnet ef database update --project Ernaehrbar.Adapters.Infrastructure --startup-project Ernaehrbar.Api
dotnet run --project Ernaehrbar.Api
```

### 3. Frontend Setup
```bash
cd client
pnpm install
cp .env.example .env
# Bearbeite .env mit deinen Supabase Credentials
pnpm dev
```

## Features

- Rezept-Upload (PDF/Bild) mit OCR
- Automatische Zutatenextraktion
- Wochenplanung (Zufall oder Wunschprompt)
- Einkaufsliste aggregieren
- Export an Bring.com

## MVP-Ziele

- [ ] Upload & Parsing 1 PDF
- [ ] Zutatenstruktur speichern
- [ ] Wochenplan erzeugen
- [ ] 1x Bring-Sync erfolgreich durchführen

## Weitere Informationen

- [Architektur & Docker](docs/ARCHITECTURE.md) – Schichten, Local vs Dev/Prod, Docker
- [Lokales Setup](docs/LOCAL_SETUP.md) – Supabase + LocalStack für Local
- [Frontend README](client/README.md)
- [Backend README](server/README.md)
- [Entwicklungs-Roadmap](ROADMAP.md) – Priorisierte Implementierungsaufgaben
- [Projekt-Spezifikationen](PROJECT_SPECIFICATIONS.md) – Detaillierte Anforderungen für Dashboard & Gerichteverwaltung
- [Projektziele](project-goals.md) – MVP-Ziele und Architekturüberblick