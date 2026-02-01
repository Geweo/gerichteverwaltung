import { useMutation, useQueryClient } from '@tanstack/react-query';
import { customInstance } from '@/lib/api-client';
import { toast } from 'sonner';

interface RateRecipeRequest {
  recipeId: number;
  rating: number; // 1-5, or 0 to remove rating
}

/**
 * Hook to rate a recipe.
 * 
 * TODO: Replace with actual API endpoint when available.
 * Expected endpoint: POST /api/recipes/{id}/rating
 */
export function useRateRecipe() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async ({ recipeId, rating }: RateRecipeRequest): Promise<void> => {
      // TODO: Get userId from auth context
      const userId = 1; // Placeholder

      // TODO: Replace with actual API call
      // const response = await customInstance({
      //   url: `/api/recipes/${recipeId}/rating`,
      //   method: 'POST',
      //   data: {
      //     userId,
      //     rating: rating > 0 ? rating : null, // 0 means remove rating
      //   },
      // });

      // Mock implementation for now
      await new Promise((resolve) => setTimeout(resolve, 200));
      
      if (rating === 0) {
        console.log(`Removing rating for recipe ${recipeId}`);
      } else {
        console.log(`Rating recipe ${recipeId} with ${rating} stars`);
      }
    },
    onSuccess: (_, variables) => {
      // Invalidate recipes query to refetch with updated ratings
      queryClient.invalidateQueries({ queryKey: ['recipes'] });
      
      if (variables.rating === 0) {
        toast.success('Bewertung entfernt');
      } else {
        toast.success(`Mit ${variables.rating} Sternen bewertet`);
      }
    },
    onError: (error) => {
      console.error('Error rating recipe:', error);
      toast.error('Fehler beim Bewerten des Rezepts');
    },
  });
}
