# session_2026-01-24_build-errors-fixes.md

## Ziel der Session

Behebung aller **Build-Fehler** im Ernährbär-Projekt nach der Implementierung des Fixture-Systems, um:
- Ein vollständig kompilierbares Projekt zu haben
- Alle Abhängigkeiten korrekt zu konfigurieren
- Tests mit der neuen Fixture-Integration zum Laufen zu bringen
- Konsistente Code-Qualität sicherzustellen

**Nicht-Ziel:**
- Neue Features hinzufügen
- Architektur-Änderungen
- Performance-Optimierungen

**Constraints:**
- Alle Änderungen müssen rückwärtskompatibel sein
- Bestehende Funktionalität darf nicht beeinträchtigt werden
- Code-Stil muss konsistent bleiben

---

## Kontext

- Fixture-System wurde in vorheriger Session implementiert
- Beim Build traten mehrere Fehler auf:
  - C# LangVersion-Konflikte
  - Namespace-Mehrdeutigkeiten
  - Fehlende using-Direktiven
  - Zugriffsprobleme mit Top-Level Statements
  - Package-Versionskonflikte
- Projekt verwendet Top-Level Statements in `Program.cs` (modernes .NET 9 Pattern)
- Tests müssen auf `Program`-Klasse zugreifen können

---

## Aufgaben / Requests

1. **LangVersion-Fehler beheben**
   - `Ernaehrbar.Fixtures.csproj` hatte `LangVersion>14` (nicht unterstützt)
   - Änderung auf `latest`

2. **Namespace-Mehrdeutigkeiten auflösen**
   - `RecipeSource`, `DraftStatus`, `TaskStatus` existieren sowohl in Domain als auch Entities
   - `File` kollidiert mit `System.IO.File`
   - Qualifizierung mit Aliases oder vollständigen Namespaces

3. **Fehlende using-Direktiven hinzufügen**
   - `Microsoft.OpenApi` für OpenAPI-Konfiguration
   - `Microsoft.Extensions.Hosting` für Tests
   - `Xunit` für Test-Attribute
   - `Microsoft.Extensions.Configuration` für Test-Konfiguration
   - `Microsoft.EntityFrameworkCore` für EF Core-Methoden

4. **Top-Level Statements Zugriffsproblem lösen**
   - `Program`-Klasse ist durch Top-Level Statements intern
   - Tests benötigen öffentlichen Zugriff
   - Lösung: `public partial class Program` im globalen Namespace

5. **Package-Versionskonflikte beheben**
   - `Microsoft.OpenApi` Downgrade-Warnung
   - Explizite Referenz entfernt (wird transitiv bereitgestellt)

6. **OpenAPI-Konfiguration korrigieren**
   - `OpenApiSpecVersion.OpenApi3_1` existiert nicht
   - Änderung auf `OpenApi3_0`

---

## Beobachtungen (ohne Bewertung)

- Top-Level Statements erzeugen automatisch eine interne `Program`-Klasse im globalen Namespace
- `partial class` kann verwendet werden, um Zugriff zu ermöglichen, muss aber im gleichen Namespace sein
- Mehrere Enums haben gleiche Namen in Domain und Entities (Design-Entscheidung)
- `Microsoft.AspNetCore.OpenApi` stellt bereits `Microsoft.OpenApi` transitiv bereit
- OpenAPI 3.1 wird nicht von allen Tools unterstützt (Zentreo verwendet auch 3.0)

---

## Offene Fragen

- Sollten Domain- und Entities-Enums konsolidiert werden? (Aktuell: Beide existieren parallel)
- Ist OpenAPI 3.0 ausreichend oder sollte 3.1 unterstützt werden, sobald verfügbar?
- Sollte `Program` explizit als Klasse definiert werden statt Top-Level Statements? (Aktuell: Top-Level Statements beibehalten)

---

## Ergebnisse dieser Session

### Behobene Fehler

1. **LangVersion-Fehler**
   - ✅ `Ernaehrbar.Fixtures.csproj`: `LangVersion>14` → `latest`

2. **Namespace-Mehrdeutigkeiten**
   - ✅ `FileFixture.cs`: `Entities.File` statt `File` (Alias hinzugefügt)
   - ✅ `UploadTaskFixture.cs`: `Entities.TaskStatus` vollständig qualifiziert
   - ✅ `RecipeDraftFixture.cs`: `Entities.RecipeSource` und `Entities.DraftStatus` verwendet
   - ✅ `RecipeFixture.cs`: `Entities.RecipeSource` verwendet
   - ✅ `RecipeDraftsControllerTests.cs`: `Entities.RecipeSource` und `Entities.DraftStatus` verwendet

3. **Fehlende using-Direktiven**
   - ✅ `ServiceCollectionExtensions.cs`: `using Microsoft.OpenApi;` hinzugefügt
   - ✅ `RecipeDraftsController.cs`: `using Ernaehrbar.Parts.Ports;` für `MealCategory`
   - ✅ `BaseE2ETest.cs`: `using Xunit;` hinzugefügt
   - ✅ `CustomWebApplicationFactory.cs`: `using Microsoft.Extensions.Hosting;` und `using Microsoft.Extensions.Configuration;` hinzugefügt
   - ✅ `ShoppingListFixture.cs`: `using Microsoft.EntityFrameworkCore;` für `ToListAsync`

4. **Top-Level Statements Zugriff**
   - ✅ `Program.cs`: `public partial class Program { }` am Ende hinzugefügt (globaler Namespace)
   - ✅ `Ernaehrbar.Api.csproj`: `InternalsVisibleTo` für Tests hinzugefügt
   - ✅ Tests: `global::Program` verwendet statt Alias (vermeidet Konflikte)

