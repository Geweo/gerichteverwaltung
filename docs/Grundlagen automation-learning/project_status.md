# project_status.md

## Zweck

Aktueller Stand und nächste Schritte für Ernährbär.

**Aktualisierung:** Nach jeder Session.

---

## ✅ Abgeschlossen

- Datenbank-Reset-Skripte (PowerShell & Bash)
- Fixture-System für alle Tabellen
- Build-Fehler behoben, Projekt kompiliert
- Tests angepasst (verwenden Fixtures)

**Sessions:** [[session_2026-01-24_database-reset-fixtures]], [[session_2026-01-24_build-errors-fixes]]

---

## ⏭️ Nächste Schritte

### 1. Tests validieren
- [ ] Integration Tests ausführen
- [ ] Fixtures prüfen
- [ ] Ergebnisse dokumentieren

### 2. Frontend starten
**Basis:** [[../ernaehrbaer/ERNAEHRBAR-Components|Komponenten-Dokumentation]]

**Priorität 1: Core-Komponenten**
- [ ] Projekt-Setup (Framework wählen: React/Vue/etc.)
- [ ] Routing & Navigation
- [ ] API-Client generieren/konfigurieren
- [ ] Authentication/Authorization

**Priorität 2: Dashboard**
- [ ] Wochenplan-Tabelle (Montag–Sonntag, Morgens/Mittags/Abends)
- [ ] Filter (Favoriten, Mahlzeit, Tags)
- [ ] Detailansicht pro Mahlzeit

**Priorität 3: Rezept-Datenbank**
- [ ] Rezept-Liste/Tabelle
- [ ] Rezept-Detailansicht
- [ ] Rezept-Erstellung (Upload & KI)

**Priorität 4: Wochenplan-Verwaltung**
- [ ] Wochenplan-Erstellung (Wizard)
- [ ] Wochenplan-Bearbeitung
- [ ] Mahlzeit-Austausch

**Referenz:** [[../ernaehrbaer/README|Frontend-Dokumentation]]

---

## 🔄 In Arbeit

*(Aktuell nichts)*

---

## 📋 Offene Entscheidungen

- Frontend-Framework (React/Vue/Angular/etc.)
- UI-Framework (Material UI/Tailwind/etc.)
- State Management (Redux/Zustand/etc.)

---

## 📊 Metriken

**Letzte Aktualisierung:** 2026-01-24  
**Abgeschlossene Sessions:** 3  
**Offene Tasks:** ~15 (Tests + Frontend)

---

## 🔗 Links

- **Sessions:** [[sessions/]]
- **Frontend-Docs:** [[../ernaehrbaer/README]]
- **Architektur:** [[md_architecture_overview]]
