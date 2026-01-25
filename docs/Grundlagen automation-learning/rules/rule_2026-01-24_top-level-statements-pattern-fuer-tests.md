# rule_2026-01-24_top-level-statements-pattern-fuer-tests.md

## Abgeleitet aus Lösung

(Direkt aus Session-Reflexion validiert)

## Regel

Wenn Top-Level Statements in `Program.cs` verwendet werden, muss am Ende der Datei `public partial class Program { }` im globalen Namespace hinzugefügt werden. Tests verwenden `global::Program` für expliziten Zugriff.

## Gilt für

- ASP.NET Core Projekte mit Top-Level Statements
- Test-Projekte, die auf `Program` zugreifen müssen

## Durchsetzung

- Build-Fehler bei fehlender `partial class`
- Code-Review prüft Pattern

## Referenz

- [[session_2026-01-24_build-errors-fixes]]
