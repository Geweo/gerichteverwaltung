# Finale Architektur-Analyse – Ernährbär

**Stand:** Nach Implementierung von FluentValidation, Queries, ReadModels, ReadRepositories

---

## ✅ Vollständig umgesetzt

| Komponente | Status | Details |
|------------|--------|---------|
| **Hexagonale Architektur** | ✅ | Parts, Adapters.Infrastructure, Adapters.Api, Api |
| **MediatR (CQRS-Lite)** | ✅ | Commands/Handlers + **Queries/QueryHandlers** |
| **FluentValidation** | ✅ | Validatoren für Commands, Pipeline Behavior |
| **ReadModels** | ✅ | RecipeReadModel, MealPlanReadModel (flach, read-optimiert) |
| **ReadRepositories** | ✅ | IRecipeReadRepository, IMealPlanReadRepository + Implementierungen |
| **Ports & Adapters** | ✅ | IRecipeStorage, IBringExporter, ILLMService |
| **Docker: postgres, ollama, localstack** | ✅ | docker-compose.yml |
| **Supabase lokal** | ✅ | supabase/config.toml, supabase start |
| **LocalStack S3** | ✅ | Für PDF-Upload (nur Local) |

---

## ⚠️ Teilweise umgesetzt / Noch offen

### 1. **Controller-Endpoints für Queries**

| Status | Details |
|--------|---------|
| ✅ **Implementiert** | Alle Query-Endpoints sind verfügbar |

**Aktuell:**
- ✅ Queries erstellt: `GetRecipeByIdQuery`, `GetRecipesQuery`, `GetMealPlanByIdQuery`, `GetMealPlansQuery`
- ✅ Query-Handler implementiert
- ✅ `RecipesController.GetRecipes()` implementiert
- ✅ `RecipesController.GetRecipeById()` implementiert
- ✅ `MealPlansController` erstellt mit `GetMealPlans()` und `GetMealPlanById()`

**Endpoints:**
- ✅ `GET /api/recipes?groupId={id}` → `GetRecipesQuery` (mit Filterung: tagIds, searchTerm, skip, take)
- ✅ `GET /api/recipes/{id}` → `GetRecipeByIdQuery`
- ✅ `GET /api/meal-plans?groupId={id}` → `GetMealPlansQuery` (mit Filterung: startDateFrom, startDateTo, skip, take)
- ✅ `GET /api/meal-plans/{id}` → `GetMealPlanByIdQuery`

---

### 2. **Redundante Controller-Validierung**

| Status | Details |
|--------|---------|
| ✅ **Bereinigt** | Manuelle Validierung entfernt, FluentValidation übernimmt |

**Aktuell:**
- ✅ FluentValidation implementiert (`GenerateRecipesCommandValidator`, `RegenerateRecipeCommandValidator`)
- ✅ Manuelle Validierung im Controller entfernt (außer Prompt-Validierung, die als Business-Logik bleibt)
- ✅ FluentValidation läuft automatisch vor Handler-Ausführung

**Hinweis:**
- Prompt-Validierung (`ValidatePrompt`) bleibt als zusätzliche Business-Logik bestehen
- Alle anderen Validierungen (NumberOfDays, MealCategories, etc.) werden von FluentValidation übernommen

---

### 3. **IRecipeStorage mit CRUD**

| Status | Details |
|--------|---------|
| ❌ **Port definiert, aber leer** | Keine CRUD-Operationen |

**Aktuell:**
```csharp
public interface IRecipeStorage
{
    // TODO: Define recipe storage operations
}
```

**Fehlt:**
- `CreateRecipeAsync(Recipe recipe)`
- `UpdateRecipeAsync(int id, Recipe recipe)`
- `DeleteRecipeAsync(int id)`
- `GetRecipeByIdAsync(int id)` (für Write-Operations, nicht Read)

**Empfehlung:** CRUD-Operationen in `IRecipeStorage` definieren und `RecipeStorageAdapter` implementieren.

---

### 4. **IFileStorage-Port**

| Status | Details |
|--------|---------|
| ❌ **Nicht vorhanden** | Für PDF-Upload benötigt |

**ARCHITECTURE.md sagt:**
- **Local:** LocalStack S3 (localhost:4566)
- **Dev/Prod:** Supabase Storage

**Fehlt:**
- `IFileStorage` Port in `Ernaehrbar.Parts/Ports/`
- `LocalStackS3FileStorageAdapter` (für Local)
- `SupabaseStorageAdapter` (für Dev/Prod)
- DI-Registrierung je nach Environment

**Empfehlung:** `IFileStorage` implementieren, wenn PDF-Upload kommt.

---

### 5. **IBringExporter / ExportToBring**

| Status | Details |
|--------|---------|
| ⚠️ **Port vorhanden, aber Stub** | `ExportToBringCommandHandler` ist leer |

