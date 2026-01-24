# Ollama: Docker vs. Lokale Installation

**Datum:** Nach Einführung lokaler Ollama-Unterstützung

---

## 🔄 Zwei Optionen für Ollama

Das Backend unterstützt zwei Möglichkeiten, Ollama zu verwenden:

1. **Docker-Ollama** (Standard) - Ollama läuft im Docker-Container
2. **Lokale Ollama-Installation** - Ollama läuft direkt auf dem System

---

## 🐳 Option 1: Docker-Ollama (Standard)

### Vorteile
- ✅ Einfache Installation (nur `docker compose up -d`)
- ✅ Isoliert vom System
- ✅ Einfach zu entfernen (Container stoppen)
- ✅ Konsistent über verschiedene Systeme

### Nachteile
- ❌ Kann Probleme mit Modell-Installation haben
- ❌ Benötigt Docker

### Konfiguration

**`appsettings.Local.json`:**
```json
{
  "LLM": {
    "Provider": "Ollama",
    "Ollama": {
      "Url": "http://localhost:11434",
      "ModelName": "llama3.2"
    }
  }
}
```

**Setup:**
1. Docker Container starten: `docker compose up -d ollama`
2. Modell installieren: `docker exec ollama-ai ollama pull llama3.2`

---

## 💻 Option 2: Lokale Ollama-Installation

### Vorteile
- ✅ Keine Docker-Abhängigkeit
- ✅ Oft stabiler bei Modell-Installation
- ✅ Direkter Zugriff auf System-Ressourcen

### Nachteile
- ❌ Muss separat installiert werden
- ❌ Läuft direkt auf dem System

### Installation

**Windows:**
1. Download von [ollama.com](https://ollama.com)
2. Installer ausführen
3. Ollama startet automatisch als Service

**Prüfen:**
```powershell
ollama --version
ollama list
```

**Modell installieren:**
```powershell
ollama pull llama3.2
# oder
ollama pull llama
```

### Konfiguration

**`appsettings.Local.json`:**
```json
{
  "LLM": {
    "Provider": "Ollama",
    "Ollama": {
      "Url": "http://localhost:11434",
      "ModelName": "llama"
      // oder "llama3.2" - je nach installiertem Modell
    }
  }
}
```

**Wichtig:** Stelle sicher, dass:
- Ollama als Service läuft (Windows) oder gestartet ist
- Port 11434 erreichbar ist
- Das Modell installiert ist (`ollama list`)

---

## 🔄 Wechseln zwischen Docker und Lokal

### Von Docker zu Lokal

1. **Docker-Ollama stoppen:**
   ```powershell
   docker compose stop ollama
   ```

2. **Lokale Ollama starten:**
   - Windows: Service sollte automatisch laufen
   - Prüfen: `ollama list` sollte funktionieren

3. **Konfiguration anpassen:**
   ```json
   {
     "LLM": {
       "Ollama": {
         "Url": "http://localhost:11434",
         "ModelName": "llama"  // oder wie lokal installiert
       }
     }
   }
   ```

4. **Backend neu starten**

### Von Lokal zu Docker

1. **Lokale Ollama stoppen:**
   - Windows: Service stoppen (Services.msc)

2. **Docker-Ollama starten:**
   ```powershell
   docker compose up -d ollama
   ```

3. **Modell installieren:**
   ```powershell
   docker exec ollama-ai ollama pull llama3.2
   ```

4. **Konfiguration anpassen:**
   ```json
   {
     "LLM": {
       "Ollama": {
         "Url": "http://localhost:11434",
         "ModelName": "llama3.2"
       }
     }
   }
   ```

5. **Backend neu starten**

---

## 🔍 Prüfen, welche Version läuft

### Docker-Ollama prüfen

```powershell
# Container-Status
docker ps --filter "name=ollama-ai"

# Installierte Modelle
docker exec ollama-ai ollama list

# API testen
Invoke-RestMethod -Uri "http://localhost:11434/api/tags"
```

### Lokale Ollama prüfen

```powershell
# Version
ollama --version

# Installierte Modelle
ollama list

# API testen
Invoke-RestMethod -Uri "http://localhost:11434/api/tags"
```

**Wichtig:** Beide können nicht gleichzeitig auf Port 11434 laufen!

---

## 🎯 Empfehlung

**Für lokale Entwicklung:**
- **Lokale Installation** wenn Docker Probleme macht
- **Docker** wenn alles funktioniert

**Für Team-Entwicklung:**
- **Docker** (konsistent für alle)

---

## ✅ Checkliste

### Docker-Ollama
- [ ] `docker compose up -d ollama` ausgeführt
- [ ] Container läuft (`docker ps`)
- [ ] Modell installiert (`docker exec ollama-ai ollama list`)
- [ ] `appsettings.Local.json` zeigt auf Docker-URL
- [ ] Backend kann Ollama erreichen

### Lokale Ollama
- [ ] Ollama installiert (`ollama --version`)
- [ ] Ollama läuft (Service oder Prozess)
- [ ] Modell installiert (`ollama list`)
- [ ] `appsettings.Local.json` zeigt auf lokale URL
- [ ] Backend kann Ollama erreichen

---

**Status:** Beide Optionen sind konfigurierbar! ✅
