# regeln.md

## Zweck

Dieses Dokument ist ein **Index** für alle dokumentierten Regeln.

Die einzelnen Regeln befinden sich im Ordner `rules/` im Format `rule_YYYY-MM-DD_thema.md`.

---

## Grundregeln

- Eine Regel entsteht **nur** aus einer **validierten Lösung** in [[solutions/]]
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

Siehe: `rules/` Ordner

- [[rule_2026-01-24_explizite-zieldefinition-ist-pflicht]]
- [[rule_2026-01-24_ebenen-duerfen-nicht-vermischt-werden]]
- [[rule_2026-01-24_struktur-wird-frueh-externalisiert]]
- [[rule_2026-01-24_top-level-statements-pattern-fuer-tests]]
- [[rule_2026-01-24_namespace-management-domain-entities-parallelitaet]]
- [[rule_2026-01-24_package-referenzen-nur-explizit-was-benoeigt-wird]]
- [[rule_2026-01-24_systematische-build-fehler-analyse]]

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
- **Index und Strukturdefinition**
- das **Verhaltensgesetz** des Systems
- bindend für Mensch und Cursor
- Grundlage für Qualitätssicherung

Es wird **selten geändert**, aber konsequent angewendet und dient als **Übersicht** über alle Regeln.
