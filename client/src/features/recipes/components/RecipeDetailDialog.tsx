import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog';
import { Badge } from '@/components/ui/badge';
import type { Recipe } from '../types';

interface RecipeDetailDialogProps {
  recipe: Recipe;
  open: boolean;
  onOpenChange: (open: boolean) => void;
}

/**
 * Detail dialog for viewing and editing a recipe.
 * Based on ERNAEHRBAR-Components.md - "Detailansicht"
 */
export function RecipeDetailDialog({
  recipe,
  open,
  onOpenChange,
}: RecipeDetailDialogProps) {
  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="max-w-3xl max-h-[90vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle>{recipe.name}</DialogTitle>
          <DialogDescription>
            {recipe.description || 'Keine Beschreibung'}
          </DialogDescription>
        </DialogHeader>

        <div className="space-y-6 py-4">
          {/* Tags */}
          {recipe.tags && recipe.tags.length > 0 && (
            <div>
              <h4 className="font-semibold mb-2">Tags</h4>
              <div className="flex flex-wrap gap-2">
                {recipe.tags.map((tag) => (
                  <Badge key={tag.id} variant="secondary">
                    {tag.name}
                  </Badge>
                ))}
              </div>
            </div>
          )}

          {/* Ingredients */}
          {recipe.ingredients && recipe.ingredients.length > 0 && (
            <div>
              <h4 className="font-semibold mb-2">Zutaten</h4>
              <ul className="list-disc list-inside space-y-1">
                {recipe.ingredients.map((ingredient) => (
                  <li key={ingredient.id}>
                    {ingredient.quantity && ingredient.unit
                      ? `${ingredient.quantity} ${ingredient.unit} ${ingredient.name}`
                      : ingredient.name}
                    {ingredient.notes && ` (${ingredient.notes})`}
                  </li>
                ))}
              </ul>
            </div>
          )}

          {/* Instructions */}
          {recipe.instructions && (
            <div>
              <h4 className="font-semibold mb-2">Zubereitung</h4>
              <p className="whitespace-pre-wrap">{recipe.instructions}</p>
            </div>
          )}

          {/* Nutrition Info */}
          {recipe.nutritionInfo && (
            <div>
              <h4 className="font-semibold mb-2">Nährwerte (pro Portion)</h4>
              <div className="grid grid-cols-2 gap-2 text-sm">
                {recipe.nutritionInfo.calories && (
                  <div>Kalorien: {recipe.nutritionInfo.calories} kcal</div>
                )}
                {recipe.nutritionInfo.protein && (
                  <div>Protein: {recipe.nutritionInfo.protein} g</div>
                )}
                {recipe.nutritionInfo.carbohydrates && (
                  <div>Kohlenhydrate: {recipe.nutritionInfo.carbohydrates} g</div>
                )}
                {recipe.nutritionInfo.fat && (
                  <div>Fett: {recipe.nutritionInfo.fat} g</div>
                )}
              </div>
            </div>
          )}

          {/* Metadata */}
          <div className="grid grid-cols-2 gap-4 text-sm text-muted-foreground pt-4 border-t">
            <div>
              <span className="font-medium">Portionen:</span>{' '}
              {recipe.servings ?? '—'}
            </div>
            <div>
              <span className="font-medium">Zubereitung:</span>{' '}
              {recipe.preparationTimeMinutes
                ? `${recipe.preparationTimeMinutes} Min`
                : '—'}
            </div>
            <div>
              <span className="font-medium">Kochzeit:</span>{' '}
              {recipe.cookingTimeMinutes
                ? `${recipe.cookingTimeMinutes} Min`
                : '—'}
            </div>
            <div>
              <span className="font-medium">Source:</span>{' '}
              <Badge variant="outline">{recipe.source}</Badge>
            </div>
          </div>
        </div>
      </DialogContent>
    </Dialog>
  );
}
