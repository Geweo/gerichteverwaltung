# regeln.md

## Zweck

Dieses Dokument enthält **verbindliche Regeln**, die **ausschließlich aus validierten Lösungen** abgeleitet wurden.

Regeln sind **dauerhaft**, **handlungsleitend** und **nicht diskussionsoffen** im Tagesgeschäft.

---

## Grundregeln

- Eine Regel entsteht **nur** aus einer **validierten Lösung** in `loesungen.md`
- Regeln sind **knapp, eindeutig und überprüfbar**
- Regeln beschreiben **Verhalten**, nicht Theorie
- Regeln gelten **systemweit**

---

## Struktur einer Regel (Pflicht)

Jede Regel wird nach folgendem Schema dokumentiert:

```md
### <Regelname>

**Abgeleitet aus Lösung:** <Titel der Lösung>

**Regel**  
Kurze, eindeutige Handlungsanweisung.

**Gilt für**  
- Themen / Ebenen / Tools

**Durchsetzung**  
Wie wird die Regel eingehalten bzw. überprüft?

**Referenz**  
Link zur Lösung / Architektur / Rules
```

---

## Aktive Regeln

### Explizite Zieldefinition ist Pflicht

**Abgeleitet aus Lösung:** Explizite Zieldefinition erzwingen

**Regel**  
Jede relevante Arbeit beginnt mit einer expliziten Definition von:
- Ziel
- Nicht-Ziel
- Constraints

**Gilt für**  
- alle `session_*.md`
- alle Themen

**Durchsetzung**  
- Fehlt eine Zieldefinition → STOP und Rückfrage

**Referenz**  
- `loesungen.md`
- `md-system-regeln.md`

---

### Ebenen dürfen nicht vermischt werden

**Abgeleitet aus Lösung:** Trennung der Ebenen erzwingen

**Regel**  
Input-, Analyse- und Abstraktionsebenen dürfen nicht in einem Dokument vermischt werden.

**Gilt für**  
- alle MDs

**Durchsetzung**  
- Inhalte werden verschoben oder gestoppt

**Referenz**  
- `md-architecture-overview.md`

---

### Struktur wird früh externalisiert

**Abgeleitet aus Lösung:** Struktur früh externalisieren

**Regel**  
Neue Themen starten immer mit einer leeren Struktur-MD,
bevor Detailarbeit beginnt.

**Gilt für**  
- neue Themen
- neue größere Tasks

**Durchsetzung**  
- kein Detail ohne Grundstruktur

**Referenz**  
- `loesungen.md`

---

## Inaktive / zurückgestellte Regeln

*(noch leer)*

---

## Änderung von Regeln

- Regeln werden **nicht spontan geändert**
- Änderungen erfolgen nur durch:
  - neue validierte Lösungen
  - bewusste Architekturentscheidung

---

## Rolle dieses Dokuments

Dieses Dokument ist:
- das **Verhaltensgesetz** des Systems
- bindend für Mensch und Cursor
- Grundlage für Qualitätssicherung

Es wird **selten geändert**, aber konsequent angewendet.

