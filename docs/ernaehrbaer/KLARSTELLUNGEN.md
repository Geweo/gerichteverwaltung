# Klarstellungen – Ernährbär

**Datum:** 2026-01-24  
**Zweck:** Klarstellungen zu offenen Punkten aus der Analyse

---

## 1. Favoriten-Logik

**Entscheidung:**  
Favoriten sind **pro User**, nicht pro Group.

**Begründung:**
- `RecipeRating.IsFavorite` ist bereits pro User implementiert
- Jeder User hat eigene Favoriten-Liste
- Filter "Favorisiert" zeigt Rezepte, die vom aktuellen User favorisiert wurden

**UI-Implikation:**
- Favoriten-Button in Dashboard/Rezept-DB markiert Rezept als Favorit für aktuellen User
- Filter "Favorisiert" filtert nach `RecipeRatings.IsFavorite = true AND UserId = currentUser`

---

## 2. Bewertung – Aggregation

**Entscheidung:**  
Aggregierte Bewertung wird im **ReadModel** berechnet, nicht denormalisiert.

**Begründung:**
- `RecipeReadModel.AverageRating` wird aus `RecipeRatings` berechnet
- `RecipeReadModel.FavoriteCount` wird aus `RecipeRatings` berechnet
- Keine Denormalisierung nötig (Performance ausreichend)

**Implikation:**
- Query-Handler berechnet AverageRating und FavoriteCount beim Laden
- Keine zusätzliche DB-Spalte in `Recipes`-Tabelle

---

## 3. Nährwert – "Aggregiert"

**Klarstellung:**  
"Aggregiert" bedeutet **pro Portion** (pro Serving), nicht für das gesamte Rezept.

**Berechnung:**
- `NutritionInfo` enthält Nährwerte pro Portion
- Für Gesamt-Nährwerte: `NutritionInfo * Recipe.Servings`

**UI-Implikation:**
- Checkbox "Nährwerte anzeigen" zeigt Nährwerte pro Portion
- Optional: "Für gesamtes Rezept" → multipliziert mit Servings

---

## 4. Wochenplan – Maximal 7 Tage

**Entscheidung:**  
Aktuell auf 7 Tage beschränkt (Wochenplan), später flexibel erweiterbar.

**Begründung:**
- MVP-Fokus auf Wochenplanung
- `MealPlanEntry.DayNumber` (1-7) unterstützt Wochenpläne
- `MealPlan.StartDate/EndDate` ist flexibel für spätere Erweiterung

**Implikation:**
- Wizard validiert: EndDate - StartDate <= 7 Tage
- Später: `DayNumber` entfernen oder flexibel machen

---

## 5. Wochenplan-Wizard – Verteilung

**Entscheidung:**  
DB-Rezepte werden nach folgenden Kriterien ausgewählt:

1. **Zufällig** aus verfügbaren Rezepten
2. **Gewichtet nach Favoriten** (vom aktuellen User)
3. **Berücksichtigt Wiederholungszyklus** (RepeatCycleWeeks)
4. **Filtert nach Tags/Mahlzeit** (falls angegeben)

**Implikation:**
- Business-Logik in `GenerateMealPlanCommandHandler`
- Algorithmus: Zufällig + Gewichtung + Filter

---

## 6. Ähnlichkeitslogik

**Entscheidung:**  
Aktuell **on-the-fly** berechnet, später optional: `RecipeSimilarity` Entity für Caching.

**Berechnung:**
- Zutaten-Overlap: `RecipeIngredients.Name` Vergleich
- Tags: `RecipeTags` Overlap
- Zubereitungsart: `Instructions` Text-Ähnlichkeit (optional)

**Implikation:**
- Bei Review: Ähnlichkeits-Score berechnen
- Warnung bei Score > 70%
- Später: Background Task für Ähnlichkeits-Cache

---

## 7. Automatisierungen

**Entscheidung:**  
Aktuell **Backend-Konfiguration** (appsettings), später optional: `AutomationRule` Entity.

**Implikation:**
- Automatische Wochenplan-Erstellung: Konfigurierbar in appsettings
- Trigger: X Tage vor Ende (z.B. 2 Tage)
- Später: User kann eigene AutomationRules erstellen

---

## 8. Rezept-Löschung

**Entscheidung:**  
**Hard Delete** mit SetNull bei MealPlanEntries.

**Implikation:**
- Rezept wird aus DB gelöscht
- `MealPlanEntry.RecipeId` → null
- `MealPlanEntry.CustomMealName` bleibt (falls vorhanden)
- Cascade Delete: RecipeIngredients, RecipeTags, RecipeRatings, NutritionInfo

**UI:**
- Bestätigungs-Dialog mit Warnung
- Anzeige betroffener MealPlanEntries

---

## 9. MealPlan-Status

**Entscheidung:**  
MealPlan hat Status: Draft, Active, Archived.

**Implikation:**
- **Draft:** Wird erstellt, noch nicht aktiv
- **Active:** Aktuell in Verwendung (nur einer pro Group)
- **Archived:** Vergangener Plan

**Business-Logik:**
- Beim Aktivieren eines Plans: Andere Active-Pläne → Archived
- Oder: Mehrere Active-Pläne erlauben (später)

---

## 10. Review-Prozess

**Entscheidung:**  
`RecipeDraft` Entity für Review-Zwischenschritt.

**Workflow:**
1. Upload/KI-Generierung → `RecipeDraft` (Status: Pending)
2. Review-Dialog zeigt `RecipeDraft`
3. User bearbeitet/validiert
4. Entscheidung:
   - **Übernehmen** → `RecipeDraft.Status = Approved` → wird zu `Recipe`
   - **Anpassen** → `RecipeDraft` bleibt, kann weiter bearbeitet werden
   - **Verwerfen** → `RecipeDraft.Status = Rejected` → wird gelöscht

---

## 🔗 Referenzen

- `docs/ernaehrbaer/ANALYSE-Komponenten.md`
- `docs/ernaehrbaer/DATENBANK-Struktur.md`
- `docs/ernaehrbaer/DATENBANK-Erweiterungen.md`
