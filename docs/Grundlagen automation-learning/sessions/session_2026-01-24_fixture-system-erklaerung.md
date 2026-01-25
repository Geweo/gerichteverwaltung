# Session: Fixture-System Erklärung

**Datum:** 2026-01-24  
**Thema:** Vollständige Erklärung des Fixture-Systems in Ernährbär

---

## 🎯 Zweck von Fixtures

Fixtures sind **Testdaten**, die in die Datenbank geladen werden, um:
- **Lokale Entwicklung** zu erleichtern (konsistente Daten)
- **Integration-Tests** mit vorbereiteten Daten zu versorgen
- **Datenbank-Reset** zu automatisieren (via `reset-database.ps1`)

---

## 📐 Architektur-Übersicht

```
Ernaehrbar.Fixtures/
├── Utilities/
│   └── SeedableFixture.cs          # Basis-Klassen
├── Sets/
│   └── Development/
│       ├── DevelopmentFixtureSet.cs # Orchestrierung aller Fixtures
│       ├── GroupFixture.cs          # Einzelne Fixtures pro Tabelle
│       ├── UserFixture.cs
│       ├── RecipeFixture.cs
│       └── ...
├── Configuration/
│   └── ServiceCollectionExtensions.cs # DI-Registrierung
└── Program.cs                       # Entry Point für Standalone-Ausführung
```

---

## 🏗️ Komponenten im Detail

### 1. **SeedableFixture** (Basis-Klasse)

**Zweck:** Abstrakte Basis-Klasse für alle Fixtures

**Features:**
- **Idempotenz:** Verhindert mehrfaches Seeding (`_isSeeded` Flag)
- **Context-Verwaltung:** Stellt `ApplicationDbContext` bereit
- **Logging:** Protokolliert Seeding-Dauer

**Zwei Varianten:**

```csharp
// Ohne Parent-Abhängigkeit
public abstract class SeedableFixture
{
    protected ApplicationDbContext Context { get; private set; }
    protected abstract Task SeedAsync(CancellationToken cancellationToken);
}

// Mit Parent-Abhängigkeit (für DevelopmentFixtureSet)
public abstract class SeedableFixture<TParent> : SeedableFixture
{
    protected TParent Parent { get; private set; }
}
```

**Warum Parent?** Fixtures können auf andere Fixtures zugreifen (z.B. `RecipeFixture` braucht `GroupFixture`).

---

### 2. **DevelopmentFixtureSet** (Orchestrierung)

**Zweck:** Koordiniert alle Fixtures in der richtigen Reihenfolge

**Struktur:**
```csharp
public class DevelopmentFixtureSet : SeedableFixture
{
    // Alle Fixtures als Properties
    public GroupFixture GroupFixture { get; } = new();
    public UserFixture UserFixture { get; } = new();
    public RecipeFixture RecipeFixture { get; } = new();
    // ... 19 Fixtures insgesamt

    protected override async Task SeedAsync(CancellationToken cancellationToken)
    {
        // 1. Groups first (no dependencies)
        await GroupFixture.Seed(Context, this, cancellationToken);
        
        // 2. Users (no dependencies)
        await UserFixture.Seed(Context, this, cancellationToken);
        
        // 3. GroupMembers (depends on Groups and Users)
        await GroupMemberFixture.Seed(Context, this, cancellationToken);
        
        // ... weitere in Abhängigkeits-Reihenfolge
    }
}
```

**Wichtig:** Die Reihenfolge ist kritisch! Abhängigkeiten müssen zuerst geladen werden.

**Ablauf:**
1. `DevelopmentFixtureSet` wird erstellt
2. Alle Fixture-Instanzen werden als Properties erstellt
3. `SeedAsync` wird aufgerufen
4. Jedes Fixture wird in der richtigen Reihenfolge geseedet
5. Jedes Fixture erhält `Context` und `Parent` (DevelopmentFixtureSet)

---

### 3. **Einzelne Fixtures** (z.B. RecipeFixture)

**Zweck:** Erstellt Testdaten für eine spezifische Tabelle

