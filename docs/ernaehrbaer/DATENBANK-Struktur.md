# Datenbankstruktur – Ernährbär

**Stand:** 2026-01-24  
**Zweck:** Vollständige Übersicht über die Datenbankstruktur

---

## 📊 Tabellenübersicht

### Multi-Tenant & User-Management

#### `Groups`
Multi-Tenant-Gruppen (z.B. "Familie Müller", "WG Berlin")

| Spalte | Typ | Beschreibung |
|--------|-----|--------------|
| `Id` | int (PK) | Primärschlüssel |
| `Name` | string (required) | Gruppenname |
| `Description` | string? | Optionale Beschreibung |
| `CreatedAt` | DateTime | Erstellungsdatum |
| `UpdatedAt` | DateTime | Letzte Änderung |

**Navigation:**
- `Members` → `GroupMember[]`
- `Recipes` → `Recipe[]`
- `MealPlans` → `MealPlan[]`
- `Invites` → `GroupInvite[]`

---

#### `Users`
User (Mapping zu Supabase)

| Spalte | Typ | Beschreibung |
|--------|-----|--------------|
| `Id` | int (PK) | Primärschlüssel |
| `SupabaseUserId` | string (required, unique) | Supabase User ID (sub aus JWT) |
| `Email` | string (required, indexed) | E-Mail-Adresse |
| `DisplayName` | string? | Anzeigename |
| `CreatedAt` | DateTime | Erstellungsdatum |
| `UpdatedAt` | DateTime | Letzte Änderung |

**Constraints:**
- Unique Index auf `SupabaseUserId`
- Index auf `Email`

**Navigation:**
- `GroupMemberships` → `GroupMember[]`

---

#### `GroupMembers`
User-Gruppen-Zugehörigkeit

| Spalte | Typ | Beschreibung |
|--------|-----|--------------|
| `Id` | int (PK) | Primärschlüssel |
| `UserId` | int (FK) | Foreign Key zu `Users` |
| `GroupId` | int (FK) | Foreign Key zu `Groups` |
| `Role` | GroupRole | Rolle (Member, Admin) |
| `JoinedAt` | DateTime | Beitrittsdatum |
| `CreatedAt` | DateTime | Erstellungsdatum |
| `UpdatedAt` | DateTime | Letzte Änderung |

**Constraints:**
- Unique Index auf `(UserId, GroupId)`
- Cascade Delete bei User oder Group

**Enum:**
```csharp
public enum GroupRole
{
    Member = 1,
    Admin = 2
}
```

---

#### `GroupInvites`
Gruppen-Einladungen

| Spalte | Typ | Beschreibung |
|--------|-----|--------------|
| `Id` | int (PK) | Primärschlüssel |
| `GroupId` | int (FK) | Foreign Key zu `Groups` |
| `Token` | string (required, unique) | Einladungs-Token |
| `InvitedEmail` | string? | E-Mail des Eingeladenen |
| `CreatedByUserId` | int (FK) | User, der Einladung erstellt hat |
| `ExpiresAt` | DateTime | Ablaufdatum |
| `IsUsed` | bool | Wurde Einladung verwendet? |
| `UsedAt` | DateTime? | Verwendungsdatum |
| `CreatedAt` | DateTime | Erstellungsdatum |
| `UpdatedAt` | DateTime | Letzte Änderung |

**Constraints:**
- Unique Index auf `Token`
- Cascade Delete bei Group
- Restrict Delete bei CreatedByUser

---

### Rezepte

#### `Recipes`
Rezepte (gehören zu einer Group)

