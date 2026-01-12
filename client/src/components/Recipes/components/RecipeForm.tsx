import { useState } from 'react';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Textarea } from '@/components/ui/textarea';
import { Label } from '@/components/ui/label';
import { Alert, AlertDescription } from '@/components/ui/alert';
import type { MealCategory } from '../hooks/useRecipeGeneration';

const MEAL_CATEGORIES = [
  { value: 1 as MealCategory, label: 'Frühstück', icon: '🌅' },
  { value: 2 as MealCategory, label: 'Mittagessen', icon: '☀️' },
  { value: 3 as MealCategory, label: 'Abendessen', icon: '🌙' },
];

interface RecipeFormProps
{
  prompt: string;
  setPrompt: (prompt: string) => void;
  selectedCategories: MealCategory[];
  setSelectedCategories: (categories: MealCategory[]) => void;
  numberOfDays: number;
  setNumberOfDays: (days: number) => void;
  loading: boolean;
  error: string | null;
  onGenerate: () => void;
}

export function RecipeForm({
  prompt,
  setPrompt,
  selectedCategories,
  setSelectedCategories,
  numberOfDays,
  setNumberOfDays,
  loading,
  error,
  onGenerate,
}: RecipeFormProps)
{
  const toggleCategory = (category: MealCategory) =>
  {
    setSelectedCategories(
      selectedCategories.includes(category)
        ? selectedCategories.filter((c) => c !== category)
        : [...selectedCategories, category]
    );
  };

  const calculateRecipeCount = () =>
  {
    return selectedCategories.length * numberOfDays;
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle>Rezepte generieren</CardTitle>
      </CardHeader>
      <CardContent className="space-y-4">
        {/* Prompt Input */}
        <div className="space-y-2">
          <Label htmlFor="prompt">Beschreibe deine Wünsche für die Rezepte</Label>
          <Textarea
            id="prompt"
            value={prompt}
            onChange={(e) => setPrompt(e.target.value)}
            placeholder="z.B. gesunde vegetarische Gerichte mit frischen Zutaten, mediterrane Küche..."
            disabled={loading}
            className="min-h-[100px]"
          />
        </div>

        {/* Meal Categories */}
        <div className="space-y-2">
          <Label>Mahlzeiten-Kategorien</Label>
          <div className="flex flex-wrap gap-2">
            {MEAL_CATEGORIES.map((category) => (
              <Button
                key={category.value}
                type="button"
                variant={selectedCategories.includes(category.value) ? 'default' : 'outline'}
                onClick={() => toggleCategory(category.value)}
                disabled={loading}
                size="sm"
              >
                <span>{category.icon}</span>
                {category.label}
              </Button>
            ))}
          </div>
        </div>

        {/* Number of Days */}
        <div className="space-y-3">
          <div className="flex items-center justify-between">
            <Label htmlFor="days">Anzahl Tage</Label>
            <span className="text-sm font-medium text-muted-foreground">
              {numberOfDays} Tage · {calculateRecipeCount()} Rezepte
            </span>
          </div>
          <input
            id="days"
            type="range"
            min="7"
            max="21"
            value={numberOfDays}
            onChange={(e) => setNumberOfDays(Number.parseInt(e.target.value))}
            disabled={loading}
            className="w-full h-2 bg-secondary rounded-lg appearance-none cursor-pointer accent-primary"
          />
          <div className="flex justify-between text-xs text-muted-foreground">
            <span>7</span>
            <span>21</span>
          </div>
        </div>

        {/* Generate Button */}
        <Button
          onClick={onGenerate}
          disabled={loading || !prompt.trim() || selectedCategories.length === 0}
          className="w-full"
        >
          {loading ? 'Generiere Rezepte...' : 'Rezepte generieren'}
        </Button>

        {/* Error Alert */}
        {error && (
          <Alert variant="destructive">
            <AlertDescription>{error}</AlertDescription>
          </Alert>
        )}
      </CardContent>
    </Card>
  );
}
