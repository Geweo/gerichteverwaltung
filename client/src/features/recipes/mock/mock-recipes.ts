import type { Recipe } from '../types';

/**
 * Mock recipes data for development.
 * Based on fixture data from backend.
 * 
 * Pattern: Similar to Zentreo's mock data structure
 */
export const mockRecipes: Recipe[] = [
  {
    id: 1,
    name: 'Spaghetti Bolognese',
    description: 'Klassische italienische Pasta mit Hackfleischsoße',
    instructions: '1. Zwiebeln und Knoblauch anbraten\n2. Hackfleisch hinzufügen und anbraten\n3. Tomaten und Gewürze hinzufügen\n4. 30 Minuten köcheln lassen\n5. Mit gekochten Spaghetti servieren',
    servings: 4,
    preparationTimeMinutes: 15,
    cookingTimeMinutes: 45,
    mealCategory: 'Dinner',
    source: 'Manual',
    repeatCycleWeeks: null,
    imageUrl: null,
    pdfUrl: null,
    groupId: 1,
    createdAt: '2026-01-24T10:00:00Z',
    updatedAt: '2026-01-24T10:00:00Z',
    ingredients: [
      {
        id: 1,
        recipeId: 1,
        name: 'Spaghetti',
        quantity: 400,
        unit: 'g',
        notes: null,
        order: 1,
      },
      {
        id: 2,
        recipeId: 1,
        name: 'Hackfleisch',
        quantity: 500,
        unit: 'g',
        notes: null,
        order: 2,
      },
      {
        id: 3,
        recipeId: 1,
        name: 'Zwiebeln',
        quantity: 2,
        unit: 'Stück',
        notes: null,
        order: 3,
      },
      {
        id: 4,
        recipeId: 1,
        name: 'Tomaten',
        quantity: 400,
        unit: 'g',
        notes: null,
        order: 4,
      },
    ],
    tags: [
      {
        id: 1,
        name: 'Schnell',
        category: null,
        groupId: 1,
      },
    ],
    nutritionInfo: {
      id: 1,
      recipeId: 1,
      calories: 450,
      protein: 25,
      carbohydrates: 55,
      fat: 12,
      fiber: 5,
      sugar: 8,
      sodium: 800,
    },
    averageRating: 4.5,
    favoriteCount: 3,
  },
  {
    id: 2,
    name: 'Caesar Salad',
    description: 'Frischer Salat mit Caesar-Dressing',
    instructions: '1. Romana-Salat waschen und zerkleinern\n2. Dressing zubereiten\n3. Croutons und Parmesan hinzufügen\n4. Alles vermengen',
    servings: 2,
    preparationTimeMinutes: 10,
    cookingTimeMinutes: 0,
    mealCategory: 'Lunch',
    source: 'Manual',
    repeatCycleWeeks: null,
    imageUrl: null,
    pdfUrl: null,
    groupId: 1,
    createdAt: '2026-01-24T10:00:00Z',
    updatedAt: '2026-01-24T10:00:00Z',
    ingredients: [
      {
        id: 5,
        recipeId: 2,
        name: 'Romana-Salat',
        quantity: 1,
        unit: 'Kopf',
        notes: null,
        order: 1,
      },
      {
        id: 6,
        recipeId: 2,
        name: 'Parmesan',
        quantity: 50,
        unit: 'g',
        notes: null,
        order: 2,
      },
      {
        id: 7,
        recipeId: 2,
        name: 'Croutons',
        quantity: 100,
        unit: 'g',
        notes: null,
        order: 3,
      },
    ],
    tags: [
      {
        id: 2,
        name: 'Vegetarisch',
        category: null,
        groupId: 1,
      },
      {
        id: 1,
        name: 'Schnell',
        category: null,
        groupId: 1,
      },
    ],
    nutritionInfo: {
      id: 2,
      recipeId: 2,
      calories: 200,
      protein: 8,
      carbohydrates: 15,
      fat: 10,
      fiber: 3,
      sugar: 2,
      sodium: 400,
    },
    averageRating: 4.2,
    favoriteCount: 2,
  },
  {
    id: 3,
    name: 'Pancakes',
    description: 'Fluffige amerikanische Pfannkuchen',
    instructions: '1. Mehl, Eier, Milch und Backpulver vermischen\n2. Teig in Pfanne gießen\n3. Beidseitig goldbraun braten\n4. Mit Ahornsirup servieren',
    servings: 4,
    preparationTimeMinutes: 10,
    cookingTimeMinutes: 15,
    mealCategory: 'Breakfast',
    source: 'Manual',
    repeatCycleWeeks: 2,
    imageUrl: null,
    pdfUrl: null,
    groupId: 1,
    createdAt: '2026-01-24T10:00:00Z',
    updatedAt: '2026-01-24T10:00:00Z',
    ingredients: [
      {
        id: 8,
        recipeId: 3,
        name: 'Mehl',
        quantity: 200,
        unit: 'g',
        notes: null,
        order: 1,
      },
      {
        id: 9,
        recipeId: 3,
        name: 'Eier',
        quantity: 2,
        unit: 'Stück',
        notes: null,
        order: 2,
      },
      {
        id: 10,
        recipeId: 3,
        name: 'Milch',
        quantity: 250,
        unit: 'ml',
        notes: null,
        order: 3,
      },
    ],
    tags: [
      {
        id: 2,
        name: 'Vegetarisch',
        category: null,
        groupId: 1,
      },
      {
        id: 3,
        name: 'Einfach',
        category: null,
        groupId: 1,
      },
    ],
    nutritionInfo: {
      id: 3,
      recipeId: 3,
      calories: 250,
      protein: 8,
      carbohydrates: 35,
      fat: 8,
      fiber: 1,
      sugar: 10,
      sodium: 300,
    },
    averageRating: 4.8,
    favoriteCount: 5,
  },
];

/**
 * Simulates API delay for realistic development experience.
 */
const simulateDelay = (ms: number = 300): Promise<void> => {
  return new Promise((resolve) => setTimeout(resolve, ms));
};

/**
 * Mock function to fetch recipes with filtering.
 * Mirrors the backend API structure.
 */
export async function fetchMockRecipes(
  groupId: number,
  filters?: {
    tagIds?: number[];
    searchTerm?: string;
    skip?: number;
    take?: number;
  }
): Promise<Recipe[]> {
  await simulateDelay(300);

  let result = [...mockRecipes];

  // Filter by groupId (all mock recipes have groupId: 1)
  result = result.filter((r) => r.groupId === groupId);

  // Filter by tags
  if (filters?.tagIds && filters.tagIds.length > 0) {
    result = result.filter((r) =>
      r.tags?.some((tag) => filters.tagIds!.includes(tag.id))
    );
  }

  // Filter by search term
  if (filters?.searchTerm) {
    const searchLower = filters.searchTerm.toLowerCase();
    result = result.filter(
      (r) =>
        r.name.toLowerCase().includes(searchLower) ||
        r.description?.toLowerCase().includes(searchLower)
    );
  }

  // Apply pagination
  if (filters?.skip !== undefined || filters?.take !== undefined) {
    const skip = filters.skip ?? 0;
    const take = filters.take ?? result.length;
    result = result.slice(skip, skip + take);
  }

  return result;
}

/**
 * Mock function to fetch a single recipe by ID.
 */
export async function fetchMockRecipeById(recipeId: number): Promise<Recipe | null> {
  await simulateDelay(200);

  const recipe = mockRecipes.find((r) => r.id === recipeId);
  return recipe ?? null;
}