**Beispiel:**
```csharp
public class RecipeFixture : SeedableFixture<DevelopmentFixtureSet>
{
    // Öffentliche Properties für Zugriff von anderen Fixtures/Tests
    public Recipe SpaghettiBolognese { get; private set; } = null!;
    public Recipe CaesarSalad { get; private set; } = null!;
    public Recipe Pancakes { get; private set; } = null!;

    protected override async Task SeedAsync(CancellationToken cancellationToken)
    {
        // Zugriff auf Parent (DevelopmentFixtureSet)
        var groups = Parent.GroupFixture;  // Zugriff auf andere Fixtures!
        var tags = Parent.TagFixture;

        // Erstelle Entities
        SpaghettiBolognese = new Recipe { ... };
        await Context.Recipes.AddAsync(SpaghettiBolognese, cancellationToken);
        
        // Speichere
        await Context.SaveChangesAsync(cancellationToken);

        // Nutze Helper-Methoden von anderen Fixtures
        var recipeIngredients = Parent.RecipeIngredientFixture;
        await recipeIngredients.AddIngredient(
            Context, 
            SpaghettiBolognese, 
            "Spaghetti", 
            400, 
            "g", 
            1, 
            cancellationToken
        );
    }
}
```

**Wichtige Patterns:**
- **Public Properties:** Erstellte Entities sind öffentlich, damit andere Fixtures/Tests darauf zugreifen können
- **Parent-Zugriff:** `Parent.GroupFixture` ermöglicht Zugriff auf bereits geseedete Fixtures
- **Helper-Methoden:** Manche Fixtures haben Helper-Methoden (z.B. `AddIngredient`), die von anderen Fixtures aufgerufen werden

---

### 4. **Helper-Fixtures** (z.B. RecipeIngredientFixture)

**Zweck:** Fixtures, die nur Helper-Methoden bereitstellen, aber selbst keine Daten in `SeedAsync` erstellen

**Beispiel:**
```csharp
public class RecipeIngredientFixture : SeedableFixture<DevelopmentFixtureSet>
{
    // Helper-Methode, die von RecipeFixture aufgerufen wird
    public async Task AddIngredient(
        ApplicationDbContext context, 
        Recipe recipe, 
        string name, 
        decimal? quantity, 
        string? unit, 
        int order, 
        CancellationToken cancellationToken)
    {
        var ingredient = new RecipeIngredient { ... };
        await context.RecipeIngredients.AddAsync(ingredient, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    protected override Task SeedAsync(CancellationToken cancellationToken)
    {
        // Leer - wird von RecipeFixture aufgerufen
        return Task.CompletedTask;
    }
}
```

**Warum?** Trennung von Verantwortlichkeiten: `RecipeFixture` erstellt Rezepte, `RecipeIngredientFixture` fügt Zutaten hinzu.

---

## 🔄 Ablauf: Wie werden Fixtures aufgerufen?

### Szenario 1: Standalone (Development)

**Entry Point:** `Ernaehrbar.Fixtures/Program.cs`

```csharp
// 1. DI-Container aufsetzen
var services = new ServiceCollection();
services.AddDbContext<ApplicationDbContext>(...);
services.AddErnaehrbarFixtures();  // Registriert DevelopmentFixtureSet als Singleton

// 2. Services bauen
var serviceProvider = services.BuildServiceProvider();

// 3. FixtureSet und DbContext holen
using var scope = serviceProvider.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
var fixtureSet = scope.ServiceProvider.GetRequiredService<DevelopmentFixtureSet>();

// 4. Seeding ausführen
await fixtureSet.Seed(dbContext, CancellationToken.None);
```

**Aufruf:** `dotnet run --project Ernaehrbar.Fixtures` oder via `reset-database.ps1`

---

### Szenario 2: Integration-Tests

**Entry Point:** `CustomWebApplicationFactory.RecreateDatabase()`

```csharp
public async Task RecreateDatabase()
{
    await using var scope = Services.CreateAsyncScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // 1. Datenbank löschen und neu erstellen
    await dbContext.Database.EnsureDeletedAsync();
    await dbContext.Database.EnsureCreatedAsync();

    // 2. Fixtures laden
    var fixtureSet = scope.ServiceProvider
        .GetRequiredService<DevelopmentFixtureSet>();
    await fixtureSet.Seed(dbContext, CancellationToken.None);
}
```