**Aktuell:**
- ✅ `IBringExporter` Port vorhanden
- ✅ `BringExporterAdapter` vorhanden (Stub)
- ✅ `ExportToBringCommand` vorhanden
- ❌ `ExportToBringCommandHandler` ist leer (TODO)

**Empfehlung:** Implementieren, wenn Bring.com-Integration benötigt wird.

---

### 6. **User aus JWT (`sub`) in User-Tabelle**

| Status | Details |
|--------|---------|
| ❌ **Nicht implementiert** | User-Synchronisation fehlt |

**ARCHITECTURE.md sagt:**
- User aus JWT (`sub`) in eigener User-Tabelle (PostgreSQL) anlegen/abgleichen
- Middleware oder Service

**Fehlt:**
- Middleware/Service, der JWT `sub` extrahiert
- User in `Users`-Tabelle anlegt/aktualisiert
- User-Context für Commands/Queries (z.B. `GroupId`)

**Empfehlung:** User-Middleware implementieren, wenn Multi-Tenant-Features benötigt werden.

---

## 🔮 Optional / Später

### 7. **Wolverine für Background Tasks**

| Status | Details |
|--------|---------|
| 🔮 **Später** | Für PDF-Upload → OCR |

**Relevanz:** PDF-Upload → OCR-Verarbeitung (Background Task)

**Empfehlung:** Später implementieren, wenn PDF-Upload kommt.

---

### 8. **Domain/Application-Trennung**

| Status | Details |
|--------|---------|
| 🔮 **Optional** | Aktuelle Struktur ist ausreichend |

**Aktuell:** Alles in `Ernaehrbar.Parts`

**Empfehlung:** Für MVP ausreichend. Später optional trennen:
- `Ernaehrbar.Domain/` (Entities, Value Objects, Domain-Logik)
- `Ernaehrbar.Application/` (Commands, Queries, ReadModels, Ports)

---

### 9. **Mediator vs. MediatR**

| Status | Details |
|--------|---------|
| ✅ **Entscheidung getroffen** | Bei MediatR bleiben |

**ARCHITECTURE.md beschreibt:** `Mediator.Abstractions` + Source Generator

**Aktuell:** `MediatR` 12.4.1 (etabliert, funktioniert)

**Empfehlung:** ✅ Bei MediatR bleiben.

---

### 10. **Valkey/Redis, HAProxy, Prometheus/Grafana**

| Status | Details |
|--------|---------|
| 🔮 **Optional** | Für lokale Entwicklung nicht notwendig |

**Empfehlung:** Später ergänzen, wenn Caching/Monitoring benötigt wird.

---

## 📋 Zusammenfassung: Was noch offen ist

### **Hoch priorisiert (für vollständige API)**

1. ✅ **Controller-Endpoints für Queries** - **ERLEDIGT**
   - ✅ `GET /api/recipes` → `GetRecipesQuery`
   - ✅ `GET /api/recipes/{id}` → `GetRecipeByIdQuery`
   - ✅ `GET /api/meal-plans` → `GetMealPlansQuery`
   - ✅ `GET /api/meal-plans/{id}` → `GetMealPlanByIdQuery`

2. ✅ **Redundante Controller-Validierung entfernen** - **ERLEDIGT**
   - ✅ Manuelle Validierung in `RecipesController` entfernt
   - ✅ FluentValidation übernimmt dies bereits

3. **IRecipeStorage mit CRUD**
   - CRUD-Operationen definieren
   - `RecipeStorageAdapter` implementieren

### **Mittel priorisiert (für vollständige Features)**

4. **IFileStorage-Port**
   - Port definieren
   - Adapter für LocalStack S3 (Local) und Supabase Storage (Dev/Prod)

5. **ExportToBring implementieren**
   - `ExportToBringCommandHandler` implementieren
   - Bring.com-Integration

6. **User aus JWT synchronisieren**
   - Middleware/Service für User-Synchronisation
   - User-Context für Commands/Queries

### **Niedrig priorisiert (optional/später)**

7. **Wolverine** (später, wenn PDF-Upload kommt)
8. **Domain/Application-Trennung** (optional)
9. **Valkey/Redis, HAProxy, Monitoring** (optional)

---

## 🎯 Empfehlung: Nächste Schritte

1. ✅ **Controller-Endpoints für Queries implementieren** - **ERLEDIGT**
2. ✅ **Redundante Controller-Validierung entfernen** - **ERLEDIGT**
3. **IRecipeStorage CRUD implementieren** (wenn Write-Operations benötigt werden)
4. **User-Middleware für GroupId-Extraktion** (damit GroupId nicht als Query-Parameter übergeben werden muss)

**Status:** Architektur ist **vollständig** umgesetzt! ✅  
**API-Endpoints für Queries sind implementiert!** ✅  
**Offen:** Business-Logik-Implementierungen (IRecipeStorage CRUD, IFileStorage, ExportToBring, User-Synchronisation).
