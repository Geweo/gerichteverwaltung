# Projektziel – Zutaten- & Rezeptplaner mit Bring-Anbindung

## 🎯 Ziel
Diese App ermöglicht es Nutzer:innen, eigene Rezepte hochzuladen (PDF/Bild), die Zutaten automatisch extrahieren zu lassen, einen wöchentlichen Plan zu erstellen und alle benötigten Zutaten direkt an die Bring-Einkaufsliste zu übertragen.

## 🔧 Architekturüberblick

- **Frontend**: React 19, TanStack Router, TanStack Query, shadcn/ui
- **Backend**: C# (Hexagonale Architektur), PostgreSQL, EF Core
- **Auth**: Supabase (nur Auth)
- **Storage**: Supabase Storage für Bilder & PDFs
- **KI-Unterstützung**: GPT (OCR-Zutatenanalyse, Rezeptvorschläge)

## 🔐 Auth-Strategie

- Supabase JWT wird im Frontend gespeichert
- Backend validiert gegen Supabase JWKS
- `sub` aus Token wird auf eigenen `User` in PostgreSQL gemappt

## 👨‍🍳 Hauptfeatures

- Rezept-Upload + OCR
- Zutatenanalyse & Bearbeitung
- Wochenplanung (Zufall oder Wunschprompt)
- Einkaufsliste aggregieren
- Export an Bring.com

## ✅ MVP-Ziele

- Upload & Parsing 1 PDF
- Zutatenstruktur speichern
- Wochenplan erzeugen
- 1x Bring-Sync erfolgreich durchführen

## 🧠 Warum?
Weil Nutzer:innen realistische Rezepte verwalten wollen, nicht 10.000 aus einer Cloud – sondern die, die sie selbst hochladen.

