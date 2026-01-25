# Zentreo Adaption - Architektonische Verbesserungen

## Übersicht

Dieses Dokument dokumentiert die Adaption von Architektur-Patterns und Best Practices aus dem zentreo-Projekt in das Ernährbär-Projekt. Die Adaption erfolgte in einer Session und umfasst sowohl Frontend- als auch Backend-Verbesserungen.

## 1. Route-Struktur (TanStack Router)

### Problem
- Flache Route-Struktur ohne klare Hierarchie
- Keine Trennung zwischen authentifizierten und anonymen Routen
- Inkonsistente Struktur erschwert Wartung und Skalierung

### Lösung aus zentreo
- **Verschachtelte Folder-Struktur**: Jede Route hat einen eigenen Ordner mit `route.tsx` (Layout) und `index.tsx` (Page)
- **Parent Routes**: `_app` für authentifizierte, `_anon` für anonyme Routen
- **Klare Hierarchie**: `/routes/_app/recipes/index.tsx` statt `/routes/recipes.tsx`

### Implementierung
```
routes/
  _app/
    route.tsx              # Layout für authentifizierte User
    index.tsx              # Redirect zu /dashboard
    dashboard/
      index.tsx            # Dashboard Page
    recipes/
      index.tsx            # Recipes Page
  _anon/
    route.tsx              # Layout für anonyme User
    login/
      index.tsx            # Login Page
    register/
      index.tsx            # Register Page
```

### Vorteile
- **Bessere Organisation**: Klare Struktur, leicht zu navigieren
- **Wiederverwendbarkeit**: Layout-Logik in `route.tsx` zentralisiert
- **Skalierbarkeit**: Einfach neue Routen hinzufügen
- **Type-Safety**: TanStack Router nutzt die Struktur für Type-Inference

---

## 2. Server-Side Filtering & Pagination

### Problem
- Client-seitige Filterung bei großen Datenmengen ineffizient
- Alle Daten werden geladen, auch wenn nur ein Teil benötigt wird
- Keine echte Pagination, nur client-seitiges Slicing
- URL-Parameter nicht synchronisiert mit Filter-State

### Lösung aus zentreo
- **Backend-Filtering**: Alle Filter werden als Query-Parameter an das Backend gesendet
- **Server-Side Pagination**: Backend berechnet `totalCount` und `totalPages`
- **URL-basierte State-Verwaltung**: Filter, Pagination und Sortierung in URL-Parametern
- **PaginatedResult<T>**: Einheitliche Struktur für paginierte Responses

### Backend-Implementierung

#### PaginatedResult
```csharp
public record PaginatedResult<TPayload>(
    int Page,
    int PageSize,
    int TotalCount,
    List<TPayload> Items)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
```

#### Query mit Filtern
```csharp
public record GetRecipesQuery(
    int GroupId,
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    MealCategory? MealCategory = null,
    RecipeSource? Source = null,
    bool? Favorites = null,
    List<int>? TagIds = null,
    RecipeListSorting SortBy = RecipeListSorting.Name,
    SortDirectionEnum SortDirection = SortDirectionEnum.Asc
) : IRequest<PaginatedResult<RecipeReadModel>>;
```

#### Repository-Implementierung
- Filter werden in EF Core Query angewendet
- `CountAsync()` wird **vor** Pagination ausgeführt (für totalCount)
- `Skip()` und `Take()` für Pagination
- `AsNoTracking()` für bessere Performance

### Frontend-Implementierung

#### URL-Parameter Schema (valibot)
```typescript
const searchSchema = v.object({
  search: v.optional(v.fallback(v.string(), ''), ''),
  mealCategory: v.optional(v.picklist(['Breakfast', 'Lunch', 'Dinner'])),
  source: v.optional(v.picklist(['Manual', 'Generated', 'Upload'])),
  favorites: v.optional(v.fallback(v.boolean(), false), false),
  tagIds: v.optional(v.fallback(v.array(v.number()), []), [] as number[]),
  page: v.optional(v.fallback(v.string(), '1'), '1'),
  pageSize: v.optional(v.fallback(v.string(), '10'), '10'),
  sortBy: v.optional(v.string()),
  sortDirection: v.optional(v.picklist(['asc', 'desc'])),
});
```

