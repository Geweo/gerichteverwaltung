import { Button } from '@/components/ui/button';
import { Card, CardContent } from '@/components/ui/card';
import type { GeneratedRecipe } from '../hooks/useRecipeGeneration';

const MEAL_CATEGORIES = [
  { value: 1, label: 'Frühstück', icon: '🌅' },
  { value: 2, label: 'Mittagessen', icon: '☀️' },
  { value: 3, label: 'Abendessen', icon: '🌙' },
];

interface RecipeCardProps
{
  recipe: GeneratedRecipe;
  loading: boolean;
  onRegenerate: () => void;
}

export function RecipeCard({ recipe, loading, onRegenerate }: RecipeCardProps)
{
  const mealCategory = MEAL_CATEGORIES.find((c) => c.value === recipe.mealCategory);

  return (
    <Card className="hover:shadow-md transition-shadow">
      <CardContent className="p-6 space-y-4">
        <div className="flex items-start justify-between gap-4">
          <div className="flex-1 space-y-4">
            <div className="flex items-center gap-3">
              <span className="text-2xl">{mealCategory?.icon || '🍽️'}</span>
              <div>
                <h4 className="text-xl font-semibold">{recipe.name}</h4>
                <p className="text-sm text-muted-foreground mt-1">{recipe.description}</p>
              </div>
            </div>
            
            {recipe.tags.length > 0 && (
              <div className="flex flex-wrap gap-2">
                {recipe.tags.map((tag) => (
                  <span
                    key={tag}
                    className="px-2.5 py-1 bg-primary/10 text-primary rounded-full text-xs font-medium"
                  >
                    {tag}
                  </span>
                ))}
              </div>
            )}
            
            {recipe.ingredients.length > 0 && (
              <div>
                <p className="text-sm font-semibold mb-2">Zutaten:</p>
                <ul className="list-disc list-inside text-sm text-muted-foreground space-y-1.5">
                  {recipe.ingredients.map((ingredient, idx) => (
                    <li key={idx}>{ingredient}</li>
                  ))}
                </ul>
              </div>
            )}
          </div>
        </div>
        
        <div className="pt-2 border-t">
          <Button
            onClick={onRegenerate}
            disabled={loading}
            type="button"
            variant="outline"
            size="sm"
            className="w-full"
          >
            {loading ? 'Generiere...' : 'Neu generieren'}
          </Button>
        </div>
      </CardContent>
    </Card>
  );
}
