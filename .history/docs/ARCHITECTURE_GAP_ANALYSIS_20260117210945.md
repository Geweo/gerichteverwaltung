# Architektur-Gap-Analyse – Ernährbär

Vergleich: **ARCHITECTURE.md** (Zentreo-Template) vs. **aktueller Stand** (Ernährbär)

---

## ✅ Bereits umgesetzt

| Komponente | Status | Anmerkung |
|------------|--------|-----------|
| **Hexagonale Architektur** | ✅ | Parts, Adapters.Infrastructure, Adapters.Api, Api |
| **MediatR** | ✅ | Commands/Handlers in Parts |
| **Ports & Adapters** | ✅ | IRecipeStorage, IBringExporter, ILLMService |
| **Docker: postgres, ollama, localstack** | ✅ | docker-compose.yml |
| **Supabase lokal** | ✅ | supabase/config.toml, supabase start |
| **LocalStack S3** | ✅ | Für PDF-Upload (nur Local) |

---

## ❌ Fehlt / Abweichungen

### 1. **Mediator-Paket: MediatR vs. Mediator.Abstractions**

| ARCHITECTURE.md | Aktuell | Gap |
|-----------------|---------|-----|
| `Mediator.Abstractions` 3.0.1<br>`Mediator.SourceGenerator` 3.0.1 | `MediatR` 12.4.1 | **Unterschiedliches Paket** |

**ARCHITECTURE.md beschreibt:**
- `Mediator.Abstractions` + Source Generator (compile-time)
- `ICommand<TResponse>`, `IQuery<TResponse>`
- `ICommandHandler<TCommand, TResponse>`, `IQueryHandler<TQuery, TResponse>`

**Aktuell:**
- `MediatR` (runtime-based)
- `IRequest<TResponse>`
- `IRequestHandler<TRequest, TResponse>`

**Entscheidung:** Sollen wir auf **Mediator** (Source Generator) umstellen oder bei **MediatR** bleiben?  
**MediatR** ist etabliert und funktioniert. **Mediator** ist performanter (Source Generator), aber weniger verbreitet.

---

### 2. **Wolverine für Background Tasks & Events**

| ARCHITECTURE.md | Aktuell | Gap |
|-----------------|---------|-----|
| `WolverineFx` 5.6.0<br>`WolverineFx.Postgresql` 5.6.0 | ❌ Nicht vorhanden | **Fehlt komplett** |

**ARCHITECTURE.md beschreibt:**
- Background Tasks (z.B. `FileExtractionTask`)
- Durable Message Queues (PostgreSQL-backed)
- Event-Handling
- Integration mit SignalR für Real-time Updates

**Aktuell:** Keine Background-Task-Infrastruktur.

**Relevanz für Ernährbär:**
- PDF-Upload → OCR-Verarbeitung (Background Task)
- Rezept-Generierung könnte asynchron laufen
- Event-basierte Updates (z.B. "Rezept generiert")

**Empfehlung:** Für PDF-Upload/OCR später relevant.

---

### 3. **CQRS-Lite: Queries fehlen**

| ARCHITECTURE.md | Aktuell | Gap |
|-----------------|---------|-----|
| `IQuery<TResponse>`, `IQueryHandler<TQuery, TResponse>` | ❌ Nur Commands | **Queries fehlen** |

**ARCHITECTURE.md beschreibt:**
- **Commands** (Write): `ICommand<TResponse>`, `ICommandHandler<...>`
- **Queries** (Read): `IQuery<TResponse>`, `IQueryHandler<...>`
- Separate Read- und Write-Repositories

**Aktuell:**
- ✅ Commands: `GenerateRecipesCommand`, `RegenerateRecipeCommand`, etc.
- ❌ **Keine Queries** (z.B. `GetRecipesQuery`, `GetMealPlanQuery`)