#### useRecipes Hook
```typescript
export function useRecipes(
  groupId: number,
  filters?: {
    page?: number;
    pageSize?: number;
    mealCategory?: 'Breakfast' | 'Lunch' | 'Dinner';
    // ... weitere Filter
  }
) {
  return useQuery({
    queryKey: ['recipes', groupId, filters],
    queryFn: async (): Promise<PaginatedResult<Recipe>> => {
      const response = await customInstance<PaginatedResult<Recipe>>({
        url: '/api/Recipes',
        method: 'GET',
        params: { groupId, ...filters },
      });
      return response;
    },
  });
}
```

### Vorteile
- **Performance**: Nur benötigte Daten werden geladen
- **Skalierbarkeit**: Funktioniert auch bei Millionen von Datensätzen
- **URL-Synchronisation**: Filter können geteilt werden (Bookmarks, Links)
- **Backend-Kontrolle**: Komplexe Filter-Logik im Backend, nicht im Frontend
- **Bessere UX**: Schnellere Ladezeiten, weniger Memory-Verbrauch

---

## 3. Table Query Hooks (TanStack Table Integration)

### Problem
- Manuelle Verwaltung von Pagination- und Sort-State
- Inkonsistente Konvertierung zwischen URL-Parametern und Table-State
- Wiederholter Code für jede Tabelle

### Lösung aus zentreo
- **useTableStateFromUrl**: Konvertiert zwischen URL-Parametern (1-based) und TanStack Table State (0-based)
- **useServerTableQueryParams**: Konvertiert Table-State zu Backend-Query-Parametern
- **useServerTableInstance**: Setzt React Table Instance für Server-Side Tables auf
- **useServerTableWithQuery**: Convenience Hook für einfache Listen
- **useServerTableWithQueryById**: Convenience Hook für ID-basierte Listen

### Wichtige Hooks

#### useTableStateFromUrl
```typescript
// Konvertiert URL-Parameter (1-based page) zu TanStack Table State (0-based pageIndex)
const { pagination, sorting, handlePaginationChange, handleSortingChange } = 
  useTableStateFromUrl({
    search,
    onPaginationChange: (page, pageSize) => updateTableFilters({ page, pageSize }),
    onSortingChange: (sortBy, sortDirection) => updateTableFilters({ sortBy, sortDirection }),
  });
```

#### useSearchParams
```typescript
// Generischer Hook für URL-Parameter-Management
const { updateTableFilters } = useSearchParams({ search, navigate });

// Aktualisiert Filter in URL
updateTableFilters({ 
  mealCategory: 'Breakfast',
  page: '1',  // Reset to first page when filter changes
});
```

### Vorteile
- **DRY**: Keine Wiederholung von Konvertierungs-Logik
- **Type-Safety**: TypeScript unterstützt alle Konvertierungen
- **Konsistenz**: Alle Tabellen verhalten sich gleich
- **Wartbarkeit**: Änderungen an einem Ort wirken sich auf alle Tabellen aus

---

## 4. Exception-Handling (Backend)

### Problem
- Generische Exception-Handling ohne spezifische HTTP-Status-Codes
- Schwer zu unterscheiden zwischen verschiedenen Fehlertypen
- Inkonsistente Error-Responses

### Lösung aus zentreo
- **Spezifische Exception-Typen**: Basis-Exception + spezialisierte Exceptions
- **Pattern-Matching in Middleware**: Switch-Expression für Exception-Typen
- **Korrekte HTTP-Status-Codes**: 401, 404, 400, 500

### Implementierung

