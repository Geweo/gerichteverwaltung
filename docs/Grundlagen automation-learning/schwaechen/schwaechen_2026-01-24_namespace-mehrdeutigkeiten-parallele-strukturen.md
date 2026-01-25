# schwaechen_2026-01-24_namespace-mehrdeutigkeiten-parallele-strukturen.md

## Beschreibung

Gleiche Namen existieren in Domain und Entities (z.B. `RecipeSource`, `DraftStatus`), was zu Compiler-Mehrdeutigkeiten führt.

## Auswirkung

- Compiler-Fehler
- Notwendigkeit von Aliases oder vollständigen Qualifizierungen
- Code wird weniger lesbar
- Entwickler müssen explizit entscheiden, welche Version verwendet wird

## Beobachtungsbasis

- [[folder_summary]]: Session 3
- [[analysis_2026-01-24_build-patterns]]: Muster 2

## Status

- aktiv

## Verbindung zu Lösungen

- [[solution_2026-01-24_namespace-aliases-bei-mehrdeutigkeiten]]
