# 🗺️ Entwicklungs-Roadmap – Ernährbär

Diese Roadmap teilt die Spezifikationen in konkrete, umsetzbare Aufgaben auf und priorisiert sie nach MVP und späteren Phasen.

---

## ✅ Bereits implementiert

- [x] Basis-Architektur (Hexagonale Architektur, Frontend/Backend Setup)
- [x] Authentication (Supabase JWT)
- [x] LLM-Integration (Ollama für lokale Entwicklung)
- [x] Rezept-Generierung via LLM (mit Prompt-Validierung)
- [x] Tag-Extraktion aus Prompts
- [x] CORS-Konfiguration
- [x] Basis-Datenbank-Setup (PostgreSQL, EF Core)

---

## 🎯 Phase 1: MVP (Minimal Viable Product)

**Ziel:** Funktionale Basis-Version mit Kern-Features

### Backend

#### 1.1 Rezept-Datenmodell & Persistierung
- [ ] **Entity: Recipe** (Name, Beschreibung, Zutaten, Tags, Kochprozess, Bild-URL)
- [ ] **Entity: Ingredient** (Name, Menge, Einheit)
- [ ] **Entity: MealPlan** (Startdatum, Enddatum, User-Referenz)
- [ ] **Entity: MealPlanEntry** (Datum, MealCategory, Recipe-Referenz)
- [ ] **Entity: Tag** (Name, Kategorie: Zubereitung/Ernährung/Zutat)
- [ ] **Migration:** Initial Schema für Rezepte & Wochenplan
- [ ] **Port:** `IRecipeStorage` erweitern (CRUD-Operationen)
- [ ] **UseCase:** `UploadRecipe` implementieren (Speichern von generierten/hochgeladenen Rezepten)
- [ ] **Controller:** `POST /api/recipes` (Rezept speichern)
- [ ] **Controller:** `GET /api/recipes` (Rezepte auflisten)
- [ ] **Controller:** `PUT /api/recipes/{id}` (Rezept bearbeiten)
- [ ] **Controller:** `DELETE /api/recipes/{id}` (Rezept löschen)

#### 1.2 Wochenplan-Persistierung
- [ ] **UseCase:** `SaveMealPlan` (Wochenplan in DB speichern)
- [ ] **Controller:** `POST /api/meal-plans` (Wochenplan speichern)
- [ ] **Controller:** `GET /api/meal-plans/current` (Aktueller Wochenplan)
- [ ] **Controller:** `GET /api/meal-plans/{id}` (Wochenplan abrufen)

#### 1.3 Rezept-Generierung erweitern
- [ ] **UseCase:** Generierte Rezepte automatisch in DB speichern
- [ ] **Feature:** Startwochentag für Wochenplan konfigurierbar machen
- [ ] **Feature:** Prompt-Optionen (abwechslungsreich, wiederholend, saisonal, günstig)

### Frontend

#### 1.4 Rezept-Verwaltung
- [ ] **Route:** `/recipes` - Rezept-Übersicht
  - [ ] Liste aller Rezepte (mit Suche & Filter)
  - [ ] Rezept-Detailansicht
  - [ ] "Zum Wochenplan hinzufügen"-Button
- [ ] **Route:** `/recipes/new` - Rezept manuell erstellen
  - [ ] Formular für Name, Beschreibung, Zutaten, Tags
- [ ] **Route:** `/recipes/[id]/edit` - Rezept bearbeiten
- [ ] **Component:** `<RecipeCard />` - Rezept-Karte
- [ ] **Component:** `<RecipeForm />` - Rezept-Formular
- [ ] **Hook:** `useRecipes()` - Rezepte laden
- [ ] **Hook:** `useCreateRecipe()` - Rezept erstellen
- [ ] **Hook:** `useUpdateRecipe()` - Rezept aktualisieren
- [ ] **Hook:** `useDeleteRecipe()` - Rezept löschen

#### 1.5 Wochenplan-Ansicht
- [ ] **Route:** `/plan` - Wochenplan-Übersicht
  - [ ] Kalender-Raster (7 Tage × 3 Mahlzeiten)
  - [ ] Rezepte per Drag & Drop zuordnen
  - [ ] "Neue Woche generieren"-Button
  - [ ] Startwochentag auswählbar
- [ ] **Component:** `<WeekPlan />` - Wochenplan-Grid
- [ ] **Component:** `<DayPlan />` - Tagesplan
- [ ] **Component:** `<MealSlot />` - Mahlzeiten-Slot
- [ ] **Hook:** `useMealPlan()` - Wochenplan laden
- [ ] **Hook:** `useSaveMealPlan()` - Wochenplan speichern
- [ ] **Hook:** `useGenerateMealPlan()` - Neuen Plan generieren