| Spalte | Typ | Beschreibung |
|--------|-----|--------------|
| `Id` | int (PK) | Primärschlüssel |
| `GroupId` | int (FK) | Foreign Key zu `Groups` |
| `Name` | string (required) | Rezeptname |
| `Description` | string? | Beschreibung |
| `Instructions` | string? | Zubereitung |
| `ImageUrl` | string? | Bild-URL (Supabase Storage) |
| `PdfUrl` | string? | PDF-URL (falls Upload) |
| `Servings` | int? | Anzahl Portionen |
| `PreparationTimeMinutes` | int? | Vorbereitungszeit (Minuten) |
| `CookingTimeMinutes` | int? | Kochzeit (Minuten) |
| `CreatedAt` | DateTime | Erstellungsdatum |
| `UpdatedAt` | DateTime | Letzte Änderung |

**⚠️ Fehlt (siehe Analyse):**
- `Source` (RecipeSource: Generated, Upload, Manual) - **Empfohlen: Migration 1**
- `MealCategory` (MealCategory: Breakfast, Lunch, Dinner, nullable) - **Empfohlen: Migration 1**
- `RepeatCycleWeeks` (int?, nullable) - **Empfohlen: Migration 1**

**Hinweis:** Siehe `DATENBANK-Erweiterungen.md` für Implementierungsdetails.

**Navigation:**
- `Group` → `Group`
- `Ingredients` → `RecipeIngredient[]`
- `RecipeTags` → `RecipeTag[]`
- `Ratings` → `RecipeRating[]`
- `NutritionInfo` → `NutritionInfo?` (1:1)
- `MealPlanEntries` → `MealPlanEntry[]`

---

#### `RecipeIngredients`
Zutaten eines Rezepts

| Spalte | Typ | Beschreibung |
|--------|-----|--------------|
| `Id` | int (PK) | Primärschlüssel |
| `RecipeId` | int (FK) | Foreign Key zu `Recipes` |
| `Name` | string (required) | Zutatenname |
| `Quantity` | decimal? | Menge |
| `Unit` | string? | Einheit (z.B. "g", "ml", "Stück") |
| `Notes` | string? | Notizen |
| `Order` | int | Reihenfolge |
| `CreatedAt` | DateTime | Erstellungsdatum |
| `UpdatedAt` | DateTime | Letzte Änderung |

**Constraints:**
- Cascade Delete bei Recipe

---

#### `Tags`
Tags (kategorisiert, gehören zu einer Group)

| Spalte | Typ | Beschreibung |
|--------|-----|--------------|
| `Id` | int (PK) | Primärschlüssel |
| `GroupId` | int (FK) | Foreign Key zu `Groups` |
| `Name` | string (required) | Tag-Name |
| `Category` | TagCategory | Kategorie |
| `CreatedAt` | DateTime | Erstellungsdatum |
| `UpdatedAt` | DateTime | Letzte Änderung |

**Constraints:**
- Unique Index auf `(Name, GroupId)`
- Cascade Delete bei Group

**Enum:**
```csharp
public enum TagCategory
{
    Preparation = 1,  // z.B. "schnell", "aufwendig"
    Diet = 2,         // z.B. "vegetarisch", "vegan"
    Ingredient = 3    // z.B. "Kidneybohnen", "Fisch"
}
```

**Navigation:**
- `RecipeTags` → `RecipeTag[]`

---

#### `RecipeTags`
Many-to-Many: Rezepte ↔ Tags

| Spalte | Typ | Beschreibung |
|--------|-----|--------------|
| `Id` | int (PK) | Primärschlüssel |
| `RecipeId` | int (FK) | Foreign Key zu `Recipes` |
| `TagId` | int (FK) | Foreign Key zu `Tags` |
| `CreatedAt` | DateTime | Erstellungsdatum |
| `UpdatedAt` | DateTime | Letzte Änderung |

**Constraints:**
- Unique Index auf `(RecipeId, TagId)`
- Cascade Delete bei Recipe oder Tag

---

#### `RecipeRatings`
Bewertungen & Favoriten (pro User)

| Spalte | Typ | Beschreibung |
|--------|-----|--------------|
| `Id` | int (PK) | Primärschlüssel |
| `RecipeId` | int (FK) | Foreign Key zu `Recipes` |
| `UserId` | int (FK) | Foreign Key zu `Users` |
| `Rating` | int? | Bewertung (1-5, nullable) |
| `IsFavorite` | bool | Ist Favorit? |
| `Comment` | string? | Kommentar/Review |
| `CreatedAt` | DateTime | Erstellungsdatum |
| `UpdatedAt` | DateTime | Letzte Änderung |

