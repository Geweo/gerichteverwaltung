# Orval mit Fetch (kein Axios) ✅

**Datum:** Nach API-Client-Generierung

---

## ✅ Aktuelle Konfiguration

### Status: **Fetch-basiert, kein Axios**

Die Orval-Konfiguration verwendet bereits einen **custom mutator** mit **fetch**, nicht axios:

**`orval.config.ts`:**
```typescript
output: {
  client: 'react-query',
  override: {
    mutator: {
      path: './src/lib/api-client.ts',
      name: 'customInstance',  // ← Fetch-basiert
    },
  },
}
```

**`src/lib/api-client.ts`:**
- Verwendet native `fetch()` API
- Keine axios-Abhängigkeit
- Integriert Supabase JWT Authentication
- Handles Request/Response korrekt

---

## ✅ Bestätigung

- ✅ **Kein axios** in `package.json`
- ✅ **Generierte Clients** verwenden `customInstance` (fetch)
- ✅ **Keine axios-Imports** in generierten Dateien

---

## 🔧 Optional: Explizite Fetch-Konfiguration

Falls gewünscht, kann man explizit `httpClient: 'fetch'` setzen:

```typescript
output: {
  client: 'react-query',
  httpClient: 'fetch',  // ← Explizit fetch verwenden
  override: {
    mutator: {
      path: './src/lib/api-client.ts',
      name: 'customInstance',
    },
  },
}
```

**Hinweis:** Da wir bereits einen custom mutator verwenden, ist diese Option optional, aber macht die Intention explizit.

---

## 📋 Zusammenfassung

| Aspekt | Status |
|--------|--------|
| HTTP Client | ✅ Fetch (native) |
| Axios Dependency | ❌ Nicht vorhanden |
| Custom Mutator | ✅ `customInstance` (fetch-basiert) |
| Authentication | ✅ Supabase JWT integriert |

**Fazit:** Die Konfiguration verwendet bereits fetch, kein axios! ✅