5. **Package-Konflikte**
   - ✅ Explizite `Microsoft.OpenApi` Referenz entfernt (wird transitiv bereitgestellt)

6. **OpenAPI-Konfiguration**
   - ✅ `OpenApiSpecVersion.OpenApi3_1` → `OpenApi3_0`

7. **Weitere Fixes**
   - ✅ `ShoppingListFixture.cs`: `ingredients.Any()` → `ingredients.Count > 0` (Operator-Fehler)
   - ✅ `CustomWebApplicationFactory.cs`: `AddXunit` entfernt (wird über Serilog gehandhabt)
   - ✅ `Ernaehrbar.Fixtures.csproj`: `<OutputType>Exe</OutputType>` hinzugefügt (für Top-Level Statements)

### Geänderte Dateien

1. **Projekt-Dateien:**
   - `Ernaehrbar.Fixtures/Ernaehrbar.Fixtures.csproj`
   - `Ernaehrbar.Adapters.Api/Ernaehrbar.Adapters.Api.csproj`
   - `Ernaehrbar.Api/Ernaehrbar.Api.csproj`

2. **Fixture-Dateien:**
   - `FileFixture.cs`
   - `UploadTaskFixture.cs`
   - `RecipeDraftFixture.cs`
   - `RecipeFixture.cs`
   - `ShoppingListFixture.cs`

3. **API-Dateien:**
   - `ServiceCollectionExtensions.cs`
   - `RecipeDraftsController.cs`
   - `Program.cs`

4. **Test-Dateien:**
   - `BaseE2ETest.cs`
   - `CustomWebApplicationFactory.cs`
   - `RecipeDraftsControllerTests.cs`

### Funktionalität

- ✅ Projekt kompiliert ohne Fehler
- ✅ Alle Namespace-Konflikte aufgelöst
- ✅ Tests können auf `Program` zugreifen
- ✅ OpenAPI korrekt konfiguriert
- ✅ Alle Abhängigkeiten korrekt aufgelöst

---

## Erkenntnisse / Lessons Learned

1. **Top-Level Statements und Tests:**
   - Top-Level Statements erzeugen eine interne `Program`-Klasse im globalen Namespace
   - `public partial class Program` im globalen Namespace macht sie für Tests zugänglich
   - `global::Program` in Tests vermeidet Namespace-Konflikte

2. **Namespace-Mehrdeutigkeiten:**
   - Wenn gleiche Namen in verschiedenen Namespaces existieren, Aliases verwenden
   - Entities-Versionen sollten in Tests verwendet werden (da mit Datenbank gearbeitet wird)
   - Domain-Versionen für Business-Logik

3. **Package-Management:**
   - Nicht explizit referenzieren, was bereits transitiv bereitgestellt wird
   - Versionskonflikte durch Entfernen expliziter Referenzen beheben

4. **OpenAPI:**
   - Version 3.0 ist stabiler und besser unterstützt als 3.1
   - Zentreo verwendet auch 3.0 aus Kompatibilitätsgründen

---

## Validierungsfragen (Pflicht)

### Was hat funktioniert?

- Systematische Fehlerbehebung durch Analyse der Build-Ausgabe
- Verwendung von Aliases für Namespace-Konflikte
- `partial class` Pattern für Top-Level Statements Zugriff
- Entfernen redundanter Package-Referenzen

### Was hat nicht funktioniert?

- Initialer Versuch, `Program` in einen Namespace zu verschieben (Top-Level Statements müssen außerhalb sein)
- Alias `using Program = global::Program;` verursachte Konflikte (direkte Verwendung von `global::Program` besser)

### Was würden wir beim nächsten Mal anders machen?

- Früher prüfen, ob Top-Level Statements mit Tests kompatibel sind
- Namespace-Aliases von Anfang an verwenden, wenn Mehrdeutigkeiten bekannt sind
- Package-Abhängigkeiten vorher analysieren (transitive Dependencies)

### Welche Regeln sollten daraus abgeleitet werden?

1. **Top-Level Statements Pattern:**
   - Wenn Top-Level Statements verwendet werden, immer `public partial class Program { }` am Ende hinzufügen
   - Tests verwenden `global::Program` für expliziten Zugriff

2. **Namespace-Management:**
   - Bei bekannten Mehrdeutigkeiten (Domain vs. Entities) sofort Aliases verwenden
   - Entities-Versionen in Tests und Infrastructure, Domain-Versionen in Business-Logik

3. **Package-Referenzen:**
   - Nur explizit referenzieren, was wirklich benötigt wird
   - Transitive Dependencies nicht explizit hinzufügen

4. **Build-Fehler-Analyse:**
   - Systematisch vorgehen: LangVersion → Namespaces → using-Direktiven → Zugriffsprobleme
   - Jeden Fehler einzeln beheben und testen

---

## Nächste Schritte

- ✅ Projekt kompiliert erfolgreich
- ⏭️ Tests ausführen, um Funktionalität zu validieren
- ⏭️ Dokumentation aktualisieren, falls nötig
- ⏭️ CI/CD Pipeline prüfen, ob Build dort auch funktioniert

---

## Referenzen

- Vorherige Session: `session_2026-01-24_database-reset-fixtures.md` (gleicher Ordner)
- Zentreo-Referenz: `tp_dym-zentreo-v1/tp_dym-zentreo-v1-120596d83764`
- .NET 9 Dokumentation: Top-Level Statements und Testing