**Constraints:**
- Unique Index auf `(UserId, RecipeId)`
- Cascade Delete bei Recipe oder User

**⚠️ Offen:**
- Aggregierte Bewertung auf Recipe-Ebene (ReadModel oder denormalisiert)
- **Empfehlung:** `RecipeReadModel.AverageRating` (berechnet aus RecipeRatings)
- **Empfehlung:** `RecipeReadModel.FavoriteCount` (Anzahl Favoriten)

---

#### `NutritionInfos`
Nährwerte (1:1 mit Recipe)

| Spalte | Typ | Beschreibung |
|--------|-----|--------------|
| `Id` | int (PK) | Primärschlüssel |
| `RecipeId` | int (FK, unique) | Foreign Key zu `Recipes` |
| `Calories` | decimal? | Kalorien pro Portion |
| `Protein` | decimal? | Protein (g) pro Portion |
| `Carbohydrates` | decimal? | Kohlenhydrate (g) pro Portion |
| `Fat` | decimal? | Fett (g) pro Portion |
| `Fiber` | decimal? | Ballaststoffe (g) pro Portion |
| `Sugar` | decimal? | Zucker (g) pro Portion |
| `Sodium` | decimal? | Natrium (mg) pro Portion |
| `CreatedAt` | DateTime | Erstellungsdatum |
| `UpdatedAt` | DateTime | Letzte Änderung |

**Constraints:**
- 1:1 Beziehung zu Recipe
- Cascade Delete bei Recipe

**⚠️ Offen:**
- "Aggregiert" in Komponenten-Dokumentation unklar (pro Serving vs. gesamtes Rezept)
- **Klarstellung:** Nährwerte sind **pro Portion** (pro Serving), nicht für das gesamte Rezept
- Multipliziert mit `Recipe.Servings` für Gesamt-Nährwerte

---

### Wochenpläne

#### `MealPlans`
Wochenpläne (gehören zu einer Group)

| Spalte | Typ | Beschreibung |
|--------|-----|--------------|
| `Id` | int (PK) | Primärschlüssel |
| `GroupId` | int (FK) | Foreign Key zu `Groups` |
| `StartDate` | DateTime (required) | Startdatum (typisch: Montag) |
| `EndDate` | DateTime (required) | Enddatum (typisch: Sonntag) |
| `Name` | string? | Name/Beschreibung |
| `GenerationPrompt` | string? | Prompt für KI-Generierung |
| `CreatedAt` | DateTime | Erstellungsdatum |
| `UpdatedAt` | DateTime | Letzte Änderung |

**⚠️ Fehlt (siehe Analyse):**
- `Status` (MealPlanStatus: Draft, Active, Archived) - **Empfohlen: Migration 2**

**Hinweis:** Siehe `DATENBANK-Erweiterungen.md` für Implementierungsdetails.

**Navigation:**
- `Group` → `Group`
- `Entries` → `MealPlanEntry[]`

---

#### `MealPlanEntries`
Mahlzeiten in einem Wochenplan

| Spalte | Typ | Beschreibung |
|--------|-----|--------------|
| `Id` | int (PK) | Primärschlüssel |
| `MealPlanId` | int (FK) | Foreign Key zu `MealPlans` |
| `Date` | DateTime (required) | Datum der Mahlzeit |
| `MealCategory` | MealCategory | Mahlzeit (Breakfast, Lunch, Dinner) |
| `RecipeId` | int? (FK) | Foreign Key zu `Recipes` (nullable) |
| `CustomMealName` | string? | Custom Name (falls kein Recipe) |
| `DayNumber` | int | Tag-Nummer (1-7 für Wochenpläne) |
| `CreatedAt` | DateTime | Erstellungsdatum |
| `UpdatedAt` | DateTime | Letzte Änderung |

