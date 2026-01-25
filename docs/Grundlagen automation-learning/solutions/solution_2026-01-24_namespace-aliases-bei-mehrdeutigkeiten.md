# solution_2026-01-24_namespace-aliases-bei-mehrdeutigkeiten.md

## Bezieht sich auf Schwäche

[[schwaechen_2026-01-24_namespace-mehrdeutigkeiten-parallele-strukturen|Namespace-Mehrdeutigkeiten durch parallele Strukturen]]

## Ziel

Namespace-Konflikte werden durch präventive Verwendung von Aliases vermieden.

| Aspekt | Aktueller Zustand | Zielzustand | Lösung | Referenz |
|------|------------------|------------|--------|----------|
| Namespace-Konflikte | reaktiv (nach Fehler) | präventiv (von Anfang an) | Aliases verwenden, wenn Domain/Entities-Parallelität bekannt | [[session_2026-01-24_build-errors-fixes]] |
| Code-Lesbarkeit | vollständige Qualifizierung | klare Aliases | `using Entities = ...` Pattern | [[session_2026-01-24_build-errors-fixes]] |

## Erfolgskriterium

Keine Compiler-Fehler durch Namespace-Mehrdeutigkeiten mehr.

## Status

- aktiv

## Verbindung zu Regeln

- [[rule_2026-01-24_namespace-management-domain-entities-parallelitaet]]
