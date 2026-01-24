# schwaechen.md

## Zweck

Dieses Dokument sammelt **abstrakte, wiederkehrende Schwächen**,

die **ausschließlich** aus Analyse- und Summary-Dokumenten abgeleitet werden.

Es enthält **keine Einzelfälle**, keine Tagesmeinungen und keine spontanen Bewertungen.

---

## Grundregeln

- Schwächen entstehen **nie direkt** aus `session_*.md`
- Jede Schwäche basiert auf **mehreren Beobachtungen**
- Schwächen sind **neutral formuliert**
- Schwächen sind **Ansatzpunkte für Verbesserung**, keine Kritik

---

## Struktur einer Schwäche (Pflicht)

Jede Schwäche wird nach folgendem Schema dokumentiert:

```md
### <Kurztitel der Schwäche>

**Beschreibung**  
Neutrale Beschreibung des wiederkehrenden Musters.

**Auswirkung**  
Welche negativen Effekte entstehen dadurch?

**Beobachtungsbasis**  
Aus welchen Analysen / Summaries wurde dies abgeleitet?

**Status**  
- aktiv | verbessert | gelöst
```

---

## Aktive Schwächen

### Implizite Zieldefinition

**Beschreibung**  
Ziele, Nicht-Ziele und Constraints werden häufig vorausgesetzt,
aber nicht explizit benannt.

**Auswirkung**  
- Richtungswechsel
- unnötige Rückfragen
- inkonsistente Ergebnisse

**Beobachtungsbasis**  
- Mehrere Analyse- und Architektur-Diskussionen

**Status**  
- aktiv

---

### Vermischung von Ebenen

**Beschreibung**  
Input-, Analyse- und Abstraktionsebenen werden gedanklich zu früh vermischt.

**Auswirkung**  
- vorschnelle Verallgemeinerung
- unsaubere Ableitungen

**Beobachtungsbasis**  
- Architektur-Reflexionen

**Status**  
- aktiv

---

### Spätes Externalisieren von Struktur

**Beschreibung**  
Gedankliche Modelle werden lange intern optimiert,
bevor sie als MD festgehalten werden.

**Auswirkung**  
- unnötige Iterationen
- fehlende gemeinsame Referenz

**Beobachtungsbasis**  
- Meta-Reflexionen

**Status**  
- aktiv

---

## Archivierte / Verbesserte Schwächen

*(noch leer)*

---

## Verbindung zu Lösungen

- Jede aktive Schwäche **muss** mindestens eine Lösung in `loesungen.md` besitzen
- Statusänderungen werden dort ausgelöst, nicht hier

---

## Rolle dieses Dokuments

Dieses Dokument ist:
- langfristig gültig
- abstrahiert
- bewusst knapp gehalten

Es dient als **Input für Lösungs- und Regeldefinitionen**.