**Beispiel aus ARCHITECTURE.md:**
```csharp
public record GetPartByIdQuery(Guid PartId) : IQuery<PartReadModel>;
public class GetPartByIdQueryHandler : IQueryHandler<GetPartByIdQuery, PartReadModel>
```

**Für Ernährbär fehlen:**
- `GetRecipesQuery` (GET /api/recipes)
- `GetMealPlanQuery` (GET /api/meal-plans/{id})
- `GetRecipeByIdQuery`
- etc.

**Empfehlung:** Queries ergänzen, wenn Read-Endpoints implementiert werden.

---

### 4. **ReadModels & ReadRepositories**

| ARCHITECTURE.md | Aktuell | Gap |
|-----------------|---------|-----|
| Separate `ReadModels/`<br>Separate `ReadRepositories/` | ❌ Fehlen | **ReadModels fehlen** |

**ARCHITECTURE.md beschreibt:**
- **Entities** (Domain): für Write-Operations, enthalten Business-Logik
- **ReadModels** (Application): flach, optimiert für Read, keine Business-Logik
- **ReadRepositories**: optimiert für Read-Operations (z.B. direkte SQL, Views)

**Aktuell:**
- Entities in `Adapters.Infrastructure/Data/Entities/`
- **Keine** ReadModels
- **Keine** ReadRepositories (nur normale Repositories via IRecipeStorage)

**Beispiel aus ARCHITECTURE.md:**
```csharp
// Entity (Domain) - komplex, mit Navigation Properties
public class PartEntity { ... }

// ReadModel (Application) - flach, nur benötigte Daten
public record PartReadModel(Guid Id, string Name, List<TechnologyShortReadModel> Technologies);
```

**Für Ernährbär:**
- `RecipeReadModel` (flach, ohne Navigation Properties)
- `MealPlanReadModel`
- `RecipeListReadModel` (für Listen)

**Empfehlung:** ReadModels ergänzen, wenn Queries implementiert werden.

---

### 5. **FluentValidation für Commands**

| ARCHITECTURE.md | Aktuell | Gap |
|-----------------|---------|-----|
| `FluentValidation` 12.1.1 | ❌ Fehlt | **Validierung in Handlern** |

**ARCHITECTURE.md beschreibt:**
- FluentValidation für Command-Validierung
- Validatoren als separate Klassen

**Aktuell:**
- Validierung in Handlern (z.B. `if (request.NumberOfDays < 7) throw ...`)
- `PromptValidator` in Parts/Validation (statisch)

**Beispiel aus ARCHITECTURE.md:**
```csharp
public class CreatePartCommandValidator : AbstractValidator<CreatePartCommand>
{
    public CreatePartCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}
```

**Empfehlung:** FluentValidation ergänzen für saubere Command-Validierung.

---

### 6. **Separate Domain & Application Layers**

| ARCHITECTURE.md | Aktuell | Gap |
|-----------------|---------|-----|
| `Zentreo.Domain/`<br>`Zentreo.Application/` | ❌ Alles in `Parts` | **Keine Trennung** |

**ARCHITECTURE.md beschreibt:**
- **Domain**: Models, Values, Auth, Exceptions, Specifications (keine Dependencies)
- **Application**: Commands, Queries, ReadModels, ReadRepositories, Port, BackgroundTasks, Middleware

**Aktuell:**
- Alles in `Ernaehrbar.Parts`: Ports, Commands, Handlers, Models, Validation

**Struktur-Vergleich:**

**ARCHITECTURE.md:**
```
Zentreo.Domain/
  ├── Models/          # Domain Entities
  ├── Values/          # Value Objects
  ├── Auth/            # Auth Domain Logic
  ├── Exceptions/      # Domain Exceptions
  └── Specifications/  # Specification Pattern

Zentreo.Application/
  ├── Commands/
  ├── Queries/
  ├── ReadModels/
  ├── ReadRepositories/
  ├── Repositories/   # Write Repository Interfaces
  ├── Port/           # Port Interfaces
  ├── BackgroundTasks/
  └── Middleware/
```