#### Exception-Typen
```csharp
// Basis-Exception
public class ErnaehrbarException : Exception { }

// Spezialisierte Exceptions
public class ErnaehrbarUnauthorizedException : ErnaehrbarException { }
public class ErnaehrbarNotFoundException : ErnaehrbarException { }
public class ErnaehrbarValidationException : ErnaehrbarException { }
```

#### Middleware
```csharp
private async Task WriteErrorResponseAsync(HttpContext context, Exception ex)
{
    var (statusCode, message) = ex switch
    {
        ErnaehrbarUnauthorizedException => (HttpStatusCode.Unauthorized, ex.Message ?? "Unauthorized"),
        ErnaehrbarNotFoundException => (HttpStatusCode.NotFound, ex.Message ?? "Not Found"),
        ErnaehrbarValidationException => (HttpStatusCode.BadRequest, ex.Message ?? "Validation Error"),
        ValidationException validationException => (HttpStatusCode.BadRequest, GetValidationErrorMessage(validationException)),
        _ => (HttpStatusCode.InternalServerError, GetGenericErrorMessage(ex))
    };

    context.Response.StatusCode = (int)statusCode;
    await context.Response.WriteAsync(message);
}
```

### Vorteile
- **Klarheit**: Frontend kann spezifisch auf Fehlertypen reagieren
- **Korrekte HTTP-Codes**: RESTful API-Verhalten
- **Wartbarkeit**: Einfach neue Exception-Typen hinzufügen
- **Debugging**: Leichter zu identifizieren, welche Art von Fehler aufgetreten ist

---

## 5. UI Components & Layout System

### Problem
- Inkonsistente Page-Layouts
- Wiederholter Code für Header, Content, Actions
- Keine einheitlichen Empty States

### Lösung aus zentreo
- **Page Layout Components**: Wiederverwendbare Layout-Komponenten
- **Empty States**: Einheitliche Darstellung für leere Listen
- **Loading States**: Konsistente Loading-Indikatoren

### Implementierte Components

#### Page Layout
```typescript
// page-header.tsx, page-content.tsx, page-section.tsx
// page-title.tsx, page-action.tsx, page-description.tsx

<PageHeader>
  <PageTitle>Rezept-Datenbank</PageTitle>
  <PageDescription>Zentrale Verwaltung aller Gerichte</PageDescription>
  <PageAction>
    <Button>Neues Rezept</Button>
  </PageAction>
</PageHeader>
```

#### Empty States
```typescript
// list-empty-state.tsx, empty.tsx (shadcn/ui)

<ListEmptyState
  title="Keine Rezepte gefunden"
  description="Erstelle dein erstes Rezept!"
/>
```

### Vorteile
- **Konsistenz**: Alle Pages sehen gleich aus
- **Wartbarkeit**: Änderungen am Layout wirken sich auf alle Pages aus
- **Entwicklungsgeschwindigkeit**: Schneller neue Pages erstellen
- **UX**: Einheitliche User Experience

---

## 6. Utility Hooks

### Problem
- Wiederholter Code für häufige Aufgaben (Datum-Formatierung, Mobile-Detection, etc.)
- Inkonsistente Implementierungen

### Lösung aus zentreo
- **use-date-formatter**: Einheitliche Datumsformatierung mit date-fns
- **use-mobile**: Responsive Breakpoint-Detection
- **use-decimal-input**: Dezimal-Eingabe mit Komma/Punkt-Unterstützung

### Implementierte Hooks

#### use-date-formatter
```typescript
const { formatDate, formatTimeAgo, formatShortDate } = useDateFormatter();

formatDate(new Date()); // "25. Januar 2026"
formatTimeAgo(date);    // "vor 2 Stunden"
formatShortDate(date);   // "25.01.2026"
```

#### use-mobile
```typescript
const isMobile = useIsMobile(); // true wenn < 768px
```

#### use-decimal-input
```typescript
const { inputValue, handleChange, handleBlur } = useDecimalInput({
  value: 12.5,
  onChange: (value) => setValue(value),
});
// Unterstützt sowohl "12,5" als auch "12.5"
```

