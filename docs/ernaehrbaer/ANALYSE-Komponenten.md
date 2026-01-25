# Analyse: ERNAEHRBAR-Components.md

**Datum:** 2026-01-24  
**Zweck:** Identifikation offener Punkte, logischer Fehler und fehlender Aspekte

---

## ✅ Was ist gut definiert

1. **Klare Komponentenstruktur** – Dashboard, Rezept-DB, Wochenplan, etc.
2. **Zustandsgetriebene Architektur** – Asynchron, Background Tasks
3. **UI-geführter Workflow** – Klare Trennung zwischen Planung, Verwaltung, Generierung
4. **Review-Prozess** – Qualitätssicherung vor DB-Aufnahme

---

## ⚠️ Offene Punkte & Logische Fehler

### 1. **Rezept-Source fehlt in DB** ⚠️ NOCH OFFEN

**Problem:**  
Komponenten-Dokumentation erwähnt "Type: generiert / upload" in der Rezept-Tabelle, aber die `Recipe`-Entity hat kein `Source`-Feld.

**Aktueller Zustand:**
- `Recipe` Entity hat keine `Source` oder `RecipeSource` Property
- Keine Unterscheidung zwischen KI-generierten und Upload-Rezepten

**Status:**
- ✅ Dokumentation angepasst (`ERNAEHRBAR-Components.md`: "Source" statt "Type")
- ✅ Klarstellung erstellt (`KLARSTELLUNGEN.md`)
- ✅ Implementierungsplan erstellt (`DATENBANK-Erweiterungen.md`)
- ❌ **Code noch nicht implementiert** (Entity erweitern, Migration erstellen)

**Empfehlung:**
- `RecipeSource` Enum hinzufügen: `Generated`, `Upload`, `Manual`
- `Recipe.Source` Property hinzufügen
- Migration erstellen

**Referenz:**  
- `ERNAEHRBAR-Components.md` Zeile 87: "Source | Generiert / Upload / Manuell"
- `server/Ernaehrbar.Adapters.Infrastructure/Data/Entities/Recipe.cs`
- `docs/ernaehrbaer/DATENBANK-Erweiterungen.md` (Migration 1)

---

### 2. **"Gerichttyp" vs. "Mahlzeit" – Inkonsistenz** ⚠️ NOCH OFFEN

**Problem:**  
Komponenten-Dokumentation verwendet:
- "Gerichttyp" (Frühstück / Mittag / Abend) in Rezept-DB
- "Mahlzeit" im Dashboard und Wizard

**Aktueller Zustand:**
- `Recipe` Entity hat **kein** `MealCategory` Feld
- `MealPlanEntry` hat `MealCategory` (Breakfast, Lunch, Dinner)
- Tags können `TagCategory` haben, aber kein direkter Bezug zu Mahlzeiten

**Status:**
- ✅ Dokumentation angepasst (konsistent: "Mahlzeit" überall)
- ✅ Klarstellung erstellt (`KLARSTELLUNGEN.md`)
- ✅ Implementierungsplan erstellt (`DATENBANK-Erweiterungen.md`)
- ❌ **Code noch nicht implementiert** (Entity erweitern, Migration erstellen)

**Empfehlung:**
- `Recipe.MealCategory` Property hinzufügen (nullable, da Rezepte flexibel sein können)
- Migration erstellen

**Referenz:**  
- `ERNAEHRBAR-Components.md` Zeile 85: "Mahlzeit | Frühstück / Mittag / Abend"
- `server/Ernaehrbar.Adapters.Infrastructure/Data/Entities/Recipe.cs`
- `docs/ernaehrbaer/DATENBANK-Erweiterungen.md` (Migration 1)

---

### 3. **"Wiederholungszyklus" fehlt in DB** ⚠️ NOCH OFFEN

**Problem:**  
Komponenten-Dokumentation erwähnt "Wiederholungszyklus | z. B. alle X Wochen" in der Rezept-Tabelle.

