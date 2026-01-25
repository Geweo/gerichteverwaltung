# Ernährbär – Komponenten & Datenbank-Dokumentation

**Stand:** 2026-01-24

---

## 📚 Dokumente

### 1. [ERNAEHRBAR-Components.md](./ERNAEHRBAR-Components.md)
**Zweck:** Architektur & Komponenten-Übersicht  
**Inhalt:**
- Dashboard
- Rezept-/Gerichtedatenbank
- Rezept-Erstellung (Upload & KI)
- Wochenplanverwaltung
- Wochenplan-Erstellung (Wizard)
- Notifications
- Hintergrundverarbeitung
- Review & Validierung

---

### 2. [ANALYSE-Komponenten.md](./ANALYSE-Komponenten.md)
**Zweck:** Analyse der Komponenten-Dokumentation  
**Inhalt:**
- ✅ Was ist gut definiert
- ⚠️ Offene Punkte & logische Fehler (12 identifiziert)
- 🔍 Fehlende Aspekte (5 identifiziert)
- 📊 Datenbankstruktur-Analyse
- 🎯 Empfehlungen mit Prioritäten

**Wichtigste Erkenntnisse:**
- Recipe.Source fehlt (Generated, Upload, Manual)
- Recipe.MealCategory fehlt
- Recipe.RepeatCycleWeeks fehlt
- RecipeDrafts für Review-Prozess fehlt
- Notifications-DB-Struktur fehlt

---

### 3. [DATENBANK-Struktur.md](./DATENBANK-Struktur.md)
**Zweck:** Vollständige Übersicht über die Datenbankstruktur  
**Inhalt:**
- 📊 Tabellenübersicht (alle 12 Tabellen)
- 🔄 Beziehungen (ER-Diagramm)
- 📋 Fehlende Tabellen (5 identifiziert)
- 🎯 Empfohlene Erweiterungen

**Tabellen:**
- Multi-Tenant: Groups, Users, GroupMembers, GroupInvites
- Rezepte: Recipes, RecipeIngredients, Tags, RecipeTags, RecipeRatings, NutritionInfos
- Wochenpläne: MealPlans, MealPlanEntries

---

### 4. [DATENBANK-UML-Diagramm.md](./DATENBANK-UML-Diagramm.md)
**Zweck:** Vollständiges UML-Klassendiagramm mit allen Beziehungen  
**Inhalt:**
- Mermaid-Diagramm mit allen Tabellen
- 1:1, 1:N, N:M Beziehungen visualisiert
- Neue Tabellen: `Files`, `ShoppingLists`, `ShoppingListItems`
- Detaillierte Spezifikationen für neue Tabellen

---

### 5. [DATENBANK-Erweiterungen.md](./DATENBANK-Erweiterungen.md)
**Zweck:** Konkrete Implementierungsempfehlungen  
**Inhalt:**
- 🎯 Priorität 1: Recipe-Erweiterungen (Source, MealCategory, RepeatCycleWeeks)
- 🎯 Priorität 2: MealPlan-Status
- 🎯 Priorität 3: RecipeDrafts für Review-Prozess
- 🎯 Priorität 4: Notifications
- 🎯 Priorität 5: UploadTasks
- 📋 Migrations-Reihenfolge

**Code-Beispiele:**
- Enum-Definitionen
- Entity-Erweiterungen
- Migration-Befehle

---

### 6. [KLARSTELLUNGEN.md](./KLARSTELLUNGEN.md)
**Zweck:** Entscheidungen zu offenen Punkten  
**Inhalt:**
- Favoriten-Logik (pro User)
- Bewertung-Aggregation (ReadModel)
- Nährwert-Klarstellung (pro Portion)
- Wochenplan-Beschränkung (7 Tage)
- Verteilungs-Logik (DB-Auswahl)
- Ähnlichkeitslogik (on-the-fly)
- Automatisierungen (Backend-Config)
- Rezept-Löschung (Hard Delete)
- MealPlan-Status (Draft, Active, Archived)
- Review-Prozess (RecipeDraft)

---

## 🎯 Nächste Schritte

1. ✅ **Analyse abgeschlossen** – Offene Punkte identifiziert
2. ⏭️ **Migrations erstellen** – Gemäß `DATENBANK-Erweiterungen.md`
3. ⏭️ **Komponenten-Dokumentation aktualisieren** – Basierend auf Analyse
4. ⏭️ **Backend-Implementierung** – Fehlende Features implementieren

---

## 🔗 Verwandte Dokumente

- `docs/FINAL_STATUS.md` – Aktueller Implementierungsstand
- `docs/ARCHITECTURE.md` – Architektur-Überblick
- `docs/API_CLIENTS_GENERATED.md` – API-Client-Dokumentation

---

## 📝 Änderungshistorie

- **2026-01-24:** Initiale Analyse und Dokumentation erstellt
