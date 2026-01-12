# Ollama Setup

## Installation

### Option 1: Lokale Installation (empfohlen für Development)

1. Installiere Ollama lokal von [ollama.com](https://ollama.com)
2. Starte Ollama (wird normalerweise automatisch als Service gestartet)
3. Installiere das Modell:
```bash
ollama pull llama3.2
```
4. Prüfe, ob Ollama läuft:
```bash
# Windows PowerShell
Invoke-WebRequest -Uri http://localhost:11434/api/tags

# Oder im Browser öffnen:
# http://localhost:11434/api/tags
```

Die Konfiguration ist bereits in `appsettings.Development.json` auf `http://localhost:11434` eingestellt.

### Option 2: Docker Container

1. Stelle sicher, dass der Ollama-Container läuft:
```bash
docker-compose up -d ollama
```

2. Installiere das Modell im Container:

**Option A: Direkt (empfohlen)**
```bash
docker exec ernaehrbar-ollama ollama pull llama3.2
```

**Option B: Interaktiv (falls Option A fehlschlägt)**
```bash
docker exec -it ernaehrbar-ollama sh
ollama pull llama3.2
exit
```

**Option C: PowerShell-Skript**
```powershell
.\install-ollama-model.ps1
```

3. Prüfe, ob das Modell installiert wurde:
```bash
docker exec ernaehrbar-ollama ollama list
```

## Fehlerbehebung

### Fehler: "ssh: no key found"

Dieser Fehler kann auftreten, wenn:
- Der Container nicht richtig konfiguriert ist
- Netzwerkprobleme vorliegen
- Die Ollama-Version ein Problem hat

**Lösungen:**
1. Container neu starten:
   ```bash
   docker restart ernaehrbar-ollama
   ```

2. Container neu erstellen:
   ```bash
   docker-compose down ollama
   docker-compose up -d ollama
   ```

3. Interaktiv installieren (siehe Option B oben)

## Alternative: Modell lokal installieren

Falls du Ollama lokal installiert hast (nicht im Docker-Container):

```bash
ollama pull llama3.2
```

## Konfiguration

Die LLM-Konfiguration befindet sich in:
- **Development (lokal)**: `server/Ernaehrbar.Api/appsettings.Development.json`
- **Production**: `server/Ernaehrbar.Api/appsettings.json`

### Aktuelle Konfiguration (Development)

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

### Modell wechseln

Um ein anderes Modell zu verwenden, ändere `LLM:Ollama:ModelName` in der entsprechenden `appsettings.json`:

Beliebte Alternativen:
- `llama3.1` - Ältere Version
- `llama3` - Größeres Modell
- `mistral` - Anderes Modell
- `phi` - Kleineres, schnelleres Modell

### Zukünftige LLM-Provider (Dev/Prod)

Die Konfiguration ist bereits vorbereitet für andere LLM-Provider wie OpenAI oder Anthropic. Um diese zu verwenden:

1. Ändere `LLM:Provider` auf `"OpenAI"` oder `"Anthropic"`
2. Füge die entsprechenden API-Keys in die Konfiguration ein
3. Implementiere die entsprechenden Adapter in `Ernaehrbar.Adapters.Infrastructure.LLM`

## Testen der Verbindung

### 1. Prüfe, ob Ollama läuft

**Windows PowerShell:**
```powershell
Invoke-WebRequest -Uri http://localhost:11434/api/tags
```

**Browser:**
Öffne: `http://localhost:11434/api/tags`

**Command Line:**
```bash
curl http://localhost:11434/api/tags
```

### 2. Teste die API

Starte die Backend-API und teste den Endpoint:
```bash
POST http://localhost:5000/api/recipes/generate
```

Mit Body:
```json
{
  "prompt": "Gesunde vegetarische Rezepte",
  "mealCategories": [1, 2, 3],
  "numberOfDays": 7
}
```
