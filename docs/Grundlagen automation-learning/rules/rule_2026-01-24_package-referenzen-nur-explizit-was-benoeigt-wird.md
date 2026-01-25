# rule_2026-01-24_package-referenzen-nur-explizit-was-benoeigt-wird.md

## Abgeleitet aus Lösung

(Direkt aus Session-Reflexion validiert)

## Regel

Nur explizit referenzieren, was wirklich benötigt wird. Transitive Dependencies werden nicht explizit hinzugefügt, um Versionskonflikte zu vermeiden.

## Gilt für

- alle `.csproj` Dateien
- Package-Management

## Durchsetzung

- Build-Warnungen bei redundanten Referenzen
- Code-Review prüft Package-Referenzen

## Referenz

- [[session_2026-01-24_build-errors-fixes]]
