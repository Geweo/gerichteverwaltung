# Supabase CLI Setup für lokale Entwicklung

**Datum:** Nach Umstellung auf Supabase CLI

---

## ✅ Warum Supabase CLI?

Die Supabase CLI ist die **offizielle Methode** für lokale Supabase-Entwicklung:

- ✅ Automatische Verwaltung aller Services (Auth, Storage, REST, etc.)
- ✅ Einfache Konfiguration über `ops/local/supabase/config.toml`
- ✅ Automatische Key-Generierung
- ✅ Studio-Integration
- ✅ Keine manuelle Docker Compose-Konfiguration nötig

---

## 🚀 Installation

### Windows

**Option 1: Scoop (empfohlen)**
```powershell
scoop bucket add supabase https://github.com/supabase/scoop-bucket.git
scoop install supabase
```

**Option 2: Chocolatey**
```powershell
choco install supabase
```

**Option 3: Manuell**
1. Lade die neueste Version von [GitHub Releases](https://github.com/supabase/cli/releases)
2. Entpacke die `.zip` Datei
3. Füge den Ordner zu deinem PATH hinzu

**Option 4: npm/pnpm (falls Node.js installiert)**
```bash
pnpm add -g supabase
# oder
npm install -g supabase
```

### Verifikation

```bash
supabase --version
```

---

## 📋 Setup im Projekt

### 1. Supabase Setup

Die Supabase-Konfiguration befindet sich bereits in `ops/local/supabase/`.

### 2. Supabase starten

```bash
cd ops/local/supabase
pnpm install
pnpm run start
```

Oder verwende das Helper-Script im Projektroot:
```bash
./supabase-restart.sh
```

**Erste Ausführung:** Lädt alle Docker-Images herunter (kann einige Minuten dauern).

**Ausgabe:**
```
Started supabase local development setup.

         API URL: http://127.0.0.1:54321
     GraphQL URL: http://127.0.0.1:54321/graphql/v1
          DB URL: postgresql://postgres:postgres@127.0.0.1:54322/postgres
      Studio URL: http://127.0.0.1:54323
    Inbucket URL: http://127.0.0.1:54324
      JWT secret: your-super-secret-jwt-token-with-at-least-32-characters-long
        anon key: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
service_role key: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Wichtig:** Kopiere die **anon key** für das Frontend!

---

### 3. Frontend konfigurieren

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

---

### 4. Backend konfigurieren

Die `appsettings.Local.json` ist bereits konfiguriert:

```json
{
  "Supabase": {
    "Url": "http://localhost:54321",
    "JwksUrl": "http://localhost:54321/auth/v1/.well-known/jwks.json"
  }
}
```

**Wichtig:** Stelle sicher, dass `ASPNETCORE_ENVIRONMENT=Local` gesetzt ist!

---

## 🔍 Nützliche Befehle

### Supabase Status prüfen

```bash
supabase status
```

### Supabase stoppen

```bash
supabase stop
```

### Supabase neu starten

```bash
supabase restart
```

### Logs anzeigen

```bash
supabase logs
```

### Studio öffnen

```bash
# Studio ist verfügbar unter: http://127.0.0.1:54323
# Oder:
supabase studio
```

---

## 📋 Ports

| Service | Port | URL |
|---------|------|-----|
| **API Gateway** | 54321 | `http://127.0.0.1:54321` |
| **PostgreSQL** | 54322 | `postgresql://postgres:postgres@127.0.0.1:54322/postgres` |
| **Studio** | 54323 | `http://127.0.0.1:54323` |
| **Inbucket (Email)** | 54324 | `http://127.0.0.1:54324` |

---

## 🔧 Troubleshooting

### Port bereits belegt

Falls Ports bereits belegt sind, ändere sie in `ops/local/supabase/config.toml`:

```toml
[api]
port = 54321  # Ändere zu einem anderen Port

[db]
port = 54322  # Ändere zu einem anderen Port
```

Dann: `supabase stop` und `supabase start`

### Docker-Container hängen

```bash
# Alle Supabase-Container stoppen
supabase stop

# Falls nötig, manuell:
docker ps | grep supabase
docker stop <container-id>
```

### Daten zurücksetzen

```bash
supabase stop
supabase db reset
supabase start
```

⚠️ **Achtung:** Alle lokalen Daten gehen verloren!

---

## 📝 Workflow

### Tägliche Entwicklung

1. **Docker Services starten:**
   ```bash
   docker compose up -d
   ```

2. **Supabase starten:**
   ```bash
   cd ops/local/supabase
   pnpm install
   pnpm run start
   ```
   
   Oder verwende das Helper-Script:
   ```bash
   ./supabase-restart.sh
   ```

3. **Backend starten:**
   ```bash
   cd server
   $env:ASPNETCORE_ENVIRONMENT="Local"
   dotnet run --project Ernaehrbar.Api
   ```

4. **Frontend starten:**
   ```bash
   cd client
   pnpm dev
   ```

### Am Ende des Tages

```bash
# Optional: Services stoppen
supabase stop
docker compose down
```

---

## ✅ Checkliste

- [ ] Supabase CLI installiert (`supabase --version`)
- [ ] `ops/local/supabase/` Verzeichnis vorhanden
- [ ] `pnpm run start` in `ops/local/supabase/` erfolgreich
- [ ] `client/.env` mit `VITE_SUPABASE_URL` und `VITE_SUPABASE_ANON_KEY` konfiguriert
- [ ] `appsettings.Local.json` vorhanden und korrekt
- [ ] Backend mit `ASPNETCORE_ENVIRONMENT=Local` startet
- [ ] Frontend verbindet sich mit lokaler Supabase

---

## 📚 Weitere Ressourcen

- [Supabase CLI Dokumentation](https://supabase.com/docs/guides/cli)
- [Lokale Entwicklung mit Supabase](https://supabase.com/docs/guides/local-development)
- [Supabase Studio](https://supabase.com/docs/guides/cli/local-development#supabase-studio)

---

**Status:** Supabase CLI ist jetzt die empfohlene Methode für lokale Entwicklung! 🎉
