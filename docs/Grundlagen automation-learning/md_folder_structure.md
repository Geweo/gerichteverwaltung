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

### `03_sessions/` – Rohdaten / Arbeit

**Inhalt:**
- `session_*.md`

**Regeln:**
- konkret
- zeitlich oder taskbezogen
- keine Bewertung

---

### `04_analysis/` – Vergleich & Analyse

**Inhalt:**
- `__folder_summary.md`
- `analysis_*.md`

**Regeln:**
- vergleichend
- hypothesen erlaubt
- keine Schwächen oder Lösungen

---

### `05_abstraction/` – Dauerhafte Erkenntnisse

**Inhalt:**
- `schwaechen.md`
- `loesungen.md`
- `regeln.md`

**Regeln:**
- sehr langsam wachsend
- streng abgeleitet
- hohe Qualität

---

## Erlaubter Informationsfluss

```text
03_sessions/
   ↓
04_analysis/
   ↓
05_abstraction/
```

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

