# Ollama Troubleshooting

**Problem:** `Network Error` beim Generieren von Rezepten

---

## 🔍 Problemdiagnose

Der Fehler "Network Error" kann mehrere Ursachen haben:

1. **Backend läuft nicht** - API ist nicht erreichbar
2. **Ollama ist nicht erreichbar** - Backend kann Ollama nicht erreichen
3. **CORS-Problem** - Frontend kann Backend nicht erreichen
4. **Falsche API-URL** - Frontend verbindet sich mit falscher URL

---

## ✅ Schritt-für-Schritt Lösung

### Schritt 1: Prüfe, ob Backend läuft

**Im Terminal:**
```powershell
# Prüfe, ob Backend auf Port 5000 läuft
curl http://localhost:5000/swagger/v1/swagger.json
```

**Oder im Browser:**
- Öffne: http://localhost:5000/swagger
- Sollte Swagger UI anzeigen

**Falls nicht erreichbar:**
1. Backend starten:
   ```powershell
   cd server
   $env:ASPNETCORE_ENVIRONMENT="Local"
   dotnet run --project Ernaehrbar.Api
   ```

2. Prüfe, ob Port 5000 frei ist:
   ```powershell
   netstat -ano | findstr :5000
   ```

---

### Schritt 2: Prüfe, ob Ollama läuft

**Docker Container:**
```powershell
docker ps --filter "name=ollama"
```

**Sollte zeigen:**
```
NAMES       STATUS         PORTS
ollama-ai   Up X minutes   0.0.0.0:11434->11434/tcp
```

**Falls nicht:**
```powershell
docker compose up -d ollama
```

**Ollama direkt testen:**
```powershell
curl http://localhost:11434/api/tags
```

Sollte eine Liste der installierten Modelle zurückgeben.

---

### Schritt 3: Prüfe Ollama-Modell

**Prüfe, ob `llama3.2` installiert ist:**
```powershell
curl http://localhost:11434/api/tags
```

**Falls nicht installiert:**
```powershell
docker exec ollama-ai ollama pull llama3.2
```

**Oder mit Script:**
```powershell
.\install-ollama-model.ps1
```

---

### Schritt 4: Prüfe Backend-Konfiguration

**Prüfe `appsettings.Local.json`:**
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

**Wichtig:** Backend muss mit `ASPNETCORE_ENVIRONMENT=Local` gestartet werden!

---

### Schritt 5: Prüfe Frontend-Konfiguration

**Prüfe `client/.env`:**
```env
VITE_API_URL=http://localhost:5000
```

**Falls nicht gesetzt:**
- Frontend verwendet Standard: `http://localhost:5000`
- Prüfe Browser-Konsole für tatsächlich verwendete URL

---

### Schritt 6: Prüfe CORS

**Backend CORS-Konfiguration:**
- In `Program.cs` ist CORS für `Development` und `Local` erlaubt
- Prüfe, ob Backend mit `Local` Environment läuft

**Browser DevTools → Network:**
- Prüfe, ob CORS-Fehler in der Konsole erscheinen
- Prüfe, ob Request zu `http://localhost:5000/api/recipes/generate` geht

---

### Schritt 7: Prüfe Backend-Logs

**Backend-Logs zeigen:**
- Ob Ollama-Request erfolgreich war
- Ob Fehler beim Aufruf von Ollama auftraten

**Typische Fehler:**
- `Connection refused` → Ollama läuft nicht
- `Model not found` → Modell nicht installiert
- `Timeout` → Ollama antwortet zu langsam

---

## 🔧 Häufige Probleme

### Problem 1: Backend läuft nicht

**Symptom:** `Network Error` im Frontend

**Lösung:**
1. Backend starten:
   ```powershell
   cd server
   $env:ASPNETCORE_ENVIRONMENT="Local"
   dotnet run --project Ernaehrbar.Api
   ```

2. Prüfe, ob Backend auf Port 5000 läuft:
   ```powershell
   curl http://localhost:5000/swagger/v1/swagger.json
   ```

---

### Problem 2: Ollama läuft nicht

**Symptom:** Backend-Logs zeigen `Connection refused` zu Ollama

**Lösung:**
```powershell
docker compose up -d ollama
```

**Prüfe Status:**
```powershell
docker ps --filter "name=ollama"
```

---

### Problem 3: Modell nicht installiert

**Symptom:** Backend-Logs zeigen `Model not found`

**Lösung:**
```powershell
docker exec ollama-ai ollama pull llama3.2
```

**Prüfe installierte Modelle:**
```powershell
curl http://localhost:11434/api/tags
```

---

### Problem 4: CORS-Fehler

**Symptom:** Browser-Konsole zeigt CORS-Fehler

**Lösung:**
- Stelle sicher, dass Backend mit `ASPNETCORE_ENVIRONMENT=Local` läuft
- CORS ist für `Development` und `Local` erlaubt

**Prüfe `Program.cs`:**
```csharp
if (builder.Environment.IsDevelopment())
{
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader();
}
```

**Hinweis:** `Local` Environment wird als `Development` behandelt, aber prüfe die CORS-Logik.

---

### Problem 5: Falsche API-URL

**Symptom:** Frontend verbindet sich mit falscher URL

**Lösung:**
1. Prüfe `client/.env`:
   ```env
   VITE_API_URL=http://localhost:5000
   ```

2. **Wichtig:** Dev-Server neu starten nach `.env`-Änderung!

3. Prüfe Browser-Konsole für tatsächlich verwendete URL

---

## ✅ Checkliste

- [ ] Backend läuft (`curl http://localhost:5000/swagger/v1/swagger.json`)
- [ ] Backend läuft mit `ASPNETCORE_ENVIRONMENT=Local`
- [ ] Ollama Container läuft (`docker ps --filter "name=ollama"`)
- [ ] Ollama ist erreichbar (`curl http://localhost:11434/api/tags`)
- [ ] Modell `llama3.2` ist installiert (`curl http://localhost:11434/api/tags`)
- [ ] `appsettings.Local.json` enthält korrekte Ollama-URL
- [ ] Frontend `.env` enthält `VITE_API_URL=http://localhost:5000`
- [ ] Dev-Server wurde nach `.env`-Änderung neu gestartet
- [ ] Keine CORS-Fehler in Browser-Konsole
- [ ] Backend-Logs zeigen keine Fehler

---

## 🎯 Schnelllösung

1. **Backend starten:**
   ```powershell
   cd server
   $env:ASPNETCORE_ENVIRONMENT="Local"
   dotnet run --project Ernaehrbar.Api
   ```

2. **Ollama prüfen:**
   ```powershell
   docker ps --filter "name=ollama"
   docker exec ollama-ai ollama pull llama3.2
   ```

3. **Frontend prüfen:**
   - Öffne Browser DevTools → Network
   - Versuche Rezept zu generieren
   - Prüfe, ob Request zu `http://localhost:5000/api/recipes/generate` geht

---

**Status:** Nach diesen Schritten sollte Ollama erreichbar sein! ✅