**Constraints:**
- Cascade Delete bei MealPlan
- SetNull Delete bei Recipe (RecipeId wird null, Entry bleibt)
- Index auf `(MealPlanId, Date, MealCategory)`

**Enum:**
```csharp
public enum MealCategory
{
    Breakfast = 1,
    Lunch = 2,
    Dinner = 3
}
```

**Navigation:**
- `MealPlan` → `MealPlan`
- `Recipe` → `Recipe?` (nullable)

**⚠️ Offen:**
- `DayNumber` (1-7) deutet auf Wochenpläne hin, aber `MealPlan` ist flexibel (StartDate/EndDate)
- **Empfehlung:** `DayNumber` beibehalten für Wochenpläne, bei längeren Plänen: Tag-Nummer relativ zu StartDate
- Oder: `DayNumber` entfernen, nur `Date` verwenden (flexibler)

---

## 🔄 Beziehungen (ER-Diagramm)

```
Groups (1) ──< (N) GroupMembers (N) >── (1) Users
Groups (1) ──< (N) Recipes
Groups (1) ──< (N) MealPlans
Groups (1) ──< (N) Tags
Groups (1) ──< (N) GroupInvites

Recipes (1) ──< (N) RecipeIngredients
Recipes (1) ──< (N) RecipeTags (N) >── (1) Tags
Recipes (1) ──< (N) RecipeRatings (N) >── (1) Users
Recipes (1) ──< (1) NutritionInfos
Recipes (1) ──< (N) MealPlanEntries

MealPlans (1) ──< (N) MealPlanEntries
```

---

## 📋 Fehlende Tabellen (aus Analyse)

### `Files` (NEU - empfohlen)
Zentrale File-Verwaltung für PDFs, Bilder, etc.

| Spalte | Typ | Beschreibung |
|--------|-----|--------------|
| `Id` | int (PK) | Primärschlüssel |
| `GroupId` | int (FK) | Foreign Key zu `Groups` |
| `UploadedByUserId` | int (FK) | User, der hochgeladen hat |
| `FileName` | string | Original-Dateiname |
| `FilePath` | string | S3/Supabase Storage-Pfad |
| `ContentType` | string | MIME-Type (z.B. "image/png", "application/pdf") |
| `FileSizeBytes` | long | Dateigröße in Bytes |
| `Type` | FileType | Image, Pdf, Other |
| `RecipeId` | int? (FK) | Foreign Key zu `Recipes` (nullable) |
| `RecipeDraftId` | int? (FK) | Foreign Key zu `RecipeDrafts` (nullable) |
| `CreatedAt` | DateTime | Erstellungsdatum |
| `UpdatedAt` | DateTime | Letzte Änderung |

**Enum:**
```csharp
public enum FileType
{
    Image = 1,  // PNG, JPG, etc.
    Pdf = 2,    // PDF
    Other = 3   // Sonstige
}
```

**Navigation:**
- `Group` → `Group`
- `UploadedByUser` → `User`
- `Recipe` → `Recipe?` (nullable)
- `RecipeDraft` → `RecipeDraft?` (nullable)

**Vorteile:**
- Zentrale File-Verwaltung
- Metadaten (Größe, Content-Type)
- User-Referenz (wer hat hochgeladen?)
- Versionierung möglich
- Löschung von ungenutzten Files einfacher

**⚠️ Aktuell:** `Recipe.ImageUrl` und `Recipe.PdfUrl` als Strings  
**Empfohlen: Migration 6** (siehe `DATENBANK-UML-Diagramm.md`)

---

### `ShoppingLists` (NEU - empfohlen)
Einkaufslisten pro Group/Woche

