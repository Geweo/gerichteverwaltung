# Supabase lokal - Troubleshooting

**Problem:** Anmeldung erfolgt nicht über lokale Supabase

---

## 🔍 Problemdiagnose

### Schritt 1: Prüfe Browser-Konsole

Öffne DevTools (F12) → Console und suche nach:

```
🔍 Environment check: { hasUrl: true, hasKey: true, urlLength: ..., keyLength: ... }
```

**Prüfe:**
- `urlLength` sollte `24` sein (für `http://127.0.0.1:54321`)
- `keyLength` sollte größer als 50 sein

### Schritt 2: Prüfe Network-Tab

1. Öffne DevTools → Network
2. Versuche dich anzumelden
3. Suche nach Requests zu `supabase.co` (Cloud) oder `127.0.0.1:54321` (lokal)

**Erwartet:** Requests sollten zu `http://127.0.0.1:54321` gehen, **nicht** zu `*.supabase.co`

---

## ✅ Lösung

### Problem 1: `.env` enthält noch Cloud-URL

**Prüfe `client/.env`:**

```env
# ❌ FALSCH (Cloud):
VITE_SUPABASE_URL=https://dtvrbnosmonkprwpgoyx.supabase.co

# ✅ RICHTIG (lokal):
VITE_SUPABASE_URL=http://127.0.0.1:54321
VITE_SUPABASE_ANON_KEY=sb_publishable_ACJWlzQHlZjBrEguHvfOxg_3BJgxAaH
```

**Aktion:** Aktualisiere `client/.env` mit den lokalen Werten.

---

### Problem 2: Dev-Server wurde nicht neu gestartet

**Vite lädt `.env` nur beim Start!**

**Lösung:**
1. Stoppe den Dev-Server (Ctrl+C)
2. Starte neu:
   ```powershell
   cd client
   pnpm dev
   ```

---

### Problem 3: Mehrere `.env`-Dateien

Vite lädt `.env`-Dateien in dieser Reihenfolge (höhere Priorität überschreibt):

1. `.env.production` (bei `pnpm build`)
2. `.env.local` (lokal, wird ignoriert von git)
3. `.env.development` (bei `pnpm dev`)
4. `.env` (Standard)

**Prüfe, ob es mehrere gibt:**
```powershell
cd client
Get-ChildItem .env*
```

**Lösung:** Stelle sicher, dass `.env` oder `.env.local` die lokalen Werte enthält.

---

### Problem 4: Browser-Cache

**Lösung:**
1. Hard Refresh: `Ctrl+Shift+R` (Windows) oder `Cmd+Shift+R` (Mac)
2. Oder: DevTools öffnen → Network → "Disable cache" aktivieren

---

## 🔍 Debugging

### 1. Prüfe, welche URL verwendet wird

Füge temporär in `client/src/lib/supabase.ts` hinzu:

```typescript
console.log('🔍 Supabase Config:', {
  url: supabaseUrl,
  key: supabaseAnonKey?.substring(0, 20) + '...',
});
```

**Erwartet:**
```
🔍 Supabase Config: {
  url: "http://127.0.0.1:54321",
  key: "sb_publishable_ACJW..."
}
```

**Falls Cloud-URL angezeigt wird:** `.env` ist falsch oder Dev-Server nicht neu gestartet.

---

### 2. Prüfe Supabase Status

```powershell
supabase status
```

Sollte zeigen, dass alle Services laufen.

---

### 3. Teste Supabase direkt

Öffne im Browser:
- http://127.0.0.1:54321/auth/v1/health

Sollte `{"status":"ok"}` zurückgeben.

---

## ✅ Checkliste

- [ ] `client/.env` enthält `VITE_SUPABASE_URL=http://127.0.0.1:54321`
- [ ] `client/.env` enthält `VITE_SUPABASE_ANON_KEY=sb_publishable_...`
- [ ] Dev-Server wurde **neu gestartet** nach `.env`-Änderung
- [ ] Browser-Konsole zeigt lokale URL (nicht `*.supabase.co`)
- [ ] Network-Tab zeigt Requests zu `127.0.0.1:54321`
- [ ] `supabase status` zeigt alle Services als "Running"

---

## 🎯 Schnelllösung

1. **Stoppe Frontend:**
   ```powershell
   # Im Terminal mit Frontend: Ctrl+C
   ```

2. **Prüfe `.env`:**
   ```powershell
   cd client
   Get-Content .env
   ```
   
   Sollte enthalten:
   ```env
   VITE_SUPABASE_URL=http://127.0.0.1:54321
   VITE_SUPABASE_ANON_KEY=sb_publishable_ACJWlzQHlZjBrEguHvfOxg_3BJgxAaH
   ```

3. **Starte Frontend neu:**
   ```powershell
   pnpm dev
   ```

4. **Prüfe Browser-Konsole:**
   - Öffne DevTools (F12)
   - Prüfe Console für `🔍 Environment check`
   - Prüfe Network-Tab für Requests

---

**Status:** Nach diesen Schritten sollte die Anmeldung über die lokale Supabase erfolgen! ✅
