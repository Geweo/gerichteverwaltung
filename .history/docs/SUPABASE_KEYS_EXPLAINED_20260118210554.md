# Supabase Keys erklärt

**Datum:** Nach Supabase CLI Setup

---

## 🔑 Was sind Supabase Keys?

Supabase verwendet **JWT (JSON Web Tokens)** für die Authentifizierung. Es gibt zwei wichtige Keys:

1. **anon key** (öffentlich, für Frontend)
2. **service_role key** (geheim, nur für Backend)

---

## 🔓 Anon Key (öffentlich)

### Was ist das?

Der **anon key** ist ein **öffentlicher Key**, der im Frontend verwendet wird. Er ist dafür gedacht, in Client-seitigem Code (JavaScript/TypeScript) verwendet zu werden.

### Eigenschaften

- ✅ **Öffentlich:** Kann im Frontend-Code verwendet werden
- ✅ **Sicher:** Hat nur eingeschränkte Rechte (Row Level Security)
- ✅ **Für alle:** Jeder kann ihn sehen (z.B. im Browser DevTools)
- ⚠️ **Nicht geheim:** Sollte nicht als "Secret" behandelt werden

### Verwendung

**Im Frontend (`client/.env`):**
```env
VITE_SUPABASE_URL=http://127.0.0.1:54321
VITE_SUPABASE_ANON_KEY=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Im Code:**
```typescript
import { createClient } from '@supabase/supabase-js';

const supabase = createClient(
  import.meta.env.VITE_SUPABASE_URL,
  import.meta.env.VITE_SUPABASE_ANON_KEY  // ← Anon Key
);
```

### Was kann der anon key?

- ✅ Authentifizierte Requests senden (mit JWT Token)
- ✅ Auf Daten zugreifen, die durch **Row Level Security (RLS)** erlaubt sind
- ✅ Auf öffentliche Daten zugreifen
- ❌ **Nicht:** Auf geschützte Daten ohne gültiges JWT Token
- ❌ **Nicht:** Admin-Operationen durchführen

### Sicherheit

Der anon key ist **sicher**, weil:
- Supabase verwendet **Row Level Security (RLS)** in der Datenbank
- Jeder Request benötigt ein gültiges **JWT Token** vom Benutzer
- Der anon key allein gibt keine Admin-Rechte

**Beispiel:**
```sql
-- RLS Policy: Nur eigene Daten sehen
CREATE POLICY "Users can only see their own data"
ON recipes FOR SELECT
USING (auth.uid() = user_id);
```

Auch mit dem anon key kann ein Benutzer nur seine eigenen Rezepte sehen, nicht die von anderen.

---

## 🔐 Service Role Key (geheim)

### Was ist das?

Der **service_role key** ist ein **geheimer Key** mit **Admin-Rechten**. Er sollte **niemals** im Frontend verwendet werden!

### Eigenschaften

- ❌ **Geheim:** Niemals im Frontend verwenden!
- ⚠️ **Admin-Rechte:** Umgeht Row Level Security
- ✅ **Nur Backend:** Sollte nur in serverseitigem Code verwendet werden
- 🔒 **Sicher aufbewahren:** Wie ein Passwort behandeln

### Verwendung

**Nur im Backend (z.B. für Admin-Operationen):**
```csharp
// Beispiel: Backend-Service mit Admin-Rechten
var supabaseAdmin = new SupabaseClient(
    supabaseUrl,
    serviceRoleKey  // ← Service Role Key (NICHT im Frontend!)
);
```

### Was kann der service_role key?

- ✅ **Alle** Daten lesen/schreiben (umgeht RLS)
- ✅ Admin-Operationen durchführen
- ✅ Benutzer verwalten
- ⚠️ **Gefährlich:** Sollte nur in vertrauenswürdigen Umgebungen verwendet werden

---

## 📋 Vergleich

| Aspekt | Anon Key | Service Role Key |
|--------|----------|------------------|
| **Sichtbarkeit** | Öffentlich | Geheim |
| **Verwendung** | Frontend | Backend |
| **Rechte** | Eingeschränkt (RLS) | Admin (umgeht RLS) |
| **Sicherheit** | Sicher (mit RLS) | Gefährlich (Admin) |
| **Beispiel** | `VITE_SUPABASE_ANON_KEY` | Nur in Backend-Code |

---

## 🎯 In unserem Projekt

### Frontend (`client/.env`)

```env
VITE_SUPABASE_URL=http://127.0.0.1:54321
VITE_SUPABASE_ANON_KEY=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Verwendung:**
- Authentifizierung (Login/Registrierung)
- API-Requests mit JWT Token
- Zugriff auf Daten (mit RLS)

### Backend (`appsettings.Local.json`)

```json
{
  "Supabase": {
    "Url": "http://localhost:54321",
    "JwksUrl": "http://localhost:54321/auth/v1/.well-known/jwks.json"
  }
}
```

**Verwendung:**
- JWT Token validieren
- Benutzer-Informationen aus Token extrahieren
- **Kein service_role key nötig** (wir verwenden JWT-Validierung)

---

## ✅ Zusammenfassung

**Anon Key:**
- 🔓 Öffentlich, für Frontend
- ✅ Sicher durch Row Level Security
- 📱 Wird in `client/.env` gespeichert
- 🎯 Für normale API-Requests mit JWT Token

**Service Role Key:**
- 🔐 Geheim, nur für Backend
- ⚠️ Admin-Rechte (umgeht RLS)
- 🚫 **Niemals** im Frontend verwenden!
- 🎯 Nur für spezielle Admin-Operationen

---

**Fazit:** Der **anon key** ist der Key, den du im Frontend verwendest. Er ist sicher, weil Supabase Row Level Security verwendet. Der **service_role key** ist nur für Backend-Admin-Operationen und sollte nicht im Frontend verwendet werden.