| Spalte | Typ | Beschreibung |
|--------|-----|--------------|
| `Id` | int (PK) | Primärschlüssel |
| `GroupId` | int (FK) | Foreign Key zu `Groups` |
| `CreatedByUserId` | int (FK) | User, der erstellt hat |
| `Name` | string | Name der Liste (z.B. "Woche 1/2026") |
| `ForWeekStartDate` | DateTime | Startdatum der Woche |
| `ForWeekEndDate` | DateTime | Enddatum der Woche |
| `IsCompleted` | bool | Abgeschlossen? |
| `CompletedAt` | DateTime? | Abgeschlossen-Datum |
| `CreatedAt` | DateTime | Erstellungsdatum |
| `UpdatedAt` | DateTime | Letzte Änderung |

**Navigation:**
- `Group` → `Group`
- `CreatedByUser` → `User`
- `Items` → `ShoppingListItem[]`

**⚠️ Aktuell:** Frontend existiert (`/shopping-list`), aber keine DB-Struktur  
**Empfohlen: Migration 7** (siehe `DATENBANK-UML-Diagramm.md`)

---

### `ShoppingListItems` (NEU - empfohlen)
Einzelne Zutaten in einer Einkaufsliste

| Spalte | Typ | Beschreibung |
|--------|-----|--------------|
| `Id` | int (PK) | Primärschlüssel |
| `ShoppingListId` | int (FK) | Foreign Key zu `ShoppingLists` |
| `RecipeIngredientId` | int? (FK) | Foreign Key zu `RecipeIngredients` (nullable) |
| `IngredientName` | string | Zutatenname (denormalisiert) |
| `Quantity` | decimal? | Aggregierte Menge |
| `Unit` | string? | Einheit |
| `IsChecked` | bool | Abgehakt? |
| `Order` | int | Reihenfolge |
| `CreatedAt` | DateTime | Erstellungsdatum |
| `UpdatedAt` | DateTime | Letzte Änderung |

**Navigation:**
- `ShoppingList` → `ShoppingList`
- `RecipeIngredient` → `RecipeIngredient?` (nullable)

**Logik:**
- Aggregation aus `MealPlanEntries` → `Recipes` → `RecipeIngredients`
- Mehrere Rezepte mit gleicher Zutat → `Quantity` wird summiert

**Empfohlen: Migration 7** (siehe `DATENBANK-UML-Diagramm.md`)

---

### `RecipeDrafts` (Hoch priorisiert)
Für Review-Zwischenschritt bei Upload/KI-Generierung

| Spalte | Typ | Beschreibung |
|--------|-----|--------------|
| `Id` | int (PK) | Primärschlüssel |
| `GroupId` | int (FK) | Foreign Key zu `Groups` |
| `Status` | DraftStatus | Pending, Approved, Rejected |
| `Source` | RecipeSource | Generated, Upload |
| `OriginalData` | JSON? | Original-Daten (Upload/KI) |
| `CreatedByUserId` | int (FK) | User, der erstellt hat |
| `ReviewedByUserId` | int? (FK) | User, der reviewed hat |
| `ReviewedAt` | DateTime? | Review-Datum |
| `CreatedAt` | DateTime | Erstellungsdatum |
| `UpdatedAt` | DateTime | Letzte Änderung |
| `Name` | string (required) | Rezeptname (editierbar) |
| `Description` | string? | Beschreibung |
| `Instructions` | string? | Zubereitung |
| `MealCategory` | MealCategory? | Mahlzeit (nullable) |
| `Ingredients` | List<RecipeIngredient> | Zutaten (editierbar) |
| `Tags` | List<string> | Tags (editierbar) |

**Navigation:**
- Nach Approval → wird zu `Recipe`
- `CreatedByUser` → `User`
- `ReviewedByUser` → `User?`

**Empfohlen: Migration 3** (siehe `DATENBANK-Erweiterungen.md`)

---

### `Notifications` (Mittel priorisiert)
Benachrichtigungen

