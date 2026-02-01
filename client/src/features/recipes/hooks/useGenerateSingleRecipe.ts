import { useMutation } from '@tanstack/react-query';
import { customInstance } from '@/lib/api-client';
import type { MealCategory } from '../types';

export interface GeneratedRecipe {
  name: string;
  description: string;
  ingredients: string[];
  tags: string[];
  mealCategory: MealCategory;
  dayNumber: number;
}

interface RegenerateRecipeRequest {
  originalPrompt: string;
  newPrompt?: string;
  mealCategory: MealCategory;
  existingTags?: string[];
}

/**
 * Hook to generate a single recipe using AI.
 * Uses the /api/recipes/regenerate endpoint.
 */
export function useGenerateSingleRecipe() {
  return useMutation({
    mutationFn: async (data: RegenerateRecipeRequest): Promise<GeneratedRecipe> => {
      // The backend expects MealCategory as enum (Breakfast=1, Lunch=2, Dinner=3)
      // but we're sending strings, which should work as ASP.NET Core handles enum conversion
      const response = await customInstance<GeneratedRecipe>({
        url: '/api/recipes/regenerate',
        method: 'POST',
        data: {
          originalPrompt: data.originalPrompt,
          newPrompt: data.newPrompt,
          mealCategory: data.mealCategory, // 'Breakfast' | 'Lunch' | 'Dinner'
          existingTags: data.existingTags || [],
        },
      });
      return response;
    },
  });
}