### Vorteile
- **DRY**: Keine Wiederholung von Code
- **Konsistenz**: Einheitliche Formatierung überall
- **Wartbarkeit**: Änderungen an einem Ort
- **i18n-Ready**: Vorbereitet für Internationalisierung

---

## 7. API-Endpunkt-Struktur

### Problem
- Inkonsistente Query-Parameter-Namen
- Keine einheitliche Pagination
- Unterschiedliche Response-Formate

### Lösung aus zentreo
- **Einheitliche Query-Parameter**: `page`, `pageSize`, `sortBy`, `sortDirection`
- **PaginatedResult**: Einheitliche Response-Struktur
- **Konsistente Filter-Parameter**: Alle Filter als Query-Parameter

### API-Endpunkt Beispiel
```
GET /api/Recipes?groupId=1&page=1&pageSize=10&mealCategory=Breakfast&source=Manual&sortBy=Name&sortDirection=Asc

Response:
{
  "page": 1,
  "pageSize": 10,
  "totalCount": 42,
  "totalPages": 5,
  "items": [...]
}
```

### Vorteile
- **Vorhersagbarkeit**: Frontend weiß immer, was zu erwarten ist
- **Wiederverwendbarkeit**: Gleiche Patterns für alle Endpunkte
- **Dokumentation**: Einfacher zu dokumentieren
- **Testing**: Einfacher zu testen

---

## 8. Sortierung & Enums

### Problem
- String-basierte Sortierung ohne Type-Safety
- Inkonsistente Sort-Richtungen

### Lösung aus zentreo
- **SortDirectionEnum**: `Asc`, `Desc`
- **RecipeListSorting**: Enum für alle sortierbaren Felder
- **Type-Safe**: Compile-Time-Überprüfung

### Implementierung
```csharp
public enum SortDirectionEnum
{
    Asc,
    Desc,
}

public enum RecipeListSorting
{
    Name,
    CreatedAt,
    UpdatedAt,
    MealCategory,
    Source,
    AverageRating,
}
```

### Vorteile
- **Type-Safety**: Compiler fängt Fehler ab
- **IntelliSense**: IDE unterstützt Autocomplete
- **Refactoring-Sicherheit**: Umbenennungen werden überall aktualisiert
- **Dokumentation**: Enum-Werte dokumentieren verfügbare Optionen

---

## Architektonische Prinzipien

### 1. Separation of Concerns
- **Frontend**: UI-Logik, State-Management, URL-Synchronisation
- **Backend**: Business-Logik, Datenbank-Queries, Filterung

### 2. Single Source of Truth
- **URL als State**: Filter, Pagination, Sortierung in URL
- **Backend als Datenquelle**: Eine Quelle für alle Daten

### 3. Type Safety
- **TypeScript**: Frontend-Type-Safety
- **C# Enums**: Backend-Type-Safety
- **Valibot**: Runtime-Validierung von URL-Parametern

### 4. DRY (Don't Repeat Yourself)
- **Wiederverwendbare Hooks**: useTableStateFromUrl, useSearchParams
- **Layout Components**: Page-Header, Page-Content, etc.
- **Exception-Handling**: Zentrale Middleware

### 5. Performance
- **Server-Side Filtering**: Nur benötigte Daten laden
- **AsNoTracking()**: EF Core ohne Change-Tracking
- **Pagination**: Begrenzte Datenmengen

### 6. Skalierbarkeit
- **Modulare Struktur**: Einfach neue Features hinzufügen
- **Konsistente Patterns**: Gleiche Patterns überall
- **Wartbarkeit**: Änderungen an einem Ort

---

## Wichtige Dateien