| Spalte | Typ | Beschreibung |
|--------|-----|--------------|
| `Id` | int (PK) | Primärschlüssel |
| `UserId` | int (FK) | Foreign Key zu `Users` |
| `Type` | NotificationType | UploadComplete, GenerationComplete, MealPlanReady, SimilarRecipeWarning |
| `Message` | string | Nachricht |
| `IsRead` | bool | Gelesen? |
| `ReadAt` | DateTime? | Gelesen-Datum |
| `ActionLink` | string? | Optional: Link zu Resource (z.B. RecipeDraft ID, MealPlan ID) |
| `CreatedAt` | DateTime | Erstellungsdatum |
| `UpdatedAt` | DateTime | Letzte Änderung |

**Navigation:**
- `User` → `User`

**Empfohlen: Migration 4** (siehe `DATENBANK-Erweiterungen.md`)

---

### `UploadTasks` (Mittel priorisiert)
Upload-Status-Tracking

| Spalte | Typ | Beschreibung |
|--------|-----|--------------|
| `Id` | int (PK) | Primärschlüssel |
| `UserId` | int (FK) | Foreign Key zu `Users` |
| `GroupId` | int (FK) | Foreign Key zu `Groups` |
| `FileName` | string | Dateiname |
| `FilePath` | string | Pfad in Storage (S3/Supabase) |
| `Status` | TaskStatus | Pending, Processing, Completed, Failed |
| `Error` | string? | Fehlermeldung (falls Failed) |
| `RecipeDraftId` | int? (FK) | Foreign Key zu `RecipeDrafts` (falls erfolgreich) |
| `CreatedAt` | DateTime | Erstellungsdatum |
| `UpdatedAt` | DateTime | Letzte Änderung |

**Navigation:**
- `User` → `User`
- `Group` → `Group`
- `RecipeDraft` → `RecipeDraft?` (nullable)

**Empfohlen: Migration 5** (siehe `DATENBANK-Erweiterungen.md`)

---

## 🎯 Empfohlene Erweiterungen

### 1. Recipe-Erweiterungen (Hoch priorisiert)

```csharp
public enum RecipeSource
{
    Generated = 1,  // KI-generiert
    Upload = 2,     // PDF-Upload
    Manual = 3      // Manuell erstellt
}

// In Recipe Entity hinzufügen:
public RecipeSource Source { get; set; } = RecipeSource.Manual;
public MealCategory? MealCategory { get; set; }  // nullable
public int? RepeatCycleWeeks { get; set; }       // nullable
```

### 2. MealPlan-Erweiterungen (Mittel priorisiert)

```csharp
public enum MealPlanStatus
{
    Draft = 1,
    Active = 2,
    Archived = 3
}

// In MealPlan Entity hinzufügen:
public MealPlanStatus Status { get; set; } = MealPlanStatus.Draft;
```

### 3. Recipe-ReadModel-Erweiterungen

```csharp
// In RecipeReadModel hinzufügen:
public decimal? AverageRating { get; set; }  // Aggregiert aus RecipeRatings
public int FavoriteCount { get; set; }        // Anzahl Favoriten
```

---

## 📝 Migration-Plan

1. **Migration 1:** Recipe-Erweiterungen (Source, MealCategory, RepeatCycleWeeks)
2. **Migration 2:** MealPlan.Status
3. **Migration 3:** RecipeDrafts-Tabelle
4. **Migration 4:** Notifications-Tabelle
5. **Migration 5:** UploadTasks-Tabelle
6. **Migration 6:** Files-Tabelle (zentrale File-Verwaltung)
7. **Migration 7:** ShoppingLists & ShoppingListItems (Einkaufsliste)

---

## 🔗 Referenzen

- `server/Ernaehrbar.Adapters.Infrastructure/Data/ApplicationDbContext.cs`
- `server/Ernaehrbar.Adapters.Infrastructure/Data/Entities/*.cs`
- `docs/ernaehrbaer/ERNAEHRBAR-Components.md`
- `docs/ernaehrbaer/ANALYSE-Komponenten.md`
- `docs/ernaehrbaer/DATENBANK-UML-Diagramm.md` – Vollständiges UML-Diagramm mit allen Beziehungen
