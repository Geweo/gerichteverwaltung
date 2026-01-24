# Supabase Quickstart - Schritt für Schritt

**Nach der Installation der Supabase CLI**

---

## ✅ Schritt 1: Supabase starten

Im Projektroot:

```powershell
cd ops/local/supabase
pnpm install
pnpm run start
```

Oder verwende das Helper-Script:
```powershell
./supabase-restart.sh
```

**Erste Ausführung:** Lädt alle Docker-Images herunter (kann 5-10 Minuten dauern).

**Ausgabe:**
```
Started supabase local development setup.

         API URL: http://127.0.0.1:54321
     GraphQL URL: http://127.0.0.1:54321/graphql/v1
          DB URL: postgresql://postgres:postgres@127.0.0.1:54322/postgres
      Studio URL: http://127.0.0.1:54323
    Inbucket URL: http://127.0.0.1:54324
      JWT secret: your-super-secret-jwt-token-with-at-least-32-characters-long
        anon key: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZS1kZW1vIiwicm9sZSI6ImFub24iLCJleHAiOjE5ODM4MTI5OTZ9.CRXP1A7WOeoJeXxjNni43kdQwgnWNReilDMblYTn_I0
service_role key: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZS1kZW1vIiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImV4cCI6MTk4MzgxMjk5Nn0.EGIM96RAZx35lJzdJsyH-qQwv8Hdp7fsn3W0YpN81IU
```

**Wichtig:** Kopiere die **anon key** (die lange Zeile mit `eyJ...`)!

---

## ✅ Schritt 2: Frontend konfigurieren

Erstelle oder aktualisiere `client/.env`:

```env
VITE_SUPABASE_URL=http://127.0.0.1:54321
VITE_SUPABASE_ANON_KEY=<anon key aus supabase start>
```

**Beispiel:**
```env
VITE_SUPABASE_URL=http://127.0.0.1:54321
VITE_SUPABASE_ANON_KEY=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZS1kZW1vIiwicm9sZSI6ImFub24iLCJleHAiOjE5ODM4MTI5OTZ9.CRXP1A7WOeoJeXxjNni43kdQwgnWNReilDMblYTn_I0
```

**Hinweis:** Falls `client/.env` bereits existiert, aktualisiere die Werte.

---

## ✅ Schritt 3: Backend prüfen

Die `appsettings.Local.json` ist bereits konfiguriert:

```json
{
  "Supabase": {
    "Url": "http://localhost:54321",
    "JwksUrl": "http://localhost:54321/auth/v1/.well-known/jwks.json"
  }
}
```

**Wichtig:** Stelle sicher, dass beim Starten des Backends `ASPNETCORE_ENVIRONMENT=Local` gesetzt ist!

---

## ✅ Schritt 4: Services starten

### 1. Docker Services (Postgres, Ollama, LocalStack)

```powershell
docker compose up -d
```

### 2. Supabase (falls noch nicht gestartet)

```powershell
supabase start
```

### 3. Backend starten

```powershell
cd server
$env:ASPNETCORE_ENVIRONMENT="Local"
dotnet run --project Ernaehrbar.Api
```

### 4. Frontend starten

```powershell
cd client
pnpm dev
```

---

## ✅ Schritt 5: Verifikation

### Supabase Studio öffnen

Öffne im Browser: **http://127.0.0.1:54323**

Hier kannst du:
- Nutzer anlegen
- Datenbank-Tabellen ansehen
- Auth-Einstellungen prüfen

### Frontend testen

1. Öffne `http://localhost:5173` (oder den Port, den Vite anzeigt)
2. Versuche dich zu registrieren/anmelden
3. Prüfe, ob die Verbindung zur lokalen Supabase funktioniert

---

## 🔍 Nützliche Befehle

### Supabase Status prüfen

```powershell
supabase status
```

### Supabase stoppen

```powershell
supabase stop
```

### Supabase neu starten

```powershell
supabase restart
```

### Supabase Logs anzeigen

```powershell
supabase logs
```

---

## 📋 Checkliste

- [ ] `supabase start` erfolgreich ausgeführt
- [ ] **anon key** aus der Ausgabe kopiert
- [ ] `client/.env` mit `VITE_SUPABASE_URL` und `VITE_SUPABASE_ANON_KEY` erstellt/aktualisiert
- [ ] `appsettings.Local.json` vorhanden (bereits erledigt)
- [ ] Docker Services gestartet (`docker compose up -d`)
- [ ] Backend mit `ASPNETCORE_ENVIRONMENT=Local` gestartet
- [ ] Frontend gestartet (`pnpm dev`)
- [ ] Supabase Studio geöffnet (http://127.0.0.1:54323)
- [ ] Frontend verbindet sich mit lokaler Supabase

---

## 🎯 Zusammenfassung

1. ✅ **Supabase CLI installiert** (über Scoop)
2. ⏭️ **Supabase starten:** `supabase start`
3. ⏭️ **Frontend konfigurieren:** `client/.env` mit Keys
4. ⏭️ **Services starten:** Docker, Backend, Frontend
5. ⏭️ **Testen:** Studio öffnen, Frontend testen

---

**Status:** Bereit zum Starten! 🚀
