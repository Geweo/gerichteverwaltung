# schwaechen.md

## Zweck

Dieses Dokument ist ein **Index** für alle dokumentierten Schwächen.

Die einzelnen Schwächen befinden sich im Ordner `schwaechen/` im Format `schwaechen_YYYY-MM-DD_thema.md`.

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

Siehe: `schwaechen/` Ordner

- [[schwaechen_2026-01-24_implizite-zieldefinition]]
- [[schwaechen_2026-01-24_vermischung-von-ebenen]]
- [[schwaechen_2026-01-24_spaetes-externalisieren-von-struktur]]
- [[schwaechen_2026-01-24_spaete-erkennung-entity-struktur-details]]
- [[schwaechen_2026-01-24_namespace-mehrdeutigkeiten-parallele-strukturen]]

---

## Archivierte / Verbesserte Schwächen

*(noch leer)*

---

## Verbindung zu Lösungen

- Jede aktive Schwäche **muss** mindestens eine Lösung in [[solutions/]] besitzen
- Statusänderungen werden dort ausgelöst, nicht hier

---

## Rolle dieses Dokuments

Dieses Dokument ist:
- **Index und Strukturdefinition**
- langfristig gültig
- abstrahiert
- bewusst knapp gehalten

Es dient als **Input für Lösungs- und Regeldefinitionen** und als **Übersicht** über alle Schwächen.