### Frontend
- `routes/_app/route.tsx` - Authentifiziertes Layout
- `routes/_app/recipes/index.tsx` - Recipes Route mit valibot Schema
- `features/recipes/hooks/useRecipes.ts` - PaginatedResult Hook
- `components/hooks/use-table-state-from-url.ts` - URL ↔ Table State
- `components/hooks/use-search-params.ts` - URL-Parameter Management
- `components/custom/page-*.tsx` - Layout Components
- `components/hooks/use-date-formatter.ts` - Datumsformatierung
- `components/hooks/use-mobile.ts` - Mobile Detection

### Backend
- `Ernaehrbar.Parts/Queries/Common/PaginatedResult.cs` - Pagination-Struktur
- `Ernaehrbar.Parts/Domain/SortDirectionEnum.cs` - Sort-Richtung
- `Ernaehrbar.Parts/Domain/RecipeListSorting.cs` - Sort-Felder
- `Ernaehrbar.Parts/Queries/GetRecipesQuery.cs` - Query mit allen Filtern
- `Ernaehrbar.Adapters.Api/Controllers/RecipesController.cs` - API-Endpunkt
- `Ernaehrbar.Adapters.Infrastructure/ReadRepositories/RecipeReadRepository.cs` - Repository
- `Ernaehrbar.Parts/Exceptions/*.cs` - Exception-Typen
- `Ernaehrbar.Adapters.Api/Middleware/ExceptionHandlingMiddleware.cs` - Error-Handling

---

## Lessons Learned

### Warum ist zentreo so gut gelöst?

1. **Konsistenz**: Alle Patterns werden konsistent angewendet
2. **Type-Safety**: Überall Type-Safety, sowohl Frontend als auch Backend
3. **Performance**: Server-Side Filtering und Pagination von Anfang an
4. **Wartbarkeit**: Klare Struktur, wiederverwendbare Komponenten
5. **Skalierbarkeit**: Patterns, die auch bei großen Datenmengen funktionieren
6. **Developer Experience**: Hooks und Components machen Entwicklung schneller

### Probleme, die gelöst werden

1. **Performance bei großen Datenmengen**: Server-Side Filtering
2. **URL-Synchronisation**: Filter können geteilt werden
3. **Code-Duplikation**: Wiederverwendbare Hooks und Components
4. **Inkonsistente UX**: Einheitliche Layouts und Components
5. **Schwer zu testen**: Klare Struktur, Type-Safety
6. **Schwer zu warten**: Konsistente Patterns, DRY-Prinzip

---

## Nächste Schritte

### Empfohlene weitere Adaptionen

1. **use-server-table-with-query**: Für einfachere Table-Integration
2. **use-server-table-with-query-by-id**: Für ID-basierte Queries
3. **Weitere UI Components**: table-toolbar, table-sort-dropdown, etc.
4. **Error-Boundaries**: Frontend Error-Handling
5. **Loading States**: Verbesserte Loading-Indikatoren
6. **Optimistic Updates**: Für bessere UX bei Mutations

---

## 9. Datenbank-Struktur & Entity-Konfiguration

### Problem
- Alle Entity-Konfigurationen im DbContext (OnModelCreating)
- Manuelle Timestamp-Verwaltung
- Keine Trennung zwischen Domain-Models und Entities
- Inkonsistente Konfiguration

### Lösung aus zentreo
- **Entity-Maps (IEntityTypeConfiguration)**: Separate Konfigurationsklassen pro Entity
- **Automatisches Laden**: `ApplyConfigurationsFromAssembly` lädt alle Maps
- **Automatische Timestamps**: In `SaveChanges` automatisch gesetzt
- **Interface-basierte Base-Entities**: `IBaseEntity` für Flexibilität
- **DateTimeOffset**: Statt DateTime für bessere Timezone-Unterstützung

### Zentreo-Ansatz

#### Entity-Maps
```csharp
// Separate Map-Klasse pro Entity
public class PartsEntityMap : BaseMapGuid<PartEntity>
{
    public override void Configure(EntityTypeBuilder<PartEntity> entity)
    {
        base.Configure(entity);
        entity.ToTable("parts");
        entity.HasIndex(x => x.TenantId);
        // ... weitere Konfiguration
    }
}
```