**Aktueller Zustand:**
- `Recipe` Entity hat kein `RepeatCycleWeeks` oder ähnliches Feld

**Status:**
- ✅ Dokumentation angepasst
- ✅ Implementierungsplan erstellt (`DATENBANK-Erweiterungen.md`)
- ❌ **Code noch nicht implementiert** (Entity erweitern, Migration erstellen)

**Empfehlung:**
- `Recipe.RepeatCycleWeeks` (int?, nullable) hinzufügen
- Migration erstellen

**Referenz:**  
- `ERNAEHRBAR-Components.md` Zeile 89: "Wiederholungszyklus | z. B. alle X Wochen"
- `docs/ernaehrbaer/DATENBANK-Erweiterungen.md` (Migration 1)

---

### 4. **"Bewertung" – Aggregation unklar** ✅ GEKLÄRT

**Problem:**  
Komponenten-Dokumentation erwähnt "Bewertung | manuell" in der Rezept-Tabelle.

**Aktueller Zustand:**
- `RecipeRating` Entity existiert (pro User)
- Keine aggregierte Bewertung auf `Recipe`-Ebene

**Status:**
- ✅ Klarstellung erstellt (`KLARSTELLUNGEN.md`): ReadModel-Aggregation
- ✅ Dokumentation angepasst: "Bewertung | Durchschnitt (aggregiert)"
- ⏭️ **ReadModel noch nicht erweitert** (AverageRating, FavoriteCount)

**Empfehlung:**
- ReadModel: `RecipeReadModel.AverageRating` (berechnet)
- ReadModel: `RecipeReadModel.FavoriteCount` (berechnet)

**Referenz:**  
- `ERNAEHRBAR-Components.md` Zeile 88: "Bewertung | Durchschnitt (aggregiert)"
- `server/Ernaehrbar.Adapters.Infrastructure/Data/Entities/RecipeRating.cs`
- `docs/ernaehrbaer/KLARSTELLUNGEN.md`

---

### 5. **"Nährwert aggregiert" – Unklar** ✅ GEKLÄRT

**Problem:**  
Komponenten-Dokumentation erwähnt "Nährwert | aggregiert" in der Rezept-Tabelle.

**Aktueller Zustand:**
- `NutritionInfo` Entity existiert (1:1 mit Recipe)
- Nährwerte sind pro Serving, nicht aggregiert

**Status:**
- ✅ Klarstellung erstellt (`KLARSTELLUNGEN.md`): "pro Portion"
- ✅ Dokumentation angepasst: "Nährwert | pro Portion"

**Empfehlung:**
- ✅ **Erledigt:** "Aggregiert" = "pro Portion" (pro Serving)

**Referenz:**  
- `ERNAEHRBAR-Components.md` Zeile 87: "Nährwert | pro Portion"
- `server/Ernaehrbar.Adapters.Infrastructure/Data/Entities/NutritionInfo.cs`
- `docs/ernaehrbaer/KLARSTELLUNGEN.md`

---

### 6. **Favoriten-Logik unklar** ✅ GEKLÄRT

**Problem:**  
Komponenten-Dokumentation erwähnt Favoriten-Buttons, aber:
- Favoriten sind pro User (`RecipeRating.IsFavorite`)
- Dashboard zeigt "Favorisiert"-Filter
- Wie werden Favoriten aggregiert (pro Group, pro User)?

**Aktueller Zustand:**
- `RecipeRating.IsFavorite` existiert (pro User)
- Keine Group-Level-Favoriten

**Status:**
- ✅ Klarstellung erstellt (`KLARSTELLUNGEN.md`): Favoriten sind pro User
- ✅ Dokumentation angepasst: "Favorisiert (vom aktuellen User favorisiert)"

**Empfehlung:**
- ✅ **Erledigt:** Favoriten sind pro User
- ✅ **Erledigt:** Filter "Favorisiert" = "vom aktuellen User favorisiert"

