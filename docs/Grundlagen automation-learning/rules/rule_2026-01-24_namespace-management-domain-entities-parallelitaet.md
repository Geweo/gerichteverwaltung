# rule_2026-01-24_namespace-management-domain-entities-parallelitaet.md

## Abgeleitet aus Lösung

[[solution_2026-01-24_namespace-aliases-bei-mehrdeutigkeiten|Namespace-Aliases bei bekannten Mehrdeutigkeiten]]

## Regel

Bei bekannten Mehrdeutigkeiten zwischen Domain und Entities (z.B. `RecipeSource`, `DraftStatus`) werden sofort Namespace-Aliases verwendet. Entities-Versionen in Tests und Infrastructure, Domain-Versionen in Business-Logik.

## Gilt für

- Code, der mit Domain und Entities arbeitet
- Tests und Infrastructure-Layer

## Durchsetzung

- Compiler-Fehler bei Mehrdeutigkeiten
- Code-Review prüft Verwendung von Aliases

## Referenz

- [[solution_2026-01-24_namespace-aliases-bei-mehrdeutigkeiten]]
- [[session_2026-01-24_build-errors-fixes]]
