## 📄 /upload

**Ziel:**  
Nutzer lädt ein PDF oder Bild eines Rezepts hoch.  
OCR erkennt Zutaten, diese werden als Liste angezeigt. Nutzer kann sie bearbeiten und speichern.

**Technik:**
- Upload mit Supabase Storage
- OCR via externem Service (ocr.space oder Google Vision)
- Zutatenparser mit GPT / Regex
- Resultat wird als `recipe` in Postgres gespeichert

**Komponenten:**
- `<UploadBox />`
- `<ParsedIngredientsEditor />`
- `<SaveRecipeButton />`

**Backend:**
- POST `/api/recipes/upload`
- Auth via Supabase JWT

---

## 📄 /recipes

**Ziel:**  
Übersicht aller hochgeladenen Rezepte (nur eigene), mit Such- und Filteroptionen.

**Funktionen:**
- Liste nach Datum sortiert
- „Zum Wochenplan hinzufügen“-Button
- Optional: Bearbeiten / Löschen

**Frontend-Logik:**
- `useRecipesQuery()`
- `useAddToPlanMutation()`

---

## 📄 /plan

**Ziel:**  
Zeigt aktuellen Wochenplan (7 Tage, je 1–2 Gerichte).  
Gerichte können ersetzt oder neu generiert werden.

**Funktionen:**
- Vorschläge pro Tag
- Button: „Neue Woche würfeln“
- GPT Prompt z. B. „Low Carb mit Fisch“

---

## 📄 /shopping-list

**Ziel:**  
Zeigt aggregierte Zutaten aller Gerichte im Wochenplan.  
Erlaubt Übertragung an Bring.

**Funktionen:**
- Gruppen nach Kategorie (Obst, Milchprodukte…)
- „An Bring senden“-Button
- Optional: PDF-Export

---

## 📄 /login

**Ziel:**  
Login via Supabase (email + pw)

**Technik:**
- `supabase.auth.signInWithPassword(...)`
- Session im Frontend via Supabase Client
- TanStack Router: redirect if not logged in
