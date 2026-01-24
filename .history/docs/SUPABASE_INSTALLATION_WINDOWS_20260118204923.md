# Supabase CLI Installation für Windows - Schritt für Schritt

**Problem:** `supabase` wird nicht gefunden

---

## ✅ Lösung: Manuelle Installation

### Schritt 1: Download

1. Öffne: https://github.com/supabase/cli/releases
2. Suche die neueste Version (z.B. `supabase_2.72.8_windows_amd64.zip`)
3. Lade die ZIP-Datei herunter

### Schritt 2: Entpacken

1. Erstelle einen Ordner für Tools (falls noch nicht vorhanden):
   ```
   C:\Tools\
   ```

2. Entpacke die ZIP-Datei in `C:\Tools\supabase\`
   - Die Struktur sollte sein: `C:\Tools\supabase\supabase.exe`

### Schritt 3: PATH hinzufügen

**Methode A: Über GUI (einfach)**

1. Windows-Taste drücken → "Umgebungsvariablen" eingeben
2. "Umgebungsvariablen bearbeiten" öffnen
3. Unter "Benutzervariablen" → "Path" auswählen → "Bearbeiten"
4. "Neu" klicken
5. `C:\Tools\supabase` eingeben
6. Alle Dialoge mit "OK" schließen

**Methode B: Über PowerShell (als Administrator)**

```powershell
# PATH für aktuellen Benutzer hinzufügen
[Environment]::SetEnvironmentVariable(
    "Path",
    [Environment]::GetEnvironmentVariable("Path", "User") + ";C:\Tools\supabase",
    "User"
)
```

### Schritt 4: Neues Terminal öffnen

⚠️ **Wichtig:** Schließe alle PowerShell/Terminal-Fenster und öffne ein **neues Terminal**, damit PATH aktualisiert wird.

### Schritt 5: Verifikation

```powershell
supabase --version
```

Sollte die Version anzeigen, z.B.:
```
supabase version 2.72.8
```

---

## 🔍 Troubleshooting

### "supabase" wird immer noch nicht gefunden

1. **Prüfe, ob die Datei existiert:**
   ```powershell
   Test-Path C:\Tools\supabase\supabase.exe
   ```
   Sollte `True` zurückgeben.

2. **Prüfe PATH:**
   ```powershell
   $env:Path -split ';' | Select-String "supabase"
   ```
   Sollte `C:\Tools\supabase` enthalten.

3. **Falls nicht im PATH:**
   - Öffne ein **neues Terminal** (PATH wird nur beim Start geladen)
   - Oder setze PATH temporär:
     ```powershell
     $env:Path += ";C:\Tools\supabase"
     ```

### Alternative: Temporärer PATH (für diesen Terminal)

Falls du es sofort testen möchtest, ohne PATH zu ändern:

```powershell
# Temporär für diese Session
$env:Path += ";C:\Tools\supabase"

# Dann testen
supabase --version
```

**Hinweis:** Dies funktioniert nur für diese Terminal-Session. Nach dem Schließen musst du es erneut setzen.

---

## ✅ Nach erfolgreicher Installation

```powershell
# Im Projektroot
supabase init  # Falls noch nicht geschehen
supabase start
```

---

## 📝 Schnellreferenz

**Installationspfad:** `C:\Tools\supabase\supabase.exe`  
**PATH-Eintrag:** `C:\Tools\supabase`  
**Verifikation:** `supabase --version`

---

**Status:** Nach der Installation sollte `supabase --version` funktionieren! 🎉
