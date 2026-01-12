import { useState } from 'react';
import { RecipeForm } from './components/RecipeForm';
import { TagList } from './components/TagList';
import { RecipeList } from './components/RecipeList';
import { useRecipeGeneration, type MealCategory } from './hooks/useRecipeGeneration';

export function Recipes()
{
  const [prompt, setPrompt] = useState('');
  const [selectedCategories, setSelectedCategories] = useState<MealCategory[]>([1, 2, 3]);
  const [numberOfDays, setNumberOfDays] = useState(7);

  const {
    recipes,
    tags,
    loading,
    error,
    generateRecipes,
    regenerateAll,
    regenerateSingle,
  } = useRecipeGeneration();

  const handleGenerate = () =>
  {
    generateRecipes(prompt, selectedCategories, numberOfDays);
  };

  const handleRegenerateAll = () =>
  {
    regenerateAll(selectedCategories, numberOfDays);
  };

  const handleRegenerateSingle = (recipe: typeof recipes[0]) =>
  {
    regenerateSingle(recipe);
  };

  return (
    <div className="space-y-8">
      <div>
        <h1 className="text-3xl font-bold mb-2">Rezepte generieren</h1>
        <p className="text-muted-foreground">
          Erstelle personalisierte Rezepte mit KI-Unterstützung
        </p>
      </div>

      <RecipeForm
        prompt={prompt}
        setPrompt={setPrompt}
        selectedCategories={selectedCategories}
        setSelectedCategories={setSelectedCategories}
        numberOfDays={numberOfDays}
        setNumberOfDays={setNumberOfDays}
        loading={loading}
        error={error}
        onGenerate={handleGenerate}
      />

      <TagList tags={tags} />

      <RecipeList
        recipes={recipes}
        numberOfDays={numberOfDays}
        loading={loading}
        onRegenerateAll={handleRegenerateAll}
        onRegenerateSingle={handleRegenerateSingle}
      />
    </div>
  );
}
