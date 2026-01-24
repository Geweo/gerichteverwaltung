# CURSOR_RULES.md

## Zweck

Dieses Dokument definiert **verbindliche Regeln** für die Zusammenarbeit zwischen **Marc** und **Cursor**.

Es ist ein **Operating Model**, keine Dokumentation.
Cursor hat diese Datei **vor jeder relevanten Aktion** zu lesen und einzuhalten.

---

## Grundprinzipien

1. **Explizit vor Implizit** – Ziele, Nicht-Ziele und Constraints müssen klar benannt sein.
2. **Qualität vor Geschwindigkeit** – lieber nachfragen als falsch ändern.
3. **Reflexion ist Pflicht** – jede Arbeit erzeugt Lernartefakte.
4. **Trennung der Ebenen** – Input, Analyse, Abstraktion und Steuerung werden nicht vermischt.
5. **MDs steuern Verhalten** – Code folgt den MDs, nicht umgekehrt.

---

## Pflicht: Zu lesende MDs (Reihenfolge)

Vor **jeder** inhaltlichen oder technischen Aktion muss Cursor prüfen:

1. `md-system-regeln.md`
2. `md-architecture-overview.md`
3. `md-architecture-visual.md`
4. `CURSOR_RULES.md`

Wenn eine relevante MD fehlt oder widerspricht:
- **keine Änderungen durchführen**
- **Rückfrage stellen**

---

## Erlaubte & verbotene Aktionen

### Cursor DARF:
- Vorschläge machen
- Alternativen vergleichen
- Risiken benennen
- Strukturen entwerfen
- Reflexionen formulieren

### Cursor DARF NICHT:
- implizite Annahmen treffen
- Ziele erraten
- mehrere Lösungswege ohne Bewertung liefern
- Code ändern ohne expliziten Auftrag

---

## Änderungs-Gate (hart)

Cursor darf **Code oder Struktur nur ändern**, wenn:

- das Ziel explizit formuliert ist
- die betroffene Ebene klar ist
- die Änderung einer bestehenden Regel nicht widerspricht

Andernfalls gilt:
> **STOP → Rückfrage → Dokumentation der Unklarheit**

---

## Session-Pflicht

Jede relevante Zusammenarbeit erzeugt **genau eine** Session-Datei:

```
session_YYYY-MM-DD_<topic>.md
```

Diese Datei wird:
- nicht bewertet
- nicht abstrahiert
- nicht optimiert

Sie dient **ausschließlich** als Input für spätere Analyse.

---

## Analyse- & Abstraktionsregeln

- Schwächen entstehen **nie direkt** aus Sessions
- Schwächen entstehen **nur** aus `__folder_summary.md`
- Lösungen entstehen **nur** aus Schwächen
- Regeln entstehen **nur** aus validierten Lösungen

---

## Pflicht: Lösungs-Tabellen

Jede Lösung **muss** in Tabellenform dokumentiert werden:

```md
| Aspekt | Aktueller Zustand | Zielzustand | Lösung | Referenz |
|------|------------------|------------|--------|----------|
```

### Referenz-Regeln
- relative Pfade
- bei Code: Datei + Zeilennummer
- bei Struktur: MD-Datei

---

## Pflicht-Reflexionsfragen

Nach jeder Session beantwortet Cursor explizit:

1. Was war unklar?
2. Wo gab es implizite Annahmen?
3. Welche Information kam zu spät?
4. Was hätte man systemisch besser machen können?

Diese Antworten fließen **nicht direkt** in `schwaechen.md`.

---

## Eskalationsregel

Tritt dasselbe Muster **≥ 3×** auf:
- markieren
- priorisieren
- in `marc_overview.md` aufnehmen

---

## Meta-Regel (höchste Priorität)

> **Wenn etwas unklar ist, ist nicht der Code das Problem – sondern die Kommunikation.**

Diese Unklarheit ist immer zu dokumentieren.

---

## Gültigkeit

Diese Regeln gelten:
- für alle Themen
- für alle Technologien
- für jede Zusammenarbeit mit Cursor

Abweichungen sind **explizit zu begründen**.
