# Ernährbär

Rezept- & Zutatenplaner mit Bring-Anbindung.

## Projektstruktur

```
Ernährbär/
├── client/          # React Frontend
├── server/          # C# Backend (Hexagonale Architektur)
└── docker-compose.yml  # PostgreSQL Database
```

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

### 1. Datenbank starten
```bash
docker-compose up -d
```

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

- [Frontend README](client/README.md)
- [Backend README](server/README.md)
