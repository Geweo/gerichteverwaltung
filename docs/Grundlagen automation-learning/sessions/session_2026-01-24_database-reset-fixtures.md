# session_2026-01-24_database-reset-fixtures.md

## Ziel der Session

Erstellung eines **Datenbank-Reset-Skripts** und eines **vollständigen Fixture-Systems** für Ernährbär, ähnlich wie bei Zentreo, um:
- Datenbank schnell zurücksetzen zu können
- Konsistente Testdaten für alle Tabellen zu haben
- Tests mit Fixtures statt manueller Daten-Erstellung laufen zu lassen
- Entwicklungsumgebung mit realistischen Daten zu versorgen

**Nicht-Ziel:**
- Änderung der bestehenden Datenbank-Struktur
- Migration von bestehenden Daten
- Änderung der Unit Tests (die verwenden weiterhin Mocks)

**Constraints:**
- Struktur muss ähnlich zu Zentreo sein (Architektur-Vorgabe)
- Alle Tabellen müssen Fixtures haben
- Skripte müssen auf Windows (PowerShell) und Linux/Mac (Bash) funktionieren

---

## Kontext

- Ernährbär-Projekt wurde bereits an Zentreo-Architektur angeglichen
- Es existieren bereits Test-Fixtures (`TestFixtures` Helper-Klasse), aber keine Development-Fixtures
- Tests erstellen aktuell Daten manuell pro Test
- Benutzer möchte Fixtures für alle Tabellen und Integration in Tests

---

## Aufgaben / Requests

1. **Datenbank-Reset-Skript erstellen**
   - Ähnlich wie bei Zentreo
   - Datenbank löschen
   - Migrations anwenden
   - Fixtures laden

2. **Fixture-System erstellen**
   - `Ernaehrbar.Fixtures` Projekt
   - Fixtures für alle Tabellen:
     - Groups & Users (Group, User, GroupMember, GroupInvite)
     - Recipes (Recipe, RecipeIngredient, Tag, RecipeTag, RecipeRating, NutritionInfo)
     - Meal Plans (MealPlan, MealPlanEntry)
     - Recipe Drafts (RecipeDraft, RecipeDraftIngredient)
     - Notifications, UploadTasks, Files, ShoppingLists

3. **Tests anpassen**
   - Integration Tests sollen Fixtures verwenden
   - Unit Tests bleiben unverändert (Mocks)

---

## Beobachtungen (ohne Bewertung)

- Zentreo verwendet Development Fixtures, aber Tests erstellen Daten manuell
- Benutzer wollte explizit, dass Tests die Fixtures verwenden (Abweichung von Zentreo)
- Es gab initial Unklarheit, ob Tests bereits Fixtures verwenden oder nicht
- Die Struktur von Zentreo Fixtures wurde als Vorlage verwendet
- Alle Tabellen wurden systematisch durchgegangen und Fixtures erstellt
- Die Abhängigkeiten zwischen Entitäten wurden berücksichtigt (Reihenfolge beim Seeding)

---

## Offene Fragen

- Sollen die Fixtures in Tests auch für Unit Tests verwendet werden? (Aktuell: Nein, nur Integration Tests)
- Wie oft werden die Fixtures in der Praxis aktualisiert werden müssen?
- Soll es verschiedene Fixture-Sets geben (z.B. Minimal vs. Extended)?

---

## Ergebnisse dieser Session

### Erstellte Dateien

1. **Skripte:**
   - `ops/scripts/reset-database.sh` - Bash-Version
   - `ops/scripts/reset-database.ps1` - PowerShell-Version

2. **Fixture-Projekt:**
   - `server/Ernaehrbar.Fixtures/` - Komplettes Fixture-Projekt
   - `Utilities/SeedableFixture.cs` - Basis-Klassen
   - `Sets/Development/` - Alle Fixture-Klassen für alle Tabellen
   - `Configuration/ServiceCollectionExtensions.cs` - DI-Registrierung
   - `Program.cs` - Fixture-Loader

3. **Test-Anpassungen:**
   - `Ernaehrbar.Tests.csproj` - Projekt-Referenz zu Fixtures
   - `CustomWebApplicationFactory.cs` - Fixtures werden geladen
   - `BaseE2ETest.cs` - Zugriff auf Fixtures
   - `RecipeDraftsControllerTests.cs` - Verwendet Fixture-Daten

### Funktionalität

- ✅ Datenbank kann mit einem Skript zurückgesetzt werden
- ✅ Fixtures werden automatisch geladen
- ✅ Alle Tabellen haben Fixtures
- ✅ Tests verwenden Fixture-Daten
- ✅ Struktur ähnlich zu Zentreo

---

## Hinweise für Folge-Sessions

- Fixtures können erweitert werden, wenn neue Tabellen hinzukommen
- Bei neuen Tabellen: Fixture erstellen und in `DevelopmentFixtureSet` in korrekter Reihenfolge einfügen
- Tests sollten weiterhin Fixture-Daten verwenden, nicht manuell erstellen

---

## Pflicht-Reflexionsfragen

### 1. Was war unklar?

- **Initial:** Ob Tests bereits Fixtures verwenden oder manuell Daten erstellen
- **Während der Arbeit:** Die genaue Struktur der Entity-Beziehungen (z.B. ShoppingListItem hat `IngredientName` statt `Name`)
- **Am Ende:** Ob Unit Tests auch Fixtures verwenden sollen (wurde geklärt: Nein)

### 2. Wo gab es implizite Annahmen?

- **Annahme:** Tests verwenden bereits Fixtures (war falsch - sie erstellen manuell)
- **Annahme:** Struktur ist identisch zu Zentreo (war größtenteils richtig, aber Details unterschiedlich)
- **Annahme:** Alle Entity-Namen sind konsistent (z.B. `IngredientName` vs. `Name`)

### 3. Welche Information kam zu spät?

- Die Information, dass Tests Fixtures verwenden sollen, kam erst nach der Frage
- Die genaue Struktur von `ShoppingListItem` (IngredientName statt Name) wurde erst spät erkannt
- Die Information über NotificationType-Enum-Werte kam erst beim Implementieren

### 4. Was hätte man systemisch besser machen können?

- **Frühere Klärung:** Hätte zuerst alle Entity-Strukturen vollständig lesen sollen, bevor Fixtures erstellt werden
- **Systematischer Ansatz:** Hätte eine Checkliste aller Tabellen erstellen sollen und dann systematisch durchgehen
- **Validierung:** Hätte nach dem Erstellen der Fixtures eine Validierung durchführen sollen (z.B. Build-Test)
- **Dokumentation:** Hätte während der Arbeit mehr Notizen machen sollen über Abhängigkeiten

---

## Status

Session abgeschlossen. Alle Aufgaben erledigt:
- ✅ Reset-Skripte erstellt
- ✅ Fixture-System vollständig implementiert
- ✅ Tests angepasst
- ✅ Dokumentation aktualisiert

Kein weiterer Handlungsbedarf auf Session-Ebene.