**Aktuell:**
```
Ernaehrbar.Parts/
  ├── Ports/          # Port Interfaces
  ├── Commands/       # Commands
  ├── Handlers/       # Command Handlers
  ├── Models/         # z.B. MealPlanResult
  └── Validation/     # PromptValidator
```

**Empfehlung:** Für MVP ist die aktuelle Struktur ausreichend. Später optional trennen:
- `Ernaehrbar.Domain/` (Entities, Value Objects, Domain-Logik)
- `Ernaehrbar.Application/` (Commands, Queries, ReadModels, Ports)

---

### 7. **Docker-Container: HAProxy, Valkey, Prometheus, Grafana**

| ARCHITECTURE.md | Aktuell | Gap |
|-----------------|---------|-----|
| HAProxy (Reverse Proxy)<br>Valkey (Redis/Cache)<br>Prometheus, Grafana | ❌ Fehlen | **Optional** |

**ARCHITECTURE.md beschreibt:**
- **HAProxy**: Reverse Proxy, SSL Termination, Load Balancing
- **Valkey**: Redis-kompatibler Cache (Port 9060)
- **Prometheus/Grafana**: Metrics (optional)

**Aktuell:**
- Nur `postgres`, `ollama`, `localstack`

**Relevanz für Ernährbär:**
- **Valkey/Redis**: Caching für häufig abgerufene Rezepte, Tags, etc. (später sinnvoll)
- **HAProxy**: Für Production-Setup relevant, nicht für lokale Entwicklung
- **Prometheus/Grafana**: Monitoring (optional)

**Empfehlung:** Für lokale Entwicklung optional. Valkey später ergänzen, wenn Caching benötigt wird.

---

### 8. **ASP.NET Core 10 vs. 9**

| ARCHITECTURE.md | Aktuell | Gap |
|-----------------|---------|-----|
| ASP.NET Core **10** | ASP.NET Core **9** | **Version** |

**Aktuell:** .NET 9.0, ASP.NET Core 9.0  
**ARCHITECTURE.md:** ASP.NET Core 10

**Empfehlung:** Bei .NET 9 bleiben (aktuell stabil). Upgrade auf 10 später, wenn verfügbar.

---

## 📋 Zusammenfassung: Was fehlt

### **Hoch priorisiert (für CQRS-Lite)**

1. **Queries** (`IQuery<T>`, `IQueryHandler<...>`)
   - `GetRecipesQuery`, `GetMealPlanQuery`, etc.
   - Separate von Commands

2. **ReadModels**
   - `RecipeReadModel`, `MealPlanReadModel`
   - Flach, read-optimiert

3. **ReadRepositories**
   - `IRecipeReadRepository` (separat von `IRecipeStorage`)
   - Optimiert für Read-Operations

4. **FluentValidation**
   - Command-Validatoren
   - Saubere Trennung von Validierung und Business-Logik

### **Mittel priorisiert (für Background Tasks)**

5. **Wolverine**
   - Background Tasks (PDF-Upload → OCR)
   - Event-Handling
   - Durable Queues

### **Niedrig priorisiert (optional)**

6. **Domain/Application-Trennung**
   - Separater Domain Layer
   - Separater Application Layer

7. **Mediator vs. MediatR**
   - Umstellung auf `Mediator.Abstractions` + Source Generator (optional)

8. **Valkey/Redis**
   - Caching-Infrastruktur

---

## 🎯 Empfehlung: Nächste Schritte

1. **FluentValidation** hinzufügen (schnell, saubere Validierung)
2. **Queries** ergänzen (für Read-Endpoints wie GET /api/recipes)
3. **ReadModels** erstellen (wenn Queries implementiert werden)
4. **Wolverine** später (wenn PDF-Upload/OCR kommt)

**MediatR vs. Mediator:** Bei **MediatR** bleiben (etabliert, funktioniert). Umstellung auf Mediator optional.
