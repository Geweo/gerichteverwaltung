import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { RecipeCreateInput, Recipe } from '../types';
import { mockRecipes } from '../mock/mock-recipes';

/**
 * Hook to create a new recipe.
 * 
 * Currently uses mock data. Switch to real API by replacing with API call.
 * 
 * Backend endpoint (when ready): POST /api/RecipeDrafts (for manual creation)
 */
export function useCreateRecipe() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (data: RecipeCreateInput & { groupId: number; createdByUserId: number }): Promise<{ id: number }> => {
      // Simulate API delay
      await new Promise((resolve) => setTimeout(resolve, 300));

      // Mock: Create a new recipe with next available ID
      const newId = Math.max(...mockRecipes.map((r) => r.id), 0) + 1;

      // TODO: Replace with real API call
      // const response = await customInstance<{ id: number }>({
      //   url: '/api/RecipeDrafts',
      //   method: 'POST',
      //   data: { ... },
      // });
      // return response;

      return { id: newId };
    },
    onSuccess: () => {
      // Invalidate recipes query to refetch
      queryClient.invalidateQueries({ queryKey: ['recipes'] });
    },
  });
}
