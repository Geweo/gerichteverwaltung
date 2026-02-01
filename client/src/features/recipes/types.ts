/**
 * Type definitions for the recipes feature.
 */

export type MealCategory = 'Breakfast' | 'Lunch' | 'Dinner';

export type RecipeSource = 'Manual' | 'Generated' | 'Upload';

/**
 * Paginated result from backend API.
 */
export interface PaginatedResult<T> {
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  items: T[];
}

export interface Recipe {
  id: number;
  name: string;
  description: string | null;
  instructions: string | null;
  servings: number | null;
  preparationTimeMinutes: number | null;
  cookingTimeMinutes: number | null;
  mealCategory: MealCategory | null;
  source: RecipeSource;
  repeatCycleWeeks: number | null;
  imageUrl: string | null;
  pdfUrl: string | null;
  groupId: number;
  createdAt: string;
  updatedAt: string;
  // Related data
  tags?: Tag[];
  ingredients?: RecipeIngredient[];
  nutritionInfo?: NutritionInfo;
  averageRating?: number;
  favoriteCount?: number;
  // User-specific data
  userRating?: number; // Current user's rating (1-5)
  ratingCount?: number; // Total number of ratings
}

export interface RecipeIngredient {
  id: number;
  recipeId: number;
  name: string;
  quantity: number | null;
  unit: string | null;
  notes: string | null;
  order: number;
}

export interface Tag {
  id: number;
  name: string;
  category: string | null;
  groupId: number;
}

export interface NutritionInfo {
  id: number;
  recipeId: number;
  calories: number | null;
  protein: number | null;
  carbohydrates: number | null;
  fat: number | null;
  fiber: number | null;
  sugar: number | null;
  sodium: number | null;
}

export interface RecipeFilters {
  mealCategory?: MealCategory;
  tags: number[]; // Tag IDs
  source?: RecipeSource;
  favorites: boolean; // Only recipes favorited by current user
}

export interface RecipeCreateInput {
  name: string;
  description?: string;
  instructions?: string;
  servings?: number;
  preparationTimeMinutes?: number;
  cookingTimeMinutes?: number;
  mealCategory?: MealCategory;
  source: RecipeSource;
  tags?: number[];
  ingredients?: Omit<RecipeIngredient, 'id' | 'recipeId'>[];
}
