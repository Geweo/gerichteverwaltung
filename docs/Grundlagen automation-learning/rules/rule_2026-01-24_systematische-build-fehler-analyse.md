# rule_2026-01-24_systematische-build-fehler-analyse.md

## Abgeleitet aus Lösung

(Direkt aus Session-Reflexion validiert)

## Regel

Build-Fehler werden systematisch kategorisiert und in dieser Reihenfolge behoben: LangVersion → Namespaces → using-Direktiven → Zugriffsprobleme. Jeder Fehler wird einzeln behoben und getestet.

## Gilt für

- alle Build-Fehler
- Debugging-Prozesse

## Durchsetzung

- Systematisches Vorgehen wird dokumentiert
- Fehler werden kategorisiert, nicht ad-hoc behoben

## Referenz

- [[session_2026-01-24_build-errors-fixes]]
