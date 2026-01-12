# Ernährbär Style Guide

## Designphilosophie

Modern, frisch und einladend. Die App soll gesunde Ernährung ansprechend und motivierend präsentieren.

## Farbpalette

### Primärfarben

**Grün (Primary)**
- Frisch, gesund, natürlich
- Verwendet für: Hauptaktionen, Erfolgsmeldungen, gesunde Lebensmittel
- `hsl(142, 71%, 45%)` - Hauptfarbe
- `hsl(142, 71%, 35%)` - Hover/Dunkel
- `hsl(142, 71%, 55%)` - Hell

**Orange (Accent)**
- Energie, Appetit, Wärme
- Verwendet für: Highlights, Warnungen, wichtige Aktionen
- `hsl(25, 95%, 53%)` - Hauptfarbe
- `hsl(25, 95%, 43%)` - Hover/Dunkel
- `hsl(25, 95%, 63%)` - Hell

### Neutrale Farben

**Slate (Basis)**
- Moderne, neutrale Basis
- Verwendet für: Text, Hintergründe, Borders
- Folgt shadcn/ui Standard (slate)

**Weiß/Cremeweiß**
- Sauber, frisch
- `hsl(0, 0%, 100%)` - Reines Weiß
- `hsl(30, 20%, 98%)` - Warmes Weiß (Hintergründe)

### Semantische Farben

- **Erfolg**: Grün `hsl(142, 71%, 45%)`
- **Warnung**: Orange `hsl(25, 95%, 53%)`
- **Fehler**: Rot `hsl(0, 84%, 60%)`
- **Info**: Blau `hsl(217, 91%, 60%)`

## Typografie

### Schriftarten

- **Primary**: System Font Stack (Inter/SF Pro/Helvetica)
- **Monospace**: Für Code/Technische Daten (falls benötigt)

### Schriftgrößen

- **Display**: `3.5rem` (56px) - Hero Headlines
- **H1**: `2.5rem` (40px) - Hauptüberschriften
- **H2**: `2rem` (32px) - Sektionen
- **H3**: `1.5rem` (24px) - Unterüberschriften
- **Body Large**: `1.125rem` (18px) - Wichtiger Text
- **Body**: `1rem` (16px) - Standard Text
- **Small**: `0.875rem` (14px) - Sekundärer Text
- **Tiny**: `0.75rem` (12px) - Labels, Captions

### Schriftgewichte

- **Bold**: 700 - Überschriften, wichtige Betonungen
- **Semibold**: 600 - Unterüberschriften
- **Medium**: 500 - Labels, Buttons
- **Regular**: 400 - Body Text
- **Light**: 300 - Große Display-Texte

## Spacing & Layout

### Spacing Scale (8px Basis)

- `0.5` = 4px
- `1` = 8px
- `2` = 16px
- `3` = 24px
- `4` = 32px
- `6` = 48px
- `8` = 64px
- `12` = 96px
- `16` = 128px

### Border Radius

- **Small**: `0.25rem` (4px) - Kleine Elemente
- **Medium**: `0.5rem` (8px) - Standard (Buttons, Cards)
- **Large**: `1rem` (16px) - Große Cards
- **Full**: `9999px` - Pills, Badges

### Shadows

- **Small**: `0 1px 2px 0 rgb(0 0 0 / 0.05)`
- **Medium**: `0 4px 6px -1px rgb(0 0 0 / 0.1)`
- **Large**: `0 10px 15px -3px rgb(0 0 0 / 0.1)`

## Komponenten

### Buttons

**Primary (Grün)**
- Hintergrund: Primary Grün
- Text: Weiß
- Hover: Dunkleres Grün
- Verwendet für: Hauptaktionen (Registrieren, Speichern, etc.)

**Secondary (Orange)**
- Hintergrund: Transparent
- Border: Orange
- Text: Orange
- Hover: Orange Hintergrund
- Verwendet für: Sekundäre Aktionen

**Ghost**
- Hintergrund: Transparent
- Text: Slate
- Hover: Slate Hintergrund
- Verwendet für: Tertiäre Aktionen

### Cards

- Hintergrund: Weiß
- Border: `1px solid hsl(214.3 31.8% 91.4%)`
- Radius: `0.5rem` (8px)
- Shadow: Small
- Padding: `1.5rem` (24px)

### Input Fields

- Border: `1px solid hsl(214.3 31.8% 91.4%)`
- Focus: Primary Grün Border
- Radius: `0.5rem` (8px)
- Padding: `0.75rem 1rem`

### Badges/Tags

- Hintergrund: Leichtes Grün/Orange (je nach Typ)
- Text: Dunkles Grün/Orange
- Radius: `9999px` (Pill)
- Padding: `0.25rem 0.75rem`

## Icons & Illustrationen

- **Stil**: Outline, modern, minimalistisch
- **Größe**: 16px, 20px, 24px (Standard)
- **Bibliothek**: Lucide Icons (empfohlen für shadcn/ui)

## Animationen & Transitions

- **Standard**: `150ms ease-in-out`
- **Hover**: `200ms ease-in-out`
- **Focus**: `150ms ease-in-out`

## Responsive Breakpoints

- **Mobile**: < 640px
- **Tablet**: 640px - 1024px
- **Desktop**: > 1024px

## Accessibility

- **Kontrast**: Mindestens WCAG AA (4.5:1 für Text)
- **Focus States**: Sichtbar, Primary Grün
- **Touch Targets**: Mindestens 44x44px

## Beispiele

### Rezept Card
```tsx
<div className="bg-white rounded-lg border border-slate-200 p-6 shadow-sm hover:shadow-md transition-shadow">
  <h3 className="text-xl font-semibold text-slate-900 mb-2">Rezept Name</h3>
  <p className="text-slate-600 text-sm">Beschreibung...</p>
  <div className="mt-4 flex gap-2">
    <Badge className="bg-green-50 text-green-700">Gesund</Badge>
    <Badge className="bg-orange-50 text-orange-700">Schnell</Badge>
  </div>
</div>
```

### Primary Button
```tsx
<button className="bg-green-600 hover:bg-green-700 text-white font-medium px-4 py-2 rounded-lg transition-colors">
  Aktion
</button>
```

## Implementierung in shadcn/ui

Die Farben können in `src/index.css` als CSS-Variablen definiert werden:

```css
:root {
  --primary: 142 71% 45%;
  --primary-foreground: 0 0% 100%;
  --accent: 25 95% 53%;
  --accent-foreground: 0 0% 100%;
  /* ... weitere Variablen */
}
```
