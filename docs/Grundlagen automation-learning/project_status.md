# project_status.md

## Zweck

Aktueller Stand und nächste Schritte für Ernährbär.

**Aktualisierung:** Nach jeder Session.

---

## ✅ Abgeschlossen

- Datenbank-Reset-Skripte (PowerShell & Bash)
- Fixture-System für alle Tabellen (19/19 Tabellen)
- Build-Fehler behoben, Projekt kompiliert
- Tests angepasst (verwenden Fixtures)
- **Frontend: Rezept-Datenbank Feature-Modul** (Grundstruktur)
  - Feature-Modul `src/features/recipes/` erstellt
  - Komponenten: RecipeDatabase, RecipeTable, RecipeFilters, RecipeCreateDialog, RecipeDetailDialog
  - Types & Hooks definiert
  - Route `/recipes` aktualisiert
  - UI-Komponenten: Badge, Checkbox hinzugefügt

**Sessions:** 
- [[session_2026-01-24_database-reset-fixtures]]
- [[session_2026-01-24_build-errors-fixes]]
- [[session_2026-01-24_fixture-system-erklaerung]]

---

## ⏭️ Nächste Schritte

### 1. Tests validieren
- [ ] Integration Tests ausführen
- [ ] Fixtures prüfen
- [ ] Ergebnisse dokumentieren

### 2. Frontend (In Arbeit)

**✅ Abgeschlossen:**
- Projekt-Setup (React 19 + TypeScript + TanStack Router + TanStack Query)
- Routing & Navigation (TanStack Router)
- Feature-Struktur nach Zentreo-Architektur
- **Rezept-Datenbank Grundstruktur:**
  - ✅ Feature-Modul `src/features/recipes/`
  - ✅ Komponenten-Struktur (RecipeDatabase, RecipeTable, RecipeFilters, etc.)
  - ✅ TypeScript-Typen definiert
  - ✅ TanStack Query Hooks vorbereitet

**⏭️ Nächste Schritte für Rezept-Datenbank:**
- [ ] API-Client generieren (Orval) und konfigurieren
- [ ] `useRecipes` Hook mit echtem API-Call verbinden
- [ ] Inline-Editierung in RecipeTable implementieren
- [ ] Rezept-Bearbeitung (Detail-Dialog erweitern)
- [ ] Rezept-Löschung implementieren
- [ ] Tags-Filter erweitern (wenn Tag-API verfügbar)
- [ ] Upload-Funktion implementieren
- [ ] KI-Generierung implementieren

**Priorität 2: Dashboard**
- [ ] Wochenplan-Tabelle (Montag–Sonntag, Morgens/Mittags/Abends)
- [ ] Filter (Favoriten, Mahlzeit, Tags)
- [ ] Detailansicht pro Mahlzeit

**Priorität 3: Wochenplan-Verwaltung**
- [ ] Wochenplan-Erstellung (Wizard)
- [ ] Wochenplan-Bearbeitung
- [ ] Mahlzeit-Austausch

**Referenz:** [[../ernaehrbaer/README|Frontend-Dokumentation]]

---

## 🔄 In Arbeit

- **Frontend: Rezept-Datenbank** – Grundstruktur erstellt, API-Integration fehlt noch

---

## 📋 Offene Entscheidungen

- Frontend-Framework (React/Vue/Angular/etc.)
- UI-Framework (Material UI/Tailwind/etc.)
- State Management (Redux/Zustand/etc.)

---

## 📊 Metriken

**Letzte Aktualisierung:** 2026-01-24  
**Abgeschlossene Sessions:** 4  
**Offene Tasks:** ~12 (API-Integration + Frontend-Features)

---

## 🔗 Links

- **Sessions:** [[sessions/]]
- **Frontend-Docs:** [[../ernaehrbaer/README]]
- **Architektur:** [[md_architecture_overview]]