**Referenz:**  
- `ERNAEHRBAR-Components.md` Zeile 54: "Favoriten-Button"
- `server/Ernaehrbar.Adapters.Infrastructure/Data/Entities/RecipeRating.cs`
- `docs/ernaehrbaer/KLARSTELLUNGEN.md`

---

### 7. **Wochenplan-Wizard: "Maximal: 7 Tage" – Warum?** ✅ GEKLÄRT

**Problem:**  
Komponenten-Dokumentation sagt "Maximal: 7 Tage" für Wochenplan-Wizard.

**Aktueller Zustand:**
- `MealPlan` hat `StartDate` und `EndDate` (flexibel)
- `MealPlanEntry.DayNumber` (1-7) deutet auf Wochenpläne hin

**Status:**
- ✅ Klarstellung erstellt (`KLARSTELLUNGEN.md`): MVP-Fokus auf Wochenplanung
- ✅ Dokumentation angepasst: "Aktuell auf 7 Tage beschränkt (Wochenplan), später flexibel erweiterbar"

**Empfehlung:**
- ✅ **Erledigt:** MVP-Fokus auf 7 Tage, später erweiterbar

**Referenz:**  
- `ERNAEHRBAR-Components.md` Zeile 188: "Aktuell auf 7 Tage beschränkt"
- `server/Ernaehrbar.Adapters.Infrastructure/Data/Entities/MealPlanEntry.cs`
- `docs/ernaehrbaer/KLARSTELLUNGEN.md`

---

### 8. **Wochenplan-Wizard: "Verteilung" unklar** ✅ GEKLÄRT

**Problem:**  
Komponenten-Dokumentation erwähnt "Verteilung: ⅓ neu / ⅔ DB" etc.

**Aktueller Zustand:**
- Keine Logik für Verteilung zwischen neuen und DB-Rezepten
- `GenerateRecipesCommand` generiert nur neue Rezepte

**Status:**
- ✅ Klarstellung erstellt (`KLARSTELLUNGEN.md`): DB-Auswahl-Logik definiert
- ✅ Dokumentation angepasst: Verteilungs-Logik ergänzt
- ⏭️ **Code noch nicht implementiert** (Business-Logik in CommandHandler)

**Empfehlung:**
- ✅ **Definiert:** Zufällig + Gewichtet nach Favoriten + Wiederholungszyklus + Tags/Mahlzeit
- ⏭️ **Implementieren:** In `GenerateMealPlanCommandHandler`

**Referenz:**  
- `ERNAEHRBAR-Components.md` Zeile 212-215: "Verteilung" (erweitert)
- `docs/ernaehrbaer/KLARSTELLUNGEN.md`

---

### 9. **Ähnlichkeitslogik – Keine DB-Struktur** ✅ GEKLÄRT

**Problem:**  
Komponenten-Dokumentation erwähnt "Ähnlichkeitslogik" (Zutaten-Overlap, Tags, Zubereitungsart).

**Aktueller Zustand:**
- Keine DB-Struktur für Ähnlichkeitsberechnung
- Keine Caching-Mechanismus für Ähnlichkeiten

**Status:**
- ✅ Klarstellung erstellt (`KLARSTELLUNGEN.md`): On-the-fly, später optional Caching
- ✅ Dokumentation angepasst: Berechnungslogik beschrieben
- ⏭️ **Code noch nicht implementiert** (Ähnlichkeits-Algorithmus)

**Empfehlung:**
- ✅ **Entscheidung:** Aktuell on-the-fly, später optional `RecipeSimilarity` Entity
- ⏭️ **Implementieren:** Ähnlichkeits-Algorithmus bei Review

**Referenz:**  
- `ERNAEHRBAR-Components.md` Zeile 271-277: "Ähnlichkeitslogik" (erweitert)
- `docs/ernaehrbaer/KLARSTELLUNGEN.md`

---

### 10. **Notifications – Keine DB-Struktur** ⚠️ NOCH OFFEN

**Problem:**  
Komponenten-Dokumentation erwähnt Notifications, aber keine DB-Struktur.

**Aktueller Zustand:**
- Keine `Notification` Entity
- Keine `UserNotification` Entity

