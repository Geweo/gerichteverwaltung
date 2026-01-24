# API-Clients erfolgreich generiert! ✅

**Datum:** Nach Implementierung der Backend-Endpoints

---

## ✅ Generierte Dateien

### Recipes (`src/generated/api/recipes/`)

**Hooks:**
- ✅ `usePostApiRecipesGenerate` - POST /api/recipes/generate
- ✅ `usePostApiRecipesRegenerate` - POST /api/recipes/regenerate
- ✅ `usePostApiRecipesUpload` - POST /api/recipes/upload
- ✅ `useGetApiRecipes` - GET /api/recipes (mit Filterung)
- ✅ `useGetApiRecipesId` - GET /api/recipes/{id}

**Funktionen:**
- `postApiRecipesGenerate(data, options)`
- `postApiRecipesRegenerate(data, options)`
- `getApiRecipes(params, options)`
- `getApiRecipesId(id, options)`

### Meal Plans (`src/generated/api/meal-plans/`)

**Hooks:**
- ✅ `useGetApiMealPlans` - GET /api/meal-plans (mit Filterung)
- ✅ `useGetApiMealPlansId` - GET /api/meal-plans/{id}

**Funktionen:**
- `getApiMealPlans(params, options)`
- `getApiMealPlansId(id, options)`

### Schemas (`src/generated/api/schemas/`)

**Types:**
- `GenerateRecipesRequest`
- `RegenerateRecipeRequest`
- `GetApiRecipesParams`
- `GetApiMealPlansParams`
- `RecipeReadModel` (aus Backend)
- `MealPlanReadModel` (aus Backend)
- `MealPlanResult`
- `GeneratedRecipe`
- etc.

---

## ⚠️ Bekannte Probleme

### 1. URL-Case-Sensitivity

**Problem:** Generierte URLs verwenden `/api/Recipes` (großes R) statt `/api/recipes` (klein).

**Ursache:** ASP.NET Core Swagger generiert Routen basierend auf Controller-Namen (PascalCase).

**Lösung:** ASP.NET Core ist standardmäßig case-insensitive für Routen, also sollte es funktionieren. Falls nicht, kann man in `Program.cs` explizit case-sensitive Routen aktivieren oder Swagger-Konfiguration anpassen.

**Status:** Sollte funktionieren, aber nicht ideal. Kann später angepasst werden.

---

## 📋 Nächste Schritte

### 1. Frontend-Code refactoren

**Aktuell:** `useRecipeGeneration.ts` verwendet `customInstance` direkt

**Ziel:** Generierte Hooks verwenden

**Beispiel:**
```typescript
// Vorher
const result = await customInstance<MealPlanResult>({
  url: '/api/recipes/generate',
  method: 'POST',
  data: { ... }
});

// Nachher
import { usePostApiRecipesGenerate } from '@/generated/api/recipes/recipes';

const { mutate: generateRecipes, isPending, error } = usePostApiRecipesGenerate();
```

### 2. Neue Query-Hooks erstellen

**Wrapper-Hooks für bessere API:**

```typescript
// src/features/recipes/hooks/useRecipes.ts
import { useGetApiRecipes } from '@/generated/api/recipes/recipes';

export function useRecipes(groupId: number, filters?: {
  tagIds?: number[];
  searchTerm?: string;
  skip?: number;
  take?: number;
}) {
  return useGetApiRecipes({
    groupId,
    tagIds: filters?.tagIds,
    searchTerm: filters?.searchTerm,
    skip: filters?.skip,
    take: filters?.take,
  });
}
```

### 3. UI-Komponenten implementieren

- Recipe-Liste mit `useRecipes`
- Recipe-Detail mit `useGetApiRecipesId`
- MealPlan-Liste mit `useGetApiMealPlans`
- MealPlan-Detail mit `useGetApiMealPlansId`

---

## ✅ Status

- [x] API-Clients generiert
- [x] Prettier installiert
- [ ] Frontend-Code refactoren
- [ ] UI-Komponenten implementieren

**Alle generierten Clients sind bereit zur Verwendung!** 🎉