#### 1.6 Rezept-Generierung (bereits teilweise vorhanden)
- [x] **Route:** `/recipes/generate` - Rezepte generieren
- [ ] **Feature:** Generierte Rezepte direkt speichern
- [ ] **Feature:** Generierte Rezepte zum Wochenplan hinzufügen

---

## 🚀 Phase 2: Erweiterte Features

### Backend

#### 2.1 Rezept-Upload & OCR
- [ ] **Port:** `IFileStorage` erweitern (Supabase Storage Integration)
- [ ] **Port:** `IOCRService` (OCR für PDF/Bilder)
- [ ] **UseCase:** `ProcessRecipeUpload` (Upload → OCR → Zutaten-Extraktion)
- [ ] **Controller:** `POST /api/recipes/upload` (Datei-Upload)
- [ ] **Feature:** PDF-Parsing (Text-Extraktion)
- [ ] **Feature:** Bild-OCR (z.B. Google Vision API oder ocr.space)
- [ ] **Feature:** LLM-basierte Zutaten-Extraktion aus OCR-Text

#### 2.2 Tag-System erweitern
- [ ] **Entity:** Tag-Kategorien (Zubereitung, Ernährung, Zutat)
- [ ] **Feature:** Tag-Autovervollständigung
- [ ] **Feature:** Tag-Statistiken (Häufigkeit)
- [ ] **Controller:** `GET /api/tags` (Alle Tags)
- [ ] **Controller:** `GET /api/tags/statistics` (Tag-Statistiken)

#### 2.3 Favoriten & Bewertungen
- [ ] **Entity:** `RecipeRating` (Bewertung 1-5, Favorit-Flag)
- [ ] **Controller:** `POST /api/recipes/{id}/favorite` (Als Favorit markieren)
- [ ] **Controller:** `POST /api/recipes/{id}/rating` (Bewerten)
- [ ] **Controller:** `GET /api/recipes/favorites` (Favoriten auflisten)

#### 2.4 Nährwertangaben
- [ ] **Entity:** `NutritionInfo` (Kalorien, Protein, Kohlenhydrate, Fett)
- [ ] **Port:** `INutritionService` (API-Integration oder LLM-basiert)
- [ ] **UseCase:** `CalculateNutrition` (Nährwerte berechnen)
- [ ] **Feature:** Automatische Berechnung aus Zutaten

### Frontend

#### 2.5 Dashboard
- [ ] **Route:** `/dashboard` - Dashboard-Übersicht
  - [ ] Verbrauchsstatistik (Tag-Häufigkeiten)
  - [ ] Favoriten-Liste
  - [ ] Heute-Ansicht (Was steht heute an?)
- [ ] **Component:** `<ConsumptionStats />` - Statistik-Diagramm
- [ ] **Component:** `<FavoritesList />` - Favoriten-Liste
- [ ] **Component:** `<TodayPlan />` - Heute-Übersicht
- [ ] **Hook:** `useConsumptionStats()` - Statistiken laden
- [ ] **Hook:** `useFavorites()` - Favoriten laden

#### 2.6 Rezept-Upload
- [ ] **Route:** `/recipes/upload` - Rezept hochladen
  - [ ] Drag & Drop Upload
  - [ ] OCR-Ergebnis anzeigen
  - [ ] Zutaten-Editor
  - [ ] Speichern-Button
- [ ] **Component:** `<RecipeUpload />` - Upload-Interface
- [ ] **Component:** `<IngredientEditor />` - Zutaten-Editor
- [ ] **Hook:** `useUploadRecipe()` - Upload verarbeiten

#### 2.7 Export-Funktionen
- [ ] **Feature:** Wochenplan als PDF exportieren
- [ ] **Feature:** Einkaufsliste als Excel exportieren
- [ ] **Component:** `<ExportButton />` - Export-Optionen

---

## 🌟 Phase 3: Erweiterte Planung & Gruppen

### Backend

#### 3.1 Wiederholungslogik
- [ ] **Entity:** `RecipePreference` (Wiederholungsintervall, z.B. "alle 2 Wochen")
- [ ] **UseCase:** `GeneratePlanWithPreferences` (Plan mit Wiederholungen)
- [ ] **Feature:** Intelligente Planung basierend auf Historie
- [ ] **Feature:** Saisonale Rezepte (basierend auf Datum)

#### 3.2 Gruppen & geteilte Planung
- [ ] **Entity:** `Group` (Name, Mitglieder)
- [ ] **Entity:** `GroupInvite` (Token, Ablaufdatum)
- [ ] **Entity:** `GroupMember` (User, Rolle: Admin/Member)
- [ ] **Controller:** `POST /api/groups` (Gruppe erstellen)
- [ ] **Controller:** `POST /api/groups/{id}/invite` (Invite senden)
- [ ] **Controller:** `POST /api/groups/join` (Gruppe beitreten)
- [ ] **Controller:** `GET /api/groups/{id}/meal-plans` (Geteilte Pläne)
- [ ] **Feature:** Geteilte Rezepte (Gruppen-Rezepte)
- [ ] **Feature:** Geteilte Einkaufsliste