**Status:**
- ✅ Dokumentation angepasst (`ERNAEHRBAR-Components.md`: Notification-Entity beschrieben)
- ✅ Implementierungsplan erstellt (`DATENBANK-Erweiterungen.md`)
- ❌ **Code noch nicht implementiert** (Entity erstellen, Migration erstellen)

**Empfehlung:**
- `Notification` Entity hinzufügen (Type, Message, CreatedAt, IsRead, ActionLink)
- Migration erstellen

**Referenz:**  
- `ERNAEHRBAR-Components.md` Zeile 236-246: "Notifications" (erweitert)
- `docs/ernaehrbaer/DATENBANK-Erweiterungen.md` (Migration 4)

---

### 11. **Review & Validierung – Keine DB-Struktur** ⚠️ NOCH OFFEN

**Problem:**  
Komponenten-Dokumentation erwähnt "Review-Zwischenschritt" für Uploads/KI-Generierung.

**Aktueller Zustand:**
- Keine `RecipeDraft` oder `RecipePendingReview` Entity
- Rezepte werden direkt in `Recipe` gespeichert

**Status:**
- ✅ Dokumentation angepasst (`ERNAEHRBAR-Components.md`: RecipeDraft-Workflow beschrieben)
- ✅ Klarstellung erstellt (`KLARSTELLUNGEN.md`: Review-Prozess)
- ✅ Implementierungsplan erstellt (`DATENBANK-Erweiterungen.md`)
- ❌ **Code noch nicht implementiert** (Entity erstellen, Migration erstellen)

**Empfehlung:**
- `RecipeDraft` Entity hinzufügen (Status: Pending, Approved, Rejected)
- Migration erstellen

**Referenz:**  
- `ERNAEHRBAR-Components.md` Zeile 147-159: "Review & Validierung" (erweitert)
- `docs/ernaehrbaer/DATENBANK-Erweiterungen.md` (Migration 3)
- `docs/ernaehrbaer/KLARSTELLUNGEN.md`

---

### 12. **Automatisierungen – Keine DB-Struktur** ✅ GEKLÄRT

**Problem:**  
Komponenten-Dokumentation erwähnt "Automatische Wochenplan-Erstellung" mit Trigger.

**Aktueller Zustand:**
- Keine `AutomationRule` oder `ScheduledTask` Entity
- Keine Konfiguration für Automatisierungen

**Status:**
- ✅ Klarstellung erstellt (`KLARSTELLUNGEN.md`): Aktuell Backend-Config, später optional DB
- ✅ Dokumentation angepasst: Hinweis ergänzt

**Empfehlung:**
- ✅ **Entscheidung:** Aktuell Backend-Konfiguration (appsettings), später optional `AutomationRule` Entity

**Referenz:**  
- `ERNAEHRBAR-Components.md` Zeile 260-268: "Automatisierungen" (erweitert)
- `docs/ernaehrbaer/KLARSTELLUNGEN.md`

---

## 🔍 Fehlende Aspekte

### 1. **User-Management & Gruppen** ✅ DOKUMENTIERT

**Fehlt in Komponenten-Dokumentation:**
- Wie werden Gruppen erstellt?
- Wie werden User zu Gruppen eingeladen?
- Gruppen-Verwaltung (Admin vs. Member)

**Aktueller Zustand:**
- `Group`, `GroupMember`, `GroupInvite` Entities existieren
- Keine UI-Komponenten definiert

**Status:**
- ✅ Dokumentation ergänzt (`ERNAEHRBAR-Components.md`: User-Management & Gruppen)
- ⏭️ **UI-Komponenten noch nicht implementiert**

**Empfehlung:**
- ✅ **Dokumentiert:** Gruppen-Verwaltung in Komponenten-Dokumentation ergänzt
- ⏭️ **Implementieren:** UI-Komponenten für Gruppen-Verwaltung

---

### 2. **Rezept-Bearbeitung** ✅ DOKUMENTIERT

