import { Button } from '@/components/ui/button';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { RecipeCard } from './RecipeCard';
import type { GeneratedRecipe } from '../hooks/useRecipeGeneration';

interface RecipeListProps
{
  recipes: GeneratedRecipe[];
  numberOfDays: number;
  loading: boolean;
  onRegenerateAll: () => void;
  onRegenerateSingle: (recipe: GeneratedRecipe) => void;
}

export function RecipeList({
  recipes,
  numberOfDays,
  loading,
  onRegenerateAll,
  onRegenerateSingle,
}: RecipeListProps)
{
  if (recipes.length === 0)
  {
    return null;
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <h2 className="text-2xl font-bold">Generierte Rezepte ({recipes.length})</h2>
        <Button variant="secondary" onClick={onRegenerateAll} disabled={loading}>
          Alle neu generieren
        </Button>
      </div>

      {/* Group recipes by day */}
      {Array.from({ length: numberOfDays }, (_, dayIndex) => dayIndex + 1).map((day) =>
      {
        const dayRecipes = recipes.filter((r) => r.dayNumber === day);
        if (dayRecipes.length === 0)
        {
          return null;
        }

        return (
          <Card key={day} className="overflow-hidden">
            <CardHeader className="bg-muted/50">
              <CardTitle className="text-xl">Tag {day}</CardTitle>
            </CardHeader>
            <CardContent className="p-6 space-y-4">
              {dayRecipes.map((recipe, index) => (
                <RecipeCard
                  key={`${day}-${recipe.mealCategory}-${index}`}
                  recipe={recipe}
                  loading={loading}
                  onRegenerate={() => onRegenerateSingle(recipe)}
                />
              ))}
            </CardContent>
          </Card>
        );
      })}
    </div>
  );
}