#### 3.3 Historie & Statistiken
- [ ] **Controller:** `GET /api/meal-plans/history` (Vergangene Pläne)
- [ ] **Controller:** `GET /api/statistics/consumption` (Verbrauchsstatistik)
- [ ] **Feature:** Zeitreihen-Analyse (Was wurde wann gegessen?)

### Frontend

#### 3.4 Gruppen-Verwaltung
- [ ] **Route:** `/groups` - Gruppen-Übersicht
- [ ] **Route:** `/groups/new` - Gruppe erstellen
- [ ] **Route:** `/groups/[id]` - Gruppen-Detail
- [ ] **Component:** `<GroupList />` - Gruppen-Liste
- [ ] **Component:** `<GroupInvite />` - Invite-Formular
- [ ] **Hook:** `useGroups()` - Gruppen laden
- [ ] **Hook:** `useCreateGroup()` - Gruppe erstellen

#### 3.5 Historie & Statistiken
- [ ] **Route:** `/history` - Historie-Ansicht
  - [ ] Kalender-Ansicht vergangener Wochen
  - [ ] Statistiken (Tag-Häufigkeiten, Trends)
- [ ] **Component:** `<HistoryCalendar />` - Historie-Kalender
- [ ] **Component:** `<StatisticsChart />` - Statistik-Diagramm
- [ ] **Hook:** `useHistory()` - Historie laden

---

## 🔮 Phase 4: Erweiterte KI & Optimierungen

### Backend

#### 4.1 Erweiterte KI-Features
- [ ] **Feature:** Kochanleitungen automatisch vervollständigen
- [ ] **Feature:** Rezept-Optimierung (z.B. "weniger Zutaten", "schneller")
- [ ] **Feature:** Intelligente Zutaten-Vorschläge (basierend auf Historie)
- [ ] **Feature:** Verschwendungs-Minimierung (ähnliche Zutaten gruppieren)

#### 4.2 Performance & Caching
- [ ] **Feature:** Caching für häufig abgerufene Daten
- [ ] **Feature:** Optimierte Datenbank-Queries
- [ ] **Feature:** Background-Jobs für aufwendige Operationen

### Frontend

#### 4.3 Mobile Optimierung
- [ ] **Feature:** Responsive Design für alle Komponenten
- [ ] **Feature:** Touch-Optimierung (Drag & Drop auf Mobile)
- [ ] **Feature:** Offline-Funktionalität (Service Worker)

#### 4.4 UX-Verbesserungen
- [ ] **Feature:** Dark Mode
- [ ] **Feature:** Benachrichtigungen (z.B. "Neue Woche generieren?")
- [ ] **Feature:** Keyboard-Shortcuts

---

## 📋 Technische Entscheidungen & Offene Fragen

### KI-Integration
- [ ] **Entscheidung:** Welches LLM für Production? (OpenAI GPT, Anthropic Claude, Mistral, Ollama?)
- [ ] **Entscheidung:** API-basiert oder lokales Modell?
- [ ] **Implementierung:** LLM-Provider-Abstraktion (bereits vorbereitet)

### Tag-System
- [ ] **Entscheidung:** Eigene Datenbanktabellen mit Kategorien?
- [ ] **Implementierung:** Tag-Kategorien (Zubereitung, Ernährung, Zutat)

### Visualisierung
- [ ] **Entscheidung:** Welche Chart-Bibliothek? (recharts, chart.js, d3?)
- [ ] **Entscheidung:** Kalender-Komponente? (react-big-calendar, fullcalendar?)

### Wiederholungslogik
- [ ] **Entscheidung:** Wie technisch umsetzen? (Cron-Jobs, Background-Service?)
- [ ] **Implementierung:** Wiederholungs-Engine

---

## 🎯 Priorisierung

### Must-Have (MVP)
1. Rezept-Datenmodell & Persistierung
2. Wochenplan-Persistierung
3. Rezept-Verwaltung (CRUD)
4. Wochenplan-Ansicht
5. Generierte Rezepte speichern

### Should-Have (Phase 2)
1. Rezept-Upload & OCR
2. Dashboard mit Statistiken
3. Favoriten & Bewertungen
4. Tag-System erweitern

### Nice-to-Have (Phase 3+)
1. Gruppen & geteilte Planung
2. Historie & erweiterte Statistiken
3. Wiederholungslogik
4. Export-Funktionen

---

## 📝 Notizen

- **Aktueller Stand:** Rezept-Generierung funktioniert, aber Rezepte werden noch nicht persistiert
- **Nächster Schritt:** Datenmodell für Rezepte & Wochenplan implementieren
- **Wichtig:** Alle Features sollten testbar sein (Unit Tests + Integration Tests)