**Aufruf:** Automatisch in `BaseE2ETest.InitializeAsync()` vor jedem Test

**Wichtig:** Jeder Test bekommt eine **eigene Datenbank** (`ernaehrbar-test-{guid}`), die vor dem Test neu erstellt und mit Fixtures gefüllt wird.

---

### Szenario 3: Reset-Skript

**Entry Point:** `ops/scripts/reset-database.ps1`

```powershell
# 1. Datenbank löschen
dotnet ef database drop --force

# 2. Migrations anwenden
dotnet ef database update

# 3. Fixtures laden
cd Ernaehrbar.Fixtures
dotnet run  # Führt Program.cs aus
```

---

## 🔗 Abhängigkeiten und Reihenfolge

**Kritisch:** Fixtures müssen in der richtigen Reihenfolge geladen werden!

**Aktuelle Reihenfolge in DevelopmentFixtureSet:**

1. **Groups** (keine Abhängigkeiten)
2. **Users** (keine Abhängigkeiten)
3. **GroupMembers** (braucht Groups + Users)
4. **GroupInvites** (braucht Groups + Users)
5. **Tags** (braucht Groups)
6. **Recipes** (braucht Groups)
7. **RecipeIngredients** (braucht Recipes) - wird von RecipeFixture aufgerufen
8. **RecipeTags** (braucht Recipes + Tags) - wird von RecipeFixture aufgerufen
9. **NutritionInfos** (braucht Recipes) - wird von RecipeFixture aufgerufen
10. **RecipeRatings** (braucht Recipes + Users)
11. **MealPlans** (braucht Groups)
12. **MealPlanEntries** (braucht MealPlans + Recipes)
13. **RecipeDrafts** (braucht Groups + Users)
14. **RecipeDraftIngredients** (braucht RecipeDrafts)
15. **Notifications** (braucht Users)
16. **UploadTasks** (braucht Users + Groups + RecipeDrafts)
17. **Files** (braucht Groups + Users + Recipes + RecipeDrafts)
18. **ShoppingLists** (braucht Groups + Users)
19. **ShoppingListItems** (braucht ShoppingLists + RecipeIngredients)

**Visualisierung:**
```
Groups ──┐
         ├──> GroupMembers ──┐
Users ───┘                    │
                              ├──> Recipes ──> RecipeIngredients ──> ShoppingListItems
                              │
                              └──> MealPlans ──> MealPlanEntries
```

---

## 🎨 Design-Patterns

### 1. **Parent-Pattern**

Fixtures können auf andere Fixtures über `Parent` zugreifen:

```csharp
var groups = Parent.GroupFixture;  // Zugriff auf bereits geseedete Groups
var recipe = new Recipe { GroupId = groups.FamilieMueller.Id };
```

**Vorteil:** Keine zirkulären Abhängigkeiten, klare Hierarchie

---

### 2. **Public Properties Pattern**

Erstellte Entities werden als öffentliche Properties gespeichert:

```csharp
public Recipe SpaghettiBolognese { get; private set; } = null!;
```

**Vorteil:** Andere Fixtures/Tests können auf spezifische Entities zugreifen

---

### 3. **Helper-Methoden Pattern**

Manche Fixtures bieten Helper-Methoden an:

```csharp
await recipeIngredients.AddIngredient(Context, recipe, "Zutat", 100, "g", 1, ct);
```

**Vorteil:** Wiederverwendbarkeit, konsistente Erstellung

---

### 4. **Idempotenz-Pattern**

`SeedableFixture` verhindert mehrfaches Seeding:

```csharp
if (_isSeeded) {
    Log.Logger.Warning("Fixture {Name} has already been seeded, skipping");
    return;
}
```

**Vorteil:** Sicherheit bei versehentlichem mehrfachen Aufruf

---

## 🔧 Dependency Injection

**Registrierung:** `ServiceCollectionExtensions.AddErnaehrbarFixtures()`

```csharp
public static IServiceCollection AddErnaehrbarFixtures(this IServiceCollection services)
{
    services.AddSingleton<DevelopmentFixtureSet>();
    return services;
}
```

