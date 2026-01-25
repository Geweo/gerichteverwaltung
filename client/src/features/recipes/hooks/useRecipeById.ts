import { useQuery } from '@tanstack/react-query';
import type { Recipe } from '../types';
import { fetchMockRecipeById } from '../mock/mock-recipes';

/**
 * Hook to fetch a single recipe by ID.
 * 
 * Currently uses mock data. Switch to real API by replacing fetchMockRecipeById with API call.
 * 
 * Backend endpoint (when ready): GET /api/Recipes/{id}
 */
export function useRecipeById(recipeId: number) {
  return useQuery({
    queryKey: ['recipes', recipeId],
    queryFn: async (): Promise<Recipe | null> => {
      // Use mock data for now
      // TODO: Replace with real API call when backend is ready
      // try {
      //   const response = await customInstance<RecipeReadModel>({
      //     url: `/api/Recipes/${recipeId}`,
      //     method: 'GET',
      //   });
      //   return mapRecipeReadModelToRecipe(response);
      // } catch (error) {
      //   if (error instanceof Error && error.message.includes('404')) {
      //     return null;
      //   }
      //   throw error;
      // }

      return await fetchMockRecipeById(recipeId);
    },
    enabled: recipeId > 0,
  });
}
