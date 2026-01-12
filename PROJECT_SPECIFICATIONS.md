# 🧠 Projektzusammenfassung – Dashboard & Gerichteverwaltung (Zutaten-App)

## 🎯 Ziel

Das Dashboard und die Gerichtsseite der Zutaten-/Rezept-App sollen über rein statische Listen hinausgehen und echte Mehrwerte bieten: Wiederverwendbarkeit, Kategorisierung, Verlauf, visuelle Planung und Präferenzen-Management.

---

## 📊 Dashboard – Funktionsüberblick

### 🔹 1. Verbrauchsstatistik (Essverhalten)

* **Auswertung anhand von Tags**, z. B.:

  * Wie oft wurde "vegetarisch", "vegan", "Kidneybohnen", "Wurst" gegessen?
  * Wie häufig kam ein bestimmtes Tag insgesamt vor (zählbar + filterbar)?
* Visualisierung: z. B. Balkendiagramm oder Liste mit Häufigkeiten ("Zutat X: 5× gegessen").

### 🔹 2. Favoriten-Tracking

* Gerichte mit hoher Bewertung oder "Favorisieren"-Markierung
* Möglichkeit, Favoriten zu listen
* Option: "Dieses Gericht bitte öfter" → z. B. alle 2 Wochen statt alle 3

### 🔹 3. Wochenübersicht / Tagesplan

* Aktuelle Woche: Welche Gerichte stehen an?
* Heute: Direktansicht, was auf dem Plan steht
* Visualisierung: Kalender-Raster oder Scroll-Liste

---

## 🍽️ Gerichtsseite – Funktionsüberblick

### 🔹 1. Upload & Auswertung

* Bild- oder PDF-Upload von Rezepten
* Zutaten werden automatisch erkannt
* Optional: Kochprozess durch LLM ergänzt (GPT, Lama, etc.)

### 🔹 2. Bearbeitung & Verwaltung

* Gerichte manuell editierbar (Titel, Zutaten, Prozess, Tags)
* Möglichkeit zum Löschen
* Exportfunktion: PDF oder Excel

### 🔹 3. Nährwertangaben

* Automatisch berechnet oder ergänzt durch LLM/API

### 🔹 4. Tag-System

* Tags = zentrale Referenz
* Unterschiedliche Kategorien von Tags:

  * Zubereitungsart ("schnell", "aufwendig")
  * Ernährung ("vegan", "low-carb")
  * Zutatenbasiert ("Kidneybohnen", "Fisch")
* Tags aus Prompt automatisch extrahieren lassen

---

## 📅 Wochenplanung & Wiederholungslogik

* Nutzer gibt an, **an welchem Wochentag** der Plan starten soll (z. B. Montag)
* Gericht kann für **jede einzelne Mahlzeit** einer Woche geplant werden
* Prompts steuern z. B.:

  * abwechslungsreich
  * sich wiederholend
  * saisonal
  * möglichst ähnliche Zutaten (weniger Verschwendung)
  * günstig

---

## 👥 Gruppen-Login / geteilte Planung

* Möglichkeit, **Gruppen anzulegen** (z. B. Paare, WGs)
* Nutzer kann andere per Invite einladen
* Gruppen teilen:

  * Kalender / Wochenplan
  * Rezepte / Favoriten
  * Einkaufsliste (z. B. Bring-Sync gemeinsam)

---

## 🤖 KI-Komponenten / Modelle

* LLM soll:

  * Zutaten aus OCR-Daten extrahieren
  * Kochanleitungen vervollständigen
  * Rezepte auf Basis von Vorgaben generieren
  * Tags aus Gerichten/PDFs automatisch erkennen
* Entscheidung noch offen: GPT, Mistral, Lama?

---

## 🔄 Historie & Planung

* Vergangene Wochen auswertbar
* Statistiken: Was wurde wann gegessen?
* Zukunft: Welche Gerichte stehen in der Planung?
* Visualisierung als Verlauf (Zeitleiste) oder Kalender

---

## 📌 To Do / Offene Fragen

* Wie tief soll die KI integriert werden? (Fertiges Training oder API-Nutzung?)
* Wie wird die Wiederholung geregelt (z. B. "alle 2 Wochen") technisch umgesetzt?
* Braucht das Tag-System eigene Datenbanktabellen mit Kategorien?
* Wie sieht die ideale Visualisierung für den Wochenplan aus (mobile & desktop)?
