# analysis_2026-01-24_build-patterns.md

## Zweck

Diese Analyse untersucht wiederkehrende Muster aus den Sessions, insbesondere im Kontext von Build-Fehlern und technischen Implementierungsproblemen.

**Basis:** `__folder_summary.md` (Stand: 2026-01-24)

---

## Identifizierte Muster

### Muster 1: Späte Erkennung von Struktur-Details

**Beobachtung:**  
Entity-Struktur-Details (z.B. `IngredientName` vs. `Name`, Enum-Namen, Property-Namen) werden erst während der Implementierung erkannt, nicht zu Beginn.

**Auftreten:**
- Session 2 (database-reset-fixtures): `IngredientName` vs. `Name` bei ShoppingListItem
- Session 3 (build-errors-fixes): Enum-Namen-Mehrdeutigkeiten (RecipeSource, DraftStatus)

**Auswirkung:**
- Mehrfache Iterationen nötig
- Build-Fehler, die vermeidbar wären
- Zeitverlust durch nachträgliche Korrekturen

**Neutralität:**  
Dies ist ein wiederkehrendes Muster über 2 Sessions hinweg.

---

### Muster 2: Namespace-Mehrdeutigkeiten durch parallele Strukturen

**Beobachtung:**  
Gleiche Namen existieren in Domain und Entities (z.B. `RecipeSource`, `DraftStatus`, `TaskStatus`), was zu Mehrdeutigkeiten führt.

**Auftreten:**
- Session 3 (build-errors-fixes): Mehrfache Namespace-Konflikte

**Auswirkung:**
- Compiler-Fehler
- Notwendigkeit von Aliases oder vollständigen Qualifizierungen
- Code wird weniger lesbar

**Neutralität:**  
Dies ist ein Design-Entscheidung (beide Strukturen existieren parallel), aber führt zu wiederkehrenden Problemen.

---

### Muster 3: Systematische Fehlerbehebung funktioniert

**Beobachtung:**  
Wenn Build-Fehler systematisch kategorisiert werden (LangVersion → Namespaces → using-Direktiven → Zugriffsprobleme), können sie effizient behoben werden.

**Auftreten:**
- Session 3 (build-errors-fixes): Erfolgreiche systematische Behebung

**Auswirkung:**
- Effiziente Problemlösung
- Weniger Iterationen
- Klare Struktur im Vorgehen

**Neutralität:**  
Dies ist ein positives Muster, das als Best Practice dokumentiert werden sollte.

---

### Muster 4: Moderne .NET Patterns erfordern spezielle Behandlung

**Beobachtung:**  
Top-Level Statements erzeugen interne Klassen, die für Tests nicht direkt zugänglich sind. Dies erfordert spezielle Patterns (`partial class`).

**Auftreten:**
- Session 3 (build-errors-fixes): Top-Level Statements Zugriffsproblem

**Auswirkung:**
- Initiale Verwirrung
- Notwendigkeit von speziellen Patterns
- Wissen muss dokumentiert werden

**Neutralität:**  
Dies ist ein technisches Muster, das bei Verwendung moderner .NET Features auftritt.

---

## Vergleich mit bestehenden Schwächen

**Bestehende Schwächen:**
- "Spätes Externalisieren von Struktur" (`schwaechen/schwaechen_2026-01-24_spaetes-externalisieren-von-struktur.md`)

**Vergleich:**
- Muster 1 (Späte Erkennung von Struktur-Details) ist **ähnlich**, aber spezifischer (fokussiert auf Entity-Strukturen, nicht allgemeine Struktur)
- Muster 1 könnte als **Unterkategorie** oder **neue Schwäche** betrachtet werden

---

## Empfehlungen

1. **Muster 1** sollte als Schwäche dokumentiert werden (wiederkehrend über 2 Sessions)
2. **Muster 2** sollte als Schwäche dokumentiert werden (Design-Entscheidung führt zu Problemen)
3. **Muster 3** sollte als Best Practice/Lösung dokumentiert werden
4. **Muster 4** sollte als technische Regel dokumentiert werden (wenn Top-Level Statements verwendet werden)

---

## Status

Analyse abgeschlossen.  
Bereit für Ableitung von Schwächen, Lösungen und Regeln.
