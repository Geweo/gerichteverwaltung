# Session: Frontend Rezept-Datenbank Grundstruktur

**Datum:** 2026-01-24  
**Thema:** Erstellung der Rezept-Datenbank Feature-Komponenten nach Zentreo-Architektur

---

## 🎯 Ziel

Erstellung der Grundstruktur für die Rezept-Datenbank-Komponenten im Frontend, basierend auf:
- Zentreo-Architektur (Feature-basiert, TanStack Router, TanStack Query)
- ERNAEHRBAR-Components.md Spezifikation

---

## ✅ Was wurde erstellt

### 1. Feature-Modul Struktur

```
src/features/recipes/
├── components/
│   ├── RecipeDatabase.tsx          # Hauptkomponente
│   ├── RecipeTable.tsx             # Editierbare Tabelle
│   ├── RecipeFilters.tsx           # Filter-Komponente
│   ├── RecipeCreateDialog.tsx      # Erstellungs-Dialog
│   ├── RecipeCreateForm.tsx        # Formular (manuell)
│   └── RecipeDetailDialog.tsx      # Detailansicht
├── hooks/
│   └── useRecipes.ts               # TanStack Query Hook
└── types.ts                        # TypeScript-Typen
```

### 2. Komponenten im Detail

**RecipeDatabase.tsx:**
- Hauptkomponente für die Rezept-Datenbank
- Integriert Filter, Tabelle und Create-Dialog
- Verwendet `useRecipes` Hook für Datenabfrage

**RecipeTable.tsx:**
- Editierbare Tabelle (laut Spezifikation)
- Zeigt: Gericht, Tags, Mahlzeit, Source, Nährwert, Bewertung, Wiederholungszyklus
- Klick auf Zeile öffnet Detail-Dialog
- TODO: Inline-Editierung implementieren

**RecipeFilters.tsx:**
- Filter nach Mahlzeit (Frühstück/Mittag/Abend)
- Filter nach Source (Manual/Generated/Upload)
- Checkbox für Favoriten
- TODO: Tags-Filter (wenn Tag-API verfügbar)

**RecipeCreateDialog.tsx:**
- Drei Modi: Upload, KI-Generierung, Manuell
- Basierend auf ERNAEHRBAR-Components.md
- TODO: Upload und KI-Generierung implementieren

**RecipeCreateForm.tsx:**
- Formular für manuelle Rezept-Erstellung
- Felder: Name, Beschreibung, Mahlzeit
- TODO: Zutaten, Zubereitung, Nährwerte hinzufügen
- TODO: API-Integration

**RecipeDetailDialog.tsx:**
- Detailansicht mit allen Rezept-Informationen
- Zeigt: Tags, Zutaten, Zubereitung, Nährwerte, Metadaten
- TODO: Bearbeitungs-Funktionalität hinzufügen

### 3. TypeScript-Typen

**types.ts:**
- `Recipe`, `RecipeIngredient`, `Tag`, `NutritionInfo`
- `RecipeFilters`, `RecipeCreateInput`
- `MealCategory`, `RecipeSource` Enums

### 4. Hooks

**useRecipes.ts:**
- TanStack Query Hook für Rezept-Abfrage
- Unterstützt Filter (mealCategory, tags, source, favorites)
- TODO: API-Client-Integration (aktuell Placeholder)

### 5. UI-Komponenten

**Hinzugefügt:**
- `badge.tsx` – shadcn/ui Badge-Komponente
- `checkbox.tsx` – shadcn/ui Checkbox-Komponente (Radix UI)

**Bereits vorhanden:**
- Button, Dialog, Table, Input, Textarea, Select, Label

### 6. Route

**routes/_authenticated/recipes.tsx:**
- Aktualisiert: Verwendet jetzt `RecipeDatabase` statt alte `Recipes` Komponente

---

## 🔧 Technische Details

### Architektur-Patterns

1. **Feature-basiert:** Komponenten in `src/features/recipes/`
2. **TanStack Query:** Server-State-Management
3. **shadcn/ui:** UI-Komponenten (basierend auf Radix UI)
4. **TypeScript:** Vollständig typisiert

### Abhängigkeiten

- ✅ `@radix-ui/react-checkbox` bereits installiert
- ✅ `class-variance-authority` für Badge-Varianten
- ✅ `lucide-react` für Icons

---

## ⚠️ Offene Punkte / TODOs

### API-Integration

1. **API-Client generieren:**
   - Orval konfigurieren
   - OpenAPI-Spec vom Backend generieren
   - TypeScript-Client generieren

2. **Hooks verbinden:**
   - `useRecipes` mit echtem API-Call
   - `RecipeCreateForm` mit POST-Endpoint
   - Mutations für Update/Delete

### Funktionalität

3. **Inline-Editierung:**
   - Zellen in RecipeTable editierbar machen
   - Optimistic Updates mit TanStack Query

4. **Rezept-Bearbeitung:**
   - RecipeDetailDialog erweitern
   - Edit-Modus hinzufügen
   - Formular für vollständige Bearbeitung

5. **Rezept-Löschung:**
   - Delete-Button in RecipeTable
   - Bestätigungs-Dialog
   - Cascade-Handling (MealPlanEntries)

6. **Upload-Funktion:**
   - File-Upload-Komponente
   - UploadTask-Status-Tracking
   - Progress-Indicator

7. **KI-Generierung:**
   - Formular für KI-Parameter
   - Asynchroner Generierungs-Flow
   - Notification bei Abschluss

8. **Tags-Filter:**
   - Tag-Auswahl-Komponente
   - Multi-Select für Tags
   - Tag-API-Integration

---

## 📊 Status

**Grundstruktur:** ✅ Vollständig  
**API-Integration:** ⏭️ Ausstehend  
**Funktionalität:** ⏭️ Teilweise (Create-Form, Detail-View vorhanden, aber ohne API)

---

## 🔗 Verknüpfungen

- [[../ernaehrbaer/ERNAEHRBAR-Components]] – Komponenten-Spezifikation
- [[session_2026-01-24_fixture-system-erklaerung]] – Fixture-System Erklärung
- [[project_status]] – Projekt-Status

---

## 📝 Nächste Schritte

1. **API-Client generieren** (Orval konfigurieren)
2. **API-Integration** in Hooks
3. **Inline-Editierung** implementieren
4. **Upload & KI-Generierung** implementieren
5. **Tests** schreiben (optional)
