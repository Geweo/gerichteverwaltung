import { useState } from 'react';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import type { RecipeCreateInput } from '../types';
import { useCreateRecipe } from '../hooks/useCreateRecipe';

interface RecipeCreateFormProps {
  onSuccess: () => void;
  onCancel: () => void;
}

/**
 * Form for manually creating a recipe.
 */
export function RecipeCreateForm({ onSuccess, onCancel }: RecipeCreateFormProps) {
  const [formData, setFormData] = useState<RecipeCreateInput>({
    name: '',
    description: '',
    instructions: '',
    servings: undefined,
    preparationTimeMinutes: undefined,
    cookingTimeMinutes: undefined,
    mealCategory: undefined,
    source: 'Manual',
    tags: [],
    ingredients: [],
  });

  const createRecipe = useCreateRecipe();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.name.trim()) {
      return;
    }

    try {
      // TODO: Get groupId and createdByUserId from user context/auth
      await createRecipe.mutateAsync({
        ...formData,
        groupId: 1, // Placeholder
        createdByUserId: 1, // Placeholder
      });
      onSuccess();
    } catch (error) {
      console.error('Error creating recipe:', error);
      // Error is handled by mutation
    }
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <div className="space-y-2">
        <label htmlFor="name" className="text-sm font-medium">
          Name *
        </label>
        <Input
          id="name"
          value={formData.name}
          onChange={(e) => setFormData({ ...formData, name: e.target.value })}
          placeholder="z.B. Spaghetti Bolognese"
          required
        />
      </div>

      <div className="space-y-2">
        <label htmlFor="description" className="text-sm font-medium">
          Beschreibung
        </label>
        <Textarea
          id="description"
          value={formData.description}
          onChange={(e) => setFormData({ ...formData, description: e.target.value })}
          placeholder="Kurze Beschreibung..."
        />
      </div>

      <div className="space-y-2">
        <label htmlFor="mealCategory" className="text-sm font-medium">
          Mahlzeit
        </label>
        <Select
          value={formData.mealCategory ?? 'all'}
          onValueChange={(value) =>
            setFormData({
              ...formData,
              mealCategory: value === 'all' ? undefined : (value as RecipeCreateInput['mealCategory']),
            })
          }
        >
          <SelectTrigger id="mealCategory">
            <SelectValue placeholder="Optional" />
          </SelectTrigger>
          <SelectContent>
            <SelectItem value="all">Optional</SelectItem>
            <SelectItem value="Breakfast">Frühstück</SelectItem>
            <SelectItem value="Lunch">Mittag</SelectItem>
            <SelectItem value="Dinner">Abend</SelectItem>
          </SelectContent>
        </Select>
      </div>

        <div className="flex justify-end gap-2 pt-4">
          <Button type="button" variant="outline" onClick={onCancel}>
            Abbrechen
          </Button>
          <Button type="submit" disabled={createRecipe.isPending}>
            {createRecipe.isPending ? 'Erstelle...' : 'Erstellen'}
          </Button>
        </div>
      </form>
  );
}