#### Automatisches Laden
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    // Lädt alle IEntityTypeConfiguration automatisch
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
}
```

#### Automatische Timestamps
```csharp
private void AddTimestamps()
{
    var transactionTime = DateTimeOffset.UtcNow;
    
    ChangeTracker.Entries()
        .Where(x => _managedStates.Contains(x.State))
        .Where(x => x.Entity is IBaseEntity)
        .ToList()
        .ForEach(x =>
        {
            if (x.Entity is not IBaseEntity entity) return;
            entity.UpdatedAt = transactionTime;
            if (x.State == EntityState.Added && entity.CreatedAt == default)
            {
                entity.CreatedAt = transactionTime;
            }
        });
}

public override Task<int> SaveChangesAsync(...)
{
    AddTimestamps();
    return base.SaveChangesAsync(...);
}
```

#### Interface-basierte Base-Entities
```csharp
public interface IBaseEntity
{
    DateTimeOffset CreatedAt { get; set; }
    DateTimeOffset UpdatedAt { get; set; }
}

public interface IBaseEntity<T> : IBaseEntity
{
    T Id { get; set; }
}

// Verschiedene Base-Entities für verschiedene ID-Typen
public class BaseEntityGuid : IBaseEntity<Guid> { }
public class BaseEntityInt : IBaseEntity<int> { }
```

### Ernährbär-Ansatz (aktuell)

#### Manuelle Konfiguration im DbContext
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Alle Konfigurationen manuell im DbContext
    ConfigureRecipeEntities(modelBuilder);
    ConfigureMealPlanEntities(modelBuilder);
    // ...
}

private static void ConfigureRecipeEntities(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Recipe>()
        .HasOne(r => r.Group)
        .WithMany(g => g.Recipes)
        .HasForeignKey(r => r.GroupId);
    // ...
}
```

#### Abstrakte Base-Entities
```csharp
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public abstract class BaseGroupEntity : BaseEntity
{
    public int GroupId { get; set; }
    public Group Group { get; set; } = null!;
}
```

### Vergleich

| Aspekt | Ernährbär (aktuell) | Zentreo |
|--------|---------------------|---------|
| **Entity-Konfiguration** | Im DbContext | Separate Map-Klassen |
| **Timestamp-Typ** | `DateTime` | `DateTimeOffset` |
| **Timestamp-Management** | Default-Werte in DB | Automatisch in SaveChanges |
| **Base-Entity** | Abstrakte Klassen | Interfaces |
| **ID-Typen** | Nur `int` | `Guid`, `int` (flexibel) |
| **Wartbarkeit** | Alles in einer Datei | Eine Map pro Entity |

### Vorteile von zentreo-Ansatz

1. **Bessere Organisation**: Jede Entity hat ihre eigene Map-Klasse
2. **Wartbarkeit**: Änderungen an einer Entity betreffen nur ihre Map
3. **Skalierbarkeit**: Einfach neue Entities hinzufügen
4. **Automatisierung**: Timestamps werden automatisch gesetzt
5. **Flexibilität**: Verschiedene ID-Typen möglich
6. **Timezone-Support**: DateTimeOffset für bessere Timezone-Handling

### Empfohlene Adaptionen

1. **Entity-Maps einführen**: Separate Map-Klassen für bessere Organisation
2. **Automatisches Laden**: `ApplyConfigurationsFromAssembly` verwenden
3. **Automatische Timestamps**: In `SaveChanges` setzen statt DB-Defaults
4. **DateTimeOffset**: Für bessere Timezone-Unterstützung
5. **Interface-basierte Base-Entities**: Für mehr Flexibilität

### Index-Strategie

#### Zentreo
- Indizes werden in Entity-Maps definiert
- Composite Indizes für häufige Query-Patterns
- Explizite Index-Namen

