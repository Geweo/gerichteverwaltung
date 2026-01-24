# Lokales Setup - Vollständig eingerichtet ✅

**Datum:** Nach Supabase CLI Setup und Frontend-Konfiguration

---

## ✅ Was ist jetzt eingerichtet?

### 1. Supabase (lokal)

- ✅ **Supabase CLI installiert** (über Scoop)
- ✅ **Supabase gestartet** (`supabase start`)
- ✅ **Docker-Container** laufen automatisch
- ✅ **Studio verfügbar** unter http://127.0.0.1:54323
- ✅ **API Gateway** unter http://127.0.0.1:54321

### 2. Frontend

- ✅ **`.env` konfiguriert** mit:
  - `VITE_SUPABASE_URL=http://127.0.0.1:54321`
  - `VITE_SUPABASE_ANON_KEY=sb_publishable_...`
- ✅ **Registrierung funktioniert** mit lokaler Supabase
- ✅ **Login funktioniert** mit lokaler Supabase
- ✅ **JWT Token** werden automatisch verwaltet

### 3. Backend

- ✅ **`appsettings.Local.json` konfiguriert** mit:
  - `Supabase:Url=http://localhost:54321`
  - `Supabase:JwksUrl=http://localhost:54321/auth/v1/.well-known/jwks.json`
- ✅ **JWT-Validierung** funktioniert mit lokaler Supabase
- ✅ **HTTPS-Metadaten deaktiviert** für lokale Entwicklung

### 4. Docker Services

- ✅ **PostgreSQL** (Port 5432) - App-Datenbank
- ✅ **Ollama** (Port 11434) - LLM für Rezeptgenerierung
- ✅ **LocalStack** (Port 4566) - S3 für PDF-Uploads

---

## 🎯 Vollständiger Workflow

### Tägliche Entwicklung

1. **Docker Services starten:**
   ```powershell
   docker compose up -d
   ```

2. **Supabase starten:**
   ```powershell
   supabase start
   ```

3. **Backend starten:**
   ```powershell
   cd server
   $env:ASPNETCORE_ENVIRONMENT="Local"
   dotnet run --project Ernaehrbar.Api
   ```

4. **Frontend starten:**
   ```powershell
   cd client
   pnpm dev
   ```

### Testen

- **Frontend:** http://localhost:5173
- **Backend API:** http://localhost:5000
- **Swagger:** http://localhost:5000/swagger
- **Supabase Studio:** http://127.0.0.1:54323
- **Inbucket (Email):** http://127.0.0.1:54324

---

## ✅ Funktioniert jetzt

- ✅ **Registrierung:** Neue Nutzer können sich registrieren
- ✅ **Login:** Nutzer können sich anmelden
- ✅ **JWT Token:** Werden automatisch im Frontend verwaltet
- ✅ **Backend-Auth:** Backend validiert JWT Tokens von lokaler Supabase
- ✅ **API-Requests:** Frontend sendet JWT Token mit API-Requests

---

## 📋 Nächste Schritte (optional)

### 1. Nutzer in Supabase Studio ansehen

1. Öffne http://127.0.0.1:54323
2. Gehe zu "Authentication" → "Users"
3. Sieh registrierte Nutzer

### 2. Email-Testing

- **Inbucket:** http://127.0.0.1:54324
- Hier siehst du alle E-Mails, die Supabase sendet (z.B. Bestätigungs-E-Mails)

### 3. Datenbank prüfen

- **Supabase Studio:** http://127.0.0.1:54323
- Gehe zu "Table Editor" um Datenbank-Tabellen zu sehen

---

## 🔍 Troubleshooting

### Registrierung funktioniert nicht

1. **Prüfe `.env`:**
   ```powershell
   # In client/
   cat .env
   ```
   Sollte `VITE_SUPABASE_URL` und `VITE_SUPABASE_ANON_KEY` enthalten.

2. **Prüfe Supabase Status:**
   ```powershell
   supabase status
   ```
   Alle Services sollten "Running" sein.

3. **Prüfe Browser-Konsole:**
   - Öffne DevTools (F12)
   - Prüfe auf Fehler in der Console
   - Prüfe Network-Tab für fehlgeschlagene Requests

### Backend validiert JWT nicht

1. **Prüfe `appsettings.Local.json`:**
   - `Supabase:Url` sollte `http://localhost:54321` sein
   - `Supabase:JwksUrl` sollte korrekt sein

2. **Prüfe Environment:**
   ```powershell
   $env:ASPNETCORE_ENVIRONMENT
   ```
   Sollte "Local" sein.

---

## ✅ Status

| Komponente | Status |
|------------|--------|
| Supabase CLI | ✅ Installiert |
| Supabase Services | ✅ Gestartet |
| Frontend `.env` | ✅ Konfiguriert |
| Backend `appsettings.Local.json` | ✅ Konfiguriert |
| Registrierung | ✅ Funktioniert |
| Login | ✅ Funktioniert |
| JWT-Validierung | ✅ Funktioniert |

---

**Status:** Lokales Setup ist vollständig eingerichtet! 🎉

Alle Services laufen lokal und die Authentifizierung funktioniert end-to-end.
