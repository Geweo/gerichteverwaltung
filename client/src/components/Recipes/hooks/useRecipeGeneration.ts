import { useState } from 'react';
import { customInstance } from '@/lib/api-client';

export type MealCategory = 1 | 2 | 3; // Breakfast, Lunch, Dinner

export interface GeneratedRecipe
{
  name: string;
  description: string;
  ingredients: string[];
  tags: string[];
  mealCategory: MealCategory;
  dayNumber: number;
}

export interface MealPlanResult
{
  prompt: string;
  tags: string[];
  recipes: GeneratedRecipe[];
  mealCategories: MealCategory[];
  numberOfDays: number;
  generatedAt: string;
}

export function useRecipeGeneration()
{
  const [recipes, setRecipes] = useState<GeneratedRecipe[]>([]);
  const [tags, setTags] = useState<string[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [originalPrompt, setOriginalPrompt] = useState('');

  const generateRecipes = async (
    prompt: string,
    mealCategories: MealCategory[],
    numberOfDays: number
  ) =>
  {
    if (!prompt.trim())
    {
      setError('Bitte gib einen Prompt ein.');
      return;
    }

    if (mealCategories.length === 0)
    {
      setError('Bitte wähle mindestens eine Mahlzeiten-Kategorie aus.');
      return;
    }

    setLoading(true);
    setError(null);
    setOriginalPrompt(prompt);

    try
    {
      const result = await customInstance<MealPlanResult>({
        url: '/api/recipes/generate',
        method: 'POST',
        data: {
          prompt,
          mealCategories,
          numberOfDays,
        },
      });

      setTags(result.tags);
      setRecipes(result.recipes);
    }
    catch (err)
    {
      console.error('Error generating recipes:', err);
      setError(err instanceof Error ? err.message : 'Fehler beim Generieren der Rezepte.');
    }
    finally
    {
      setLoading(false);
    }
  };

  const regenerateAll = async (
    mealCategories: MealCategory[],
    numberOfDays: number
  ) =>
  {
    if (!originalPrompt)
    {
      return;
    }

    await generateRecipes(originalPrompt, mealCategories, numberOfDays);
  };

  const regenerateSingle = async (
    recipe: GeneratedRecipe,
    newPrompt?: string
  ) =>
  {
    setLoading(true);
    setError(null);

    try
    {
      const result = await customInstance<GeneratedRecipe>({
        url: '/api/recipes/regenerate',
        method: 'POST',
        data: {
          originalPrompt: originalPrompt || '',
          newPrompt: newPrompt || undefined,
          mealCategory: recipe.mealCategory,
          existingTags: recipe.tags,
        },
      });

      setRecipes((prev) =>
        prev.map((r) =>
          r.dayNumber === recipe.dayNumber && r.mealCategory === recipe.mealCategory
            ? result
            : r
        )
      );
    }
    catch (err)
    {
      console.error('Error regenerating recipe:', err);
      setError(err instanceof Error ? err.message : 'Fehler beim Neugenerieren des Rezepts.');
    }
    finally
    {
      setLoading(false);
    }
  };

  return {
    recipes,
    tags,
    loading,
    error,
    originalPrompt,
    generateRecipes,
    regenerateAll,
    regenerateSingle,
  };
}
