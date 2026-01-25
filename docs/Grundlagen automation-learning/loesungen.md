# loesungen.md

## Zweck

Dieses Dokument ist ein **Index** für alle dokumentierten Lösungen.

Die einzelnen Lösungen befinden sich im Ordner `solutions/` im Format `solution_YYYY-MM-DD_thema.md`.

---

## Grundregeln

- Jede Lösung referenziert **genau eine Schwäche**
- Lösungen sind **konkret, überprüfbar und umsetzbar**
- Lösungen werden **nicht aus Sessions**, sondern aus `schwaechen/` abgeleitet
- Eine Schwäche kann mehrere Lösungen haben

---

## Struktur einer Lösung (Pflicht)

Jede Lösung wird nach folgendem Schema dokumentiert:

```md
### <Titel der Lösung>

**Bezieht sich auf Schwäche:** <exakter Titel aus schwaechen/>

**Ziel**  
Was soll sich durch diese Lösung konkret verbessern?

| Aspekt | Aktueller Zustand | Zielzustand | Lösung | Referenz |
|------|------------------|------------|--------|----------|
|      |                  |            |        |          |

**Erfolgskriterium**  
Woran erkennt man objektiv, dass die Lösung wirkt?

**Status**  
- geplant | aktiv | validiert
```

---

## Aktive Lösungen

Siehe: `solutions/` Ordner

- [[solution_2026-01-24_explizite-zieldefinition-erzwingen]]
- [[solution_2026-01-24_trennung-der-ebenen-erzwingen]]
- [[solution_2026-01-24_struktur-frueh-externalisieren]]
- [[solution_2026-01-24_entity-strukturen-vor-implementierung-pruefen]]
- [[solution_2026-01-24_namespace-aliases-bei-mehrdeutigkeiten]]

---

## Validierte Lösungen

*(noch leer)*

---

## Verbindung zu Regeln

- Validierte Lösungen sind **Kandidaten für [[rules/]]**
- Erst nach Validierung dürfen Lösungen zu Regeln werden

---

## Rolle dieses Dokuments

Dieses Dokument ist:
- **Index und Strukturdefinition**
- handlungsorientiert
- überprüfbar
- direkt mit Schwächen verknüpft

Es bildet die **Brücke zwischen Analyse und Steuerung** und dient als **Übersicht** über alle Lösungen.