#### Ernährbär (aktuell)
- Indizes in `OnModelCreating` definiert
- Composite Indizes für Performance (z.B. `MealPlanId, Date, MealCategory`)
- Gute Index-Strategie bereits vorhanden

### Multi-Tenancy

#### Zentreo
- `TenantId` als Guid
- Separate `TenantEntity`
- `UserTenantEntity` für User-Tenant-Beziehungen

#### Ernährbär
- `GroupId` als int
- `Group` Entity mit `GroupMember` für User-Group-Beziehungen
- Ähnliches Pattern, aber andere Namensgebung

### Vorteile
- **Bessere Organisation**: Maps statt alles im DbContext
- **Wartbarkeit**: Änderungen isoliert pro Entity
- **Automatisierung**: Weniger manueller Code
- **Flexibilität**: Verschiedene ID-Typen, Interfaces
- **Timezone-Support**: DateTimeOffset

---

## 10. Enum-Konsolidierung (Domain vs. Infrastructure)

### Problem
Nach der Implementierung der Entity-Maps traten Namenskonflikte auf:
- `RecipeSource` existierte sowohl in `Ernaehrbar.Parts.Domain` als auch in `Ernaehrbar.Adapters.Infrastructure.Data.Entities`
- `DraftStatus` existierte sowohl in `Ernaehrbar.Parts.Domain` als auch in `Ernaehrbar.Adapters.Infrastructure.Data.Entities`
- `File` Entity hatte Konflikt mit `System.IO.File`

### Lösung
**Konsolidierung auf Domain-Enums:**
- ✅ Entfernt: `Entities.RecipeSource` und `Entities.DraftStatus`
- ✅ Entities (`Recipe`, `RecipeDraft`) verwenden jetzt direkt `Domain.RecipeSource` und `Domain.DraftStatus`
- ✅ Entfernt: Alle Mapping-Funktionen (`MapRecipeSourceToInfrastructure`, etc.) aus Repositories
- ✅ Type-Alias für `File` Entity: `using FileEntity = Ernaehrbar.Adapters.Infrastructure.Data.Entities.File;`

**Vorteile:**
- **Single Source of Truth**: Enums nur im Domain-Layer
- **Weniger Boilerplate**: Keine Mapping-Funktionen mehr nötig
- **Klarere Architektur**: Domain-Enums werden direkt von Entities verwendet
- **Einfachere Wartung**: Änderungen nur an einer Stelle

**Geänderte Dateien:**
- `Recipe.cs`, `RecipeDraft.cs`: Verwenden jetzt `Domain.RecipeSource` und `Domain.DraftStatus`
- `RecipeRepository.cs`, `RecipeDraftRepository.cs`: Mapping-Funktionen entfernt
- `RecipeReadRepository.cs`: Type-Alias für `RecipeSource` hinzugefügt
- `FileEntityMap.cs`: Type-Alias für `File` Entity
- Fixtures und Tests: Aktualisiert auf Domain-Enums

---

## Zusammenfassung

Die Adaption von zentreo-Patterns hat das Ernährbär-Projekt in folgenden Bereichen verbessert:

- ✅ **Route-Struktur**: Klare Hierarchie, bessere Organisation
- ✅ **Server-Side Filtering**: Performance, Skalierbarkeit
- ✅ **URL-State-Management**: Teilbare Filter, bessere UX
- ✅ **Exception-Handling**: Klare Fehlerbehandlung
- ✅ **UI Components**: Konsistente Layouts
- ✅ **Utility Hooks**: Wiederverwendbare Logik
- ✅ **Type Safety**: Überall Type-Safety
- ✅ **Wartbarkeit**: DRY, konsistente Patterns
- ✅ **Datenbank-Struktur**: Entity-Maps und automatische Timestamps implementiert
- ✅ **Enum-Konsolidierung**: Domain-Enums als Single Source of Truth

Diese Architektur bildet eine solide Basis für zukünftige Features und Skalierung.