**Warum Singleton?** 
- `DevelopmentFixtureSet` hält alle Fixture-Instanzen
- Einmal erstellt, kann es mehrfach verwendet werden
- Fixtures selbst sind stateless (außer `_isSeeded` Flag)

---

## 📊 Fixture-Übersicht

**19 Tabellen = 19 Fixtures:**

| Tabelle | Fixture | Abhängigkeiten |
|---------|---------|----------------|
| Groups | `GroupFixture` | - |
| Users | `UserFixture` | - |
| GroupMembers | `GroupMemberFixture` | Groups, Users |
| GroupInvites | `GroupInviteFixture` | Groups, Users |
| Tags | `TagFixture` | Groups |
| Recipes | `RecipeFixture` | Groups |
| RecipeIngredients | `RecipeIngredientFixture` | Recipes (Helper) |
| RecipeTags | `RecipeTagFixture` | Recipes, Tags (Helper) |
| RecipeRatings | `RecipeRatingFixture` | Recipes, Users |
| NutritionInfos | `NutritionInfoFixture` | Recipes (Helper) |
| MealPlans | `MealPlanFixture` | Groups |
| MealPlanEntries | `MealPlanEntryFixture` | MealPlans, Recipes (Helper) |
| RecipeDrafts | `RecipeDraftFixture` | Groups, Users |
| RecipeDraftIngredients | `RecipeDraftIngredientFixture` | RecipeDrafts (Helper) |
| Notifications | `NotificationFixture` | Users |
| UploadTasks | `UploadTaskFixture` | Users, Groups, RecipeDrafts |
| Files | `FileFixture` | Groups, Users, Recipes, RecipeDrafts |
| ShoppingLists | `ShoppingListFixture` | Groups, Users |
| ShoppingListItems | `ShoppingListItemFixture` | ShoppingLists, RecipeIngredients (Helper) |

---

## 🧪 Verwendung in Tests

**Beispiel:**
```csharp
public class RecipeDraftsControllerTests : BaseE2ETest
{
    [Fact]
    public async Task GetDrafts_ReturnsDrafts()
    {
        // Fixtures sind bereits geladen (in InitializeAsync)
        // Zugriff über Factory
        var fixtureSet = Factory.Services
            .GetRequiredService<DevelopmentFixtureSet>();
        
        var draft = fixtureSet.RecipeDraftFixture.PendingDraft;
        // Test mit fixtureSet.UserFixture.MaxMueller, etc.
    }
}
```

**Wichtig:** Tests verwenden die **gleichen Fixtures** wie Development, aber in einer **separaten Test-Datenbank**.

---

## 🚀 Best Practices

1. **Reihenfolge beachten:** Abhängigkeiten müssen zuerst geladen werden
2. **Idempotenz:** Fixtures sollten mehrfach ausführbar sein (z.B. Prüfung auf Existenz)
3. **Public Properties:** Wichtige Entities als Properties exportieren
4. **Helper-Methoden:** Für wiederverwendbare Logik
5. **Context-Parameter:** Helper-Methoden sollten `ApplicationDbContext` explizit erhalten
6. **UTC DateTime:** PostgreSQL benötigt UTC, nicht Local

---

## 🔗 Verknüpfungen

- [[session_2026-01-24_database-reset-fixtures]] - Erstellung des Fixture-Systems
- [[session_2026-01-24_build-errors-fixes]] - Fixes für Fixture-Probleme
- [[rule_2026-01-24_top-level-statements-pattern-fuer-tests]] - Pattern für Tests
- [[solution_2026-01-24_entity-strukturen-vor-implementierung-pruefen]] - Entity-Struktur prüfen

---

## 📝 Zusammenfassung

**Fixture-System = Orchestrierte Testdaten-Erstellung**

1. **DevelopmentFixtureSet** orchestriert alle Fixtures
2. **SeedableFixture** Basis-Klasse mit Idempotenz und Logging
3. **Einzelne Fixtures** erstellen Daten für eine Tabelle
4. **Parent-Pattern** ermöglicht Zugriff auf andere Fixtures
5. **Helper-Methoden** für wiederverwendbare Logik
6. **Verwendung:** Development (Standalone), Tests (automatisch), Reset-Skript

**Kernprinzip:** Konsistente, wiederverwendbare Testdaten für alle Umgebungen.
