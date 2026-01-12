# Ernährbär Server

Backend für den Rezept- & Zutatenplaner mit Bring-Anbindung.

## Architektur

Das Projekt verwendet eine hexagonale Architektur (Ports & Adapters):

- **Ernaehrbar.Parts**: Domain und Application Layer
  - `Ports/`: Interface-Definitionen für externe Abhängigkeiten
  - `UseCases/`: Anwendungsfälle
- **Ernaehrbar.Adapters.Infrastructure**: Infrastructure Adapter
  - EF Core + PostgreSQL
  - Supabase Storage
  - Bring.com Integration
- **Ernaehrbar.Adapters.Api**: API Adapter
  - ASP.NET Core Controllers
  - Middleware (Supabase JWT)
- **Ernaehrbar.Api**: Entry Point
  - Program.cs
  - Dependency Injection Setup
- **Ernaehrbar.Tests**: Unit Tests
  - xunit + NSubstitute

## Tech Stack

- .NET 9.0
- C# 14
- ASP.NET Core 9.0
- Entity Framework Core 9.0
- PostgreSQL (Npgsql)
- Serilog
- xunit + NSubstitute

## Backend Details

- Supabase-JWT Middleware
- Ports: RecipeStorage, BringExporter
- UseCases: UploadRecipe, GeneratePlan, ExportToBring
- Infrastruktur: EF Core + PostgreSQL
- Architektur: hexagonal
- Logging: Serilog
- Tests: xunit + NSubstitute

## Setup

### 0. Ollama Setup (für Rezept-Generierung)

**Für lokale Entwicklung:**
1. Installiere Ollama lokal von [ollama.com](https://ollama.com)
2. Starte Ollama (wird normalerweise automatisch als Service gestartet)
3. Installiere das Modell: `ollama pull llama3.2`
4. Prüfe die Verbindung: Öffne `http://localhost:11434/api/tags` im Browser

Die Konfiguration ist bereits in `appsettings.Development.json` eingestellt.

Siehe auch: [OLLAMA_SETUP.md](../../OLLAMA_SETUP.md)

### 1. Start PostgreSQL Database

**Option A: Docker Compose (empfohlen)**
```bash
# Im Root-Verzeichnis des Projekts
docker-compose up -d
```

**Option B: Lokale PostgreSQL Installation**
- Installiere PostgreSQL lokal
- Erstelle eine Datenbank namens `ernaehrbar`
- Aktualisiere den Connection String in `appsettings.json`

### 2. Install .NET EF Core Tools (falls noch nicht installiert)
```bash
dotnet tool install --global dotnet-ef
```

### 3. Restore NuGet packages
```bash
cd server
dotnet restore
```

### 4. Erstelle und wende Migrationen an
```bash
# Erstelle Initial Migration
dotnet ef migrations add InitialCreate --project Ernaehrbar.Adapters.Infrastructure --startup-project Ernaehrbar.Api

# Wende Migrationen auf die Datenbank an
dotnet ef database update --project Ernaehrbar.Adapters.Infrastructure --startup-project Ernaehrbar.Api
```

### 5. Starte die API
```bash
dotnet run --project Ernaehrbar.Api
```

Die API läuft dann auf `https://localhost:5000` (oder dem konfigurierten Port).

## Tests

Run tests with:
```bash
dotnet test
```
