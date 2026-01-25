# md-architecture-overview.md

## Zweck

Dieses Dokument beschreibt die **Gesamtarchitektur des MD-Wissenssystems**.

Es beantwortet:
- welche MD-Typen existieren
- welche Verantwortung sie haben
- wie Informationen durch das System fließen

Dieses Dokument ist **architektonisch**, nicht operativ.

---

## Leitidee

> Wissen entsteht nicht durch einzelne gute Gedanken,
> sondern durch **strukturierte Wiederholung, Vergleich und Abstraktion**.

Die Architektur trennt bewusst:
- Beobachtung
- Analyse
- Bewertung
- Steuerung

---

## Ebenen der Architektur

### Ebene 1 – Input (Rohdaten)

**Zweck:** Erfassen dessen, was passiert ist

**MD-Typen:**
- `session_*.md` – konkrete Arbeits- oder Denk-Sessions
- `topic_*.md` – thematische Wissenssammlungen

**Regeln:**
- keine Bewertung
- keine Verallgemeinerung
- Subjektivität erlaubt

---

### Ebene 2 – Aggregation & Analyse

**Zweck:** Muster sichtbar machen

**MD-Typen:**
- `__folder_summary.md` – automatische Zusammenfassung mehrerer Sessions
- `analysis_*.md` – optionale vertiefte Analysen

**Regeln:**
- vergleichend, nicht wertend
- kein Ableiten von Lösungen

---

### Ebene 3 – Abstraktion

**Zweck:** Dauerhafte Erkenntnisse erzeugen

**MD-Typen:**
- `schwaechen.md` (Index) → `schwaechen/` – wiederkehrende Muster / Probleme
- `loesungen.md` (Index) → `solutions/` – systematische Verbesserungen
- `regeln.md` (Index) → `rules/` – abgeleitete Verhaltensregeln

**Regeln:**
- zeitlos
- wiederverwendbar
- nur aus Analyse-Ebene ableitbar

---

### Ebene 4 – Steuerung & Meta

**Zweck:** Überblick, Qualitätssicherung, Fokus

**MD-Typen:**
- `CURSOR_RULES.md`
- `marc_overview.md`

**Regeln:**
- beeinflusst zukünftige Arbeit
- wird selten geändert

---

## Informationsfluss (Pipeline)

```text
sessions/session_*.md
   ↓
folder_summary.md
   ↓
analysis/analysis_*.md
   ↓
schwaechen/schwaechen_*.md
   ↓
solutions/solution_*.md
   ↓
rules/rule_*.md
   ↓
bessere Sessions
```

Dieser Fluss ist **gerichtet** und darf nicht übersprungen werden.

---

## Abhängigkeitsregeln (hart)

- Eine Schwäche benötigt ≥ 2 unabhängige Beobachtungen
- Eine Lösung benötigt eine klar definierte Schwäche
- Eine Regel benötigt eine validierte Lösung
- Kein Dokument darf Informationen „nach oben ziehen"

---

## Qualitätsziele der Architektur

Die Architektur soll:
- Kontext sauber aufteilen
- Wiederholungen sichtbar machen
- Lernen beschleunigen
- Selbstkritik entemotionalisieren

---

## Rolle dieses Dokuments

Dieses Dokument ist:
- Referenz für neue MD-Typen
- Entscheidungsgrundlage bei Erweiterungen
- Architekturvertrag zwischen Mensch & System

Es wird **selten geändert**, aber häufig konsultiert.

