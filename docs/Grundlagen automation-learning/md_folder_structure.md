# md-folder-structure.md

## Zweck

Dieses Dokument definiert die **verbindliche Ordnerstruktur** des MD-Wissenssystems.

Es stellt sicher, dass:
- Inhalte nicht vermischt werden
- die Architektur langfristig stabil bleibt
- Cursor automatisch erkennen kann, **auf welcher Ebene** gearbeitet wird

Dieses Dokument ergänzt:
- `md-system-regeln.md`
- `md-architecture-overview.md`
- `CURSOR_AUTOMATION.md`

---

## Root-Struktur (verbindlich)

```text
/md-knowledge
│
├─ 00_system/
├─ 01_meta/
├─ 02_topics/
├─ 03_sessions/
├─ 04_analysis/
├─ 05_abstraction/
```

Die Nummerierung ist **Pflicht** und spiegelt die logische Pipeline wider.

---

## Ordner im Detail

### `00_system/` – System & Steuerung

**Inhalt:**
- `md-system-regeln.md`
- `md-architecture-overview.md`
- `md-architecture-visual.md`
- `CURSOR_RULES.md`
- `CURSOR_AUTOMATION.md`

**Regeln:**
- keine Sessions
- keine Inhalte
- seltene Änderungen

---

### `01_meta/` – Gesamtüberblick

**Inhalt:**
- `marc_overview.md`

**Regeln:**
- nur konsolidierte Informationen
- keine Details

---

### `02_topics/` – Thematisches Wissen

**Inhalt:**
- `topic_*.md`

**Regeln:**
- zeitlos
- neutral
- keine Einzelfälle

---

### `sessions/` – Rohdaten / Arbeit

**Inhalt:**
- `session_YYYY-MM-DD_thema.md`

**Regeln:**
- konkret
- zeitlich oder taskbezogen
- keine Bewertung
- Format: `session_YYYY-MM-DD_thema.md`

---

### `analysis/` – Vergleich & Analyse

**Inhalt:**
- `analysis_YYYY-MM-DD_thema.md`
- `__folder_summary.md` (im Root)

**Regeln:**
- vergleichend
- hypothesen erlaubt
- keine Schwächen oder Lösungen
- Format: `analysis_YYYY-MM-DD_thema.md`

---

### `schwaechen/` – Wiederkehrende Schwächen

**Inhalt:**
- `schwaechen_YYYY-MM-DD_thema.md`

**Regeln:**
- sehr langsam wachsend
- streng abgeleitet
- hohe Qualität
- Format: `schwaechen_YYYY-MM-DD_thema.md`
- Jede Schwäche als separate Datei

---

### `solutions/` – Konkrete Lösungen

**Inhalt:**
- `solution_YYYY-MM-DD_thema.md`

**Regeln:**
- handlungsorientiert
- überprüfbar
- direkt mit Schwächen verknüpft
- Format: `solution_YYYY-MM-DD_thema.md`
- Jede Lösung als separate Datei

---

### `rules/` – Verbindliche Regeln

**Inhalt:**
- `rule_YYYY-MM-DD_thema.md`

**Regeln:**
- dauerhaft
- handlungsleitend
- nicht diskussionsoffen
- Format: `rule_YYYY-MM-DD_thema.md`
- Jede Regel als separate Datei

---

## Erlaubter Informationsfluss

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
```

**Index-Dateien im Root:**
- `schwaechen.md` – Index und Strukturdefinition
- `loesungen.md` – Index und Strukturdefinition
- `regeln.md` – Index und Strukturdefinition

Rücksprünge sind **nicht erlaubt**.

---

## Harte Verbote

❌ Dateien ohne klaren Ordnerkontext  
❌ Mischen von Ebenen in einem Dokument  
❌ Duplizieren von Regeln oder Schwächen

---

## Rolle dieses Dokuments

Dieses Dokument ist:
- strukturelles Fundament
- Referenz für neue Inhalte
- Entscheidungsgrundlage bei Erweiterungen

Es wird selten geändert, aber konsequent angewendet.