**Fehlt in Komponenten-Dokumentation:**
- Wie werden Rezepte bearbeitet?
- Inline-Editierung in Tabelle vs. Detail-Dialog?
- Versionierung von Rezepten?

**Aktueller Zustand:**
- `Recipe` Entity ist editierbar
- Keine Versionierung

**Status:**
- ✅ Dokumentation ergänzt (`ERNAEHRBAR-Components.md`: Rezept-Bearbeitung)
- ⏭️ **UI-Komponenten noch nicht implementiert**

**Empfehlung:**
- ✅ **Dokumentiert:** Rezept-Bearbeitung in Komponenten-Dokumentation ergänzt
- ⏭️ **Implementieren:** UI-Komponenten für Rezept-Bearbeitung

---

### 3. **Rezept-Löschung** ✅ DOKUMENTIERT

**Fehlt in Komponenten-Dokumentation:**
- Soft Delete vs. Hard Delete?
- Was passiert mit MealPlanEntries, wenn Rezept gelöscht wird?

**Aktueller Zustand:**
- `MealPlanEntry.RecipeId` ist nullable (SetNull bei Delete)
- Keine Soft Delete

**Status:**
- ✅ Dokumentation ergänzt (`ERNAEHRBAR-Components.md`: Rezept-Löschung)
- ✅ Klarstellung erstellt (`KLARSTELLUNGEN.md`: Hard Delete)

**Empfehlung:**
- ✅ **Dokumentiert:** Lösch-Logik in Komponenten-Dokumentation ergänzt
- ✅ **Entscheidung:** Hard Delete mit SetNull bei MealPlanEntries

---

### 4. **MealPlan-Status** ⚠️ NOCH OFFEN

**Fehlt in Komponenten-Dokumentation:**
- Status von MealPlans (Draft, Active, Archived)?
- Mehrere aktive MealPlans gleichzeitig?

**Aktueller Zustand:**
- `MealPlan` hat keinen Status
- Keine Einschränkung für mehrere aktive Pläne

**Status:**
- ✅ Dokumentation angepasst (`ERNAEHRBAR-Components.md`: MealPlan-Status erwähnt)
- ✅ Klarstellung erstellt (`KLARSTELLUNGEN.md`: Status-Workflow)
- ✅ Implementierungsplan erstellt (`DATENBANK-Erweiterungen.md`)
- ❌ **Code noch nicht implementiert** (Entity erweitern, Migration erstellen)

**Empfehlung:**
- `MealPlan.Status` Property hinzufügen (Draft, Active, Archived)
- Migration erstellen

**Referenz:**
- `docs/ernaehrbaer/DATENBANK-Erweiterungen.md` (Migration 2)
- `docs/ernaehrbaer/KLARSTELLUNGEN.md`

---

### 5. **Upload-Status-Tracking** ⚠️ NOCH OFFEN

**Fehlt in Komponenten-Dokumentation:**
- Wie wird Upload-Status getrackt?
- Background Task Status (Processing, Completed, Failed)?

**Aktueller Zustand:**
- Keine `UploadTask` oder `BackgroundTask` Entity

**Status:**
- ✅ Dokumentation angepasst (`ERNAEHRBAR-Components.md`: UploadTask-Workflow beschrieben)
- ✅ Implementierungsplan erstellt (`DATENBANK-Erweiterungen.md`)
- ❌ **Code noch nicht implementiert** (Entity erstellen, Migration erstellen)

**Empfehlung:**
- `UploadTask` Entity hinzufügen (Status: Pending, Processing, Completed, Failed)
- Migration erstellen

**Referenz:**
- `docs/ernaehrbaer/DATENBANK-Erweiterungen.md` (Migration 5)

---

## 📊 Datenbankstruktur-Analyse

### ✅ Vorhandene Tabellen

