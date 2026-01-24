---
trigger: always_on
---

# Project Rules - Ernährbär

## Zweck

Dieses Dokument verweist auf das **verbindliche Regelwerk** für die Zusammenarbeit zwischen Entwickler und AI-Agenten (Cursor, Claude, etc.).

## Pflicht: Zu lesende Dokumente (Reihenfolge)

Vor **jeder** inhaltlichen oder technischen Aktion muss der Agent prüfen:

1. `docs/Grundlagen automation-learning/md_system_regeln.md` - Grundlegende Systemregeln
2. `docs/Grundlagen automation-learning/md_architecture_overview.md` - Architektur-Überblick
3. `docs/Grundlagen automation-learning/md_folder_structure.md` - Ordnerstruktur
4. `docs/Grundlagen automation-learning/cursor_rules.md` - Cursor-spezifische Regeln
5. `docs/Grundlagen automation-learning/cursor_automation.md` - Automatisierungslogik
6. `docs/Grundlagen automation-learning/regeln.md` - Aktive Regeln

## Rules for Sub-Projects

### Client
`./client/.agent/rules/client-project-rules.md`

### Server
- `./server/.agent/rules/setup.md`
- `./server/.agent/rules/testing-quality.md`
- `./server/.agent/rules/architecture.md`
- `./server/.agent/rules/coding-style.md`
- `./server/.agent/rules/implementation.md`
- `./server/.agent/rules/implementation_extended.md`

## Wichtige Grundsätze

1. **Explizit vor Implizit** – Ziele, Nicht-Ziele und Constraints müssen klar benannt sein
2. **Qualität vor Geschwindigkeit** – lieber nachfragen als falsch ändern
3. **Reflexion ist Pflicht** – jede Arbeit erzeugt Lernartefakte
4. **Trennung der Ebenen** – Input, Analyse, Abstraktion und Steuerung werden nicht vermischt
5. **MDs steuern Verhalten** – Code folgt den MDs, nicht umgekehrt

## Änderungs-Gate (hart)

Agent darf **Code oder Struktur nur ändern**, wenn:
- das Ziel explizit formuliert ist
- die betroffene Ebene klar ist
- die Änderung einer bestehenden Regel nicht widerspricht

Andernfalls gilt:
> **STOP → Rückfrage → Dokumentation der Unklarheit**

## Meta-Regel (höchste Priorität)

> **Wenn etwas unklar ist, ist nicht der Code das Problem – sondern die Kommunikation.**

Diese Unklarheit ist immer zu dokumentieren.
