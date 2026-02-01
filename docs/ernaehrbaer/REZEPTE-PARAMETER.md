# 🍽️ Meal Planning System – Architektur & Komponenten

## Ziel des Systems

Ziel ist der Aufbau eines modularen, erweiterbaren Meal-Planning-Systems, das folgende Kernprobleme löst:

- Strukturierte Wochenplanung (morgens / mittags / abends)
- Zentrale, editierbare Rezept- & Gerichtedatenbank
- KI-gestützte Generierung neuer Gerichte
- Wiederverwendung & Variation statt Wiederholung
- Automatisierung (Wochenplan-Vorschläge, Generierung, Notifications)
- Transparente Kontrolle für den Nutzer (Review, Austausch, Favoriten)

Das System ist **zustandsgetrieben**, **asynchron** (Background Tasks) und **UI-geführt**, mit klaren Trennungen zwischen:
- Planung
- Rezeptverwaltung
- Generierung
- Review
- Einkauf / Weiterverarbeitung (später)

---

## 🧩 Komponentenübersicht

- Dashboard
- Rezept- / Gerichtedatenbank
- Rezept-Erstellung (Upload & KI)
- Wochenplanverwaltung
- Wochenplan-Erstellung (Wizard)
- Notifications
- Hintergrundverarbeitung
- Review & Validierung

---

## 🧭 Dashboard

### Zweck
Zentraler Einstiegspunkt mit Überblick über:
- Aktuellen Wochenplan
- Schnelle Filter & Favoriten
- Direkter Zugriff auf Details

### Hauptkomponenten
- **Wochenplan-Tabelle**
  - Zeilen: Montag – Sonntag
  - Spalten: Morgens | Mittags | Abends
  - Jede Zelle:
    - Gericht
    - Favoriten-Button
    - Klick → Detailansicht / Austausch

- **Filter**
  - Favorisiert
  - Mahlzeit (Frühstück / Mittag / Abend)
  - Tags

- **Detailbereich (erweitert)**
  - Pro Mahlzeit:
    - Zutaten
    - Zubereitung
    - Dauer
    - Checkbox: Nährwerte anzeigen
    - Favorisieren

### Design-Alternativen
- Split View (Tabelle links, Detail rechts)
- Collapse / Accordion pro Tageszeit
- Mobile: Tageskarten statt Tabelle

---

## 📚 Rezept- / Gerichtedatenbank

### Zweck
Zentrale Verwaltung aller Gerichte (manuell + KI)

### Hauptansicht
**Editierbare Tabelle**

| Spalte | Beschreibung |
|------|-------------|
| Gericht | Name |
| Tags | frei editierbar |
| Gerichttyp | Frühstück / Mittag / Abend |
| Type | generiert / upload |
| Nährwert | aggregiert |
| Bewertung | manuell |
| Wiederholungszyklus | z. B. alle X Wochen |
| Aktionen | Edit / Delete |

Jede Zelle ist **Inline-editierbar**.

### Filter
- Mahlzeit
- Tags
- Gerichttyp
- Type (KI / Upload)

### Plus-Button (oben rechts)
→ Öffnet **Rezept-Erstellungsdialog**

---

## ➕ Rezept-Erstellung

### Auswahl-Dialog
Optionen:
- 📄 File Upload (PDF / PNG / mehrere)
- 🤖 KI-Gericht generieren

---

### 🧠 KI-Gericht generieren – Maske

#### Parameter (Dropdowns / Buttons)
- Mahlzeit: Frühstück | Mittag | Abend
- Ernährungsform: vegetarisch | vegan | Fleisch
- Stil: gesund | fettig | Fitness | Low Carb | eiweißreich
- Aufwand:
  - schnell
  - kurze Vorbereitungszeit
  - wiederverwendbare Zutaten
- Tags (frei)
- Spracheingabe (Speech-to-Prompt)

#### Intelligente Rückfragen
Beispiel:
> „Italienisches Gericht“
→ Rückfrage:
- vegetarisch oder Fleisch?
- Frühstück / Mittag / Abend?

---

### 📄 Upload-Flow

- Nutzer lädt Dateien hoch
- Background Task startet (Parsing / Extraktion)
- System blockiert nicht
- Nach Abschluss:
  - Notification
  - Gericht erscheint im **Review-Zwischenschritt**

---

## 🧐 Review & Validierung

### Zweck
Qualitätssicherung vor Aufnahme in die DB

- Anzeige extrahierter Daten
- Editierbar
- Warnung bei ähnlichen Gerichten
- Entscheidung:
  - ✔ Übernehmen
  - ✏ Anpassen
  - ❌ Verwerfen

---

## 🗓️ Wochenplanverwaltung

### Ansicht
- Kalender (Standard: Woche)
- Navigation:
  - ← vorherige Woche
  - → nächste Woche
- Umschaltbar:
  - Woche
  - Monat
  - (später: Jahr)

### Interaktion
- Klick auf Gericht → Austausch
- Austausch-Optionen:
  - aus DB wählen
  - zufällig
  - neu generieren (KI-Maske)

---

## ➕ Wochenplan erstellen (Wizard)

### Schritt 1 – Zeitraum
- Laufzeit des aktuellen Plans
- Neuer Zeitraum (prefilled: nächste Woche)
- Maximal: 7 Tage

---

### Schritt 2 – Mahlzeiten auswählen
Buttons:
- Frühstück
- Mittag
- Abend

Option:
- Für alle Tage?
  - Ja → fertig
  - Nein → Auswahlmatrix (Mo–So × Mahlzeiten)

---

### Schritt 3 – Rezeptquelle
Buttons:
- Nur aus DB
- Nur neue Gerichte
- Beides

Falls Beides:
- Verteilung:
  - ⅓ neu / ⅔ DB
  - 50 / 50
  - ⅔ neu / ⅓ DB
- Stil:
  - ähnlich
  - neue Ideen

---

### Schritt 4 – Generierungsparameter
- Gleiche Maske wie Rezept-Erstellung
- Optional abgespeckt

---

### Ergebnis
- Wochenplan wird **asynchron** erstellt
- Notification
- Vorschau
- Manuelles Nachjustieren pro Tag / Mahlzeit

---

## 🔔 Notifications

- Upload abgeschlossen
- KI-Generierung abgeschlossen
- Neuer Wochenplan verfügbar
- Warnung bei ähnlichen Rezepten

UI:
- Bell Icon in Menüleiste
- Badge Counter

---

## ⚙️ Hintergrundverarbeitung

- File Parsing
- KI-Generierung
- Ähnlichkeitsanalyse
- Wochenplan-Automatisierung

Nicht-blockierend, Status-basiert.

---

## 🔁 Automatisierungen

- Automatische Wochenplan-Erstellung
  - Trigger: X Tage vor Ende
  - Parameter:
    - bevorzugte Mahlzeiten
    - Variation
    - Favoritengewichtung

---

## 🧠 Ähnlichkeitslogik

- Zutaten-Overlap
- Tags
- Zubereitungsart
- Warnung statt Blockade

---

## 📐 UML – Systemübersicht

```mermaid
graph TD
    Dashboard --> Wochenplan
    Dashboard --> RezeptDB

    RezeptDB --> Rezept
    Rezept --> Review
    Review --> RezeptDB

    RezeptDB --> Wochenplan
    Wochenplan --> WochenplanWizard
    WochenplanWizard --> RezeptDB
    WochenplanWizard --> KIService

    KIService --> BackgroundTasks
    Upload --> BackgroundTasks
    BackgroundTasks --> Notifications

    Notifications --> Dashboard