| Tabelle | Zweck | Status |
|---------|-------|--------|
| `Groups` | Multi-Tenant-Gruppen | ✅ Vollständig |
| `Users` | User (Supabase-Mapping) | ✅ Vollständig |
| `GroupMembers` | User-Gruppen-Zugehörigkeit | ✅ Vollständig |
| `GroupInvites` | Gruppen-Einladungen | ✅ Vollständig |
| `Recipes` | Rezepte | ⚠️ Fehlt: Source, MealCategory, RepeatCycle |
| `RecipeIngredients` | Zutaten | ✅ Vollständig |
| `Tags` | Tags (kategorisiert) | ✅ Vollständig |
| `RecipeTags` | Rezept-Tag-Zuordnung | ✅ Vollständig |
| `RecipeRatings` | Bewertungen & Favoriten | ✅ Vollständig |
| `NutritionInfos` | Nährwerte | ✅ Vollständig |
| `MealPlans` | Wochenpläne | ⚠️ Fehlt: Status |
| `MealPlanEntries` | Mahlzeiten in Plänen | ✅ Vollständig |

### ❌ Fehlende Tabellen

| Tabelle | Zweck | Priorität |
|---------|-------|-----------|
| `RecipeDrafts` | Review-Zwischenschritt | Hoch |
| `Notifications` | Benachrichtigungen | Mittel |
| `AutomationRules` | Automatisierungs-Konfiguration | Niedrig |
| `UploadTasks` | Upload-Status-Tracking | Mittel |
| `RecipeSimilarities` | Ähnlichkeits-Cache | Niedrig |

---

## 🎯 Empfehlungen: Prioritäten

### **Hoch priorisiert**

1. **Recipe.Source** hinzufügen (Generated, Upload, Manual)
2. **Recipe.MealCategory** hinzufügen (nullable)
3. **Recipe.RepeatCycleWeeks** hinzufügen (nullable)
4. **RecipeDraft** Entity für Review-Prozess

### **Mittel priorisiert**

5. **Notification** Entity
6. **MealPlan.Status** Property
7. **UploadTask** Entity für Status-Tracking
8. Favoriten-Logik klären (User vs. Group)

### **Niedrig priorisiert**

9. **AutomationRule** Entity
10. **RecipeSimilarity** Entity (oder On-the-fly)
11. Rezept-Versionierung

---

## 📝 Status-Übersicht

### ✅ Dokumentiert & Geklärt
- Bewertung-Aggregation (ReadModel)
- Nährwert-Klarstellung (pro Portion)
- Favoriten-Logik (pro User)
- Wochenplan-Beschränkung (7 Tage)
- Verteilungs-Logik (DB-Auswahl)
- Ähnlichkeitslogik (on-the-fly)
- Automatisierungen (Backend-Config)
- Rezept-Löschung (Hard Delete)
- User-Management & Gruppen
- Rezept-Bearbeitung

### ⚠️ Dokumentiert, aber Code noch nicht implementiert
1. **Recipe.Source** (Migration 1)
2. **Recipe.MealCategory** (Migration 1)
3. **Recipe.RepeatCycleWeeks** (Migration 1)
4. **MealPlan.Status** (Migration 2)
5. **RecipeDraft** Entity (Migration 3)
6. **Notification** Entity (Migration 4)
7. **UploadTask** Entity (Migration 5)
8. **RecipeReadModel.AverageRating** (ReadModel-Erweiterung)
9. **RecipeReadModel.FavoriteCount** (ReadModel-Erweiterung)

---

## 📝 Nächste Schritte

1. ✅ Diese Analyse-Dokumentation erstellt
2. ✅ Datenbank-Erweiterungen definieren (siehe `DATENBANK-Erweiterungen.md`)
3. ✅ Komponenten-Dokumentation aktualisieren
4. ✅ Klarstellungen erstellt (siehe `KLARSTELLUNGEN.md`)
5. ⏭️ **Migrations erstellen** (siehe `DATENBANK-Erweiterungen.md`)
6. ⏭️ **Entities erweitern/erstellen** (Recipe, MealPlan, RecipeDraft, Notification, UploadTask)
7. ⏭️ **ReadModels erweitern** (AverageRating, FavoriteCount)
