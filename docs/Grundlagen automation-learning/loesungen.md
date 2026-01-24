# loesungen.md

## Zweck

Dieses Dokument sammelt **konkrete, umsetzbare Lösungen**, die **direkt aus den in `schwaechen.md` beschriebenen Schwächen** abgeleitet werden.

Es enthält **keine neuen Schwächen** und **keine Beobachtungen** aus Sessions.

---

## Grundregeln

- Jede Lösung referenziert **genau eine Schwäche**
- Lösungen sind **konkret, überprüfbar und umsetzbar**
- Lösungen werden **nicht aus Sessions**, sondern aus `schwaechen.md` abgeleitet
- Eine Schwäche kann mehrere Lösungen haben

---

## Struktur einer Lösung (Pflicht)

Jede Lösung wird nach folgendem Schema dokumentiert:

```md
### <Titel der Lösung>

**Bezieht sich auf Schwäche:** <exakter Titel aus schwaechen.md>

**Ziel**  
Was soll sich durch diese Lösung konkret verbessern?

| Aspekt | Aktueller Zustand | Zielzustand | Lösung | Referenz |
|------|------------------|------------|--------|----------|
|      |                  |            |        |          |

**Erfolgskriterium**  
Woran erkennt man objektiv, dass die Lösung wirkt?

**Status**  
- geplant | aktiv | validiert
```

---

## Aktive Lösungen

### Explizite Zieldefinition erzwingen

**Bezieht sich auf Schwäche:** Implizite Zieldefinition

**Ziel**  
Sicherstellen, dass Ziele, Nicht-Ziele und Constraints vor jeder relevanten Arbeit klar formuliert sind.

| Aspekt | Aktueller Zustand | Zielzustand | Lösung | Referenz |
|------|------------------|------------|--------|----------|
| Zielklarheit | implizit | explizit | Verpflichtende Zielsektion in jeder Session | md-system-regeln.md |

**Erfolgskriterium**  
Neue Sessions enthalten explizite Ziel- und Nicht-Ziel-Sektionen.

**Status**  
- aktiv

---

### Trennung der Ebenen erzwingen

**Bezieht sich auf Schwäche:** Vermischung von Ebenen

**Ziel**  
Verhindern, dass Beobachtung, Analyse und Abstraktion vermischt werden.

| Aspekt | Aktueller Zustand | Zielzustand | Lösung | Referenz |
|------|------------------|------------|--------|----------|
| Ebenentrennung | unscharf | klar getrennt | Strikte MD-Typen + Gates | md-architecture-overview.md |

**Erfolgskriterium**  
Schwächen tauchen nicht mehr direkt in Sessions auf.

**Status**  
- aktiv

---

### Struktur früh externalisieren

**Bezieht sich auf Schwäche:** Spätes Externalisieren von Struktur

**Ziel**  
Gedankliche Modelle früh sichtbar und teilbar machen.

| Aspekt | Aktueller Zustand | Zielzustand | Lösung | Referenz |
|------|------------------|------------|--------|----------|
| Struktur | mental | dokumentiert | Frühes Anlegen von MD-Grundstrukturen | md-system-regeln.md |

**Erfolgskriterium**  
Neue Themen starten mit einer leeren Struktur-MD.

**Status**  
- aktiv

---

## Validierte Lösungen

*(noch leer)*

---

## Verbindung zu Regeln

- Validierte Lösungen sind **Kandidaten für `regeln.md`**
- Erst nach Validierung dürfen Lösungen zu Regeln werden

---

## Rolle dieses Dokuments

Dieses Dokument ist:
- handlungsorientiert
- überprüfbar
- direkt mit Schwächen verknüpft

Es bildet die **Brücke zwischen Analyse und Steuerung**.
