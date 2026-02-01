import { useForm, useFieldArray } from 'react-hook-form';
import { valibotResolver } from '@hookform/resolvers/valibot';
import * as v from 'valibot';
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
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import { Plus, Trash2 } from 'lucide-react';
import { Label } from '@/components/ui/label';
import type { RecipeCreateInput, MealCategory } from '../types';
import { useCreateRecipe } from '../hooks/useCreateRecipe';
import { toast } from 'sonner';

interface RecipeCreateFormProps {
  onSuccess: () => void;
  onCancel: () => void;
}

// Valibot Schema für die Validierung
const ingredientSchema = v.object({
  name: v.pipe(v.string(), v.minLength(1, 'Zutat muss einen Namen haben')),
  quantity: v.optional(v.pipe(v.number(), v.minValue(0, 'Menge muss positiv sein'))),
  unit: v.optional(v.string()),
  notes: v.optional(v.string()),
});

const recipeFormSchema = v.object({
  name: v.pipe(v.string(), v.minLength(1, 'Name ist erforderlich')),
  description: v.optional(v.string()),
  instructions: v.optional(v.string()),
  servings: v.optional(v.pipe(v.number(), v.minValue(1, 'Portionen muss mindestens 1 sein'))),
  preparationTimeMinutes: v.optional(v.pipe(v.number(), v.minValue(0, 'Vorbereitungszeit muss positiv sein'))),
  cookingTimeMinutes: v.optional(v.pipe(v.number(), v.minValue(0, 'Kochzeit muss positiv sein'))),
  mealCategory: v.optional(v.picklist(['Breakfast', 'Lunch', 'Dinner'] as const)),
  ingredients: v.optional(v.array(ingredientSchema)),
});

type RecipeFormValues = v.InferOutput<typeof recipeFormSchema>;

/**
 * Form for manually creating a recipe using React Hook Form.
 * Based on ERNAEHRBAR-Components.md - "Rezept-Erstellung" - Manuell erstellen
 */
export function RecipeCreateForm({ onSuccess, onCancel }: RecipeCreateFormProps) {
  const createRecipe = useCreateRecipe();

  const form = useForm<RecipeFormValues>({
    resolver: valibotResolver(recipeFormSchema),
    defaultValues: {
      name: '',
      description: '',
      instructions: '',
      servings: undefined,
      preparationTimeMinutes: undefined,
      cookingTimeMinutes: undefined,
      mealCategory: undefined,
      ingredients: [],
    },
  });

  const { fields, append, remove } = useFieldArray({
    control: form.control,
    name: 'ingredients',
  });

  const onSubmit = async (data: RecipeFormValues) => {
    try {
      // TODO: Get groupId and createdByUserId from user context/auth
      const recipeData: RecipeCreateInput & { groupId: number; createdByUserId: number } = {
        name: data.name,
        description: data.description || undefined,
        instructions: data.instructions || undefined,
        servings: data.servings || undefined,
        preparationTimeMinutes: data.preparationTimeMinutes || undefined,
        cookingTimeMinutes: data.cookingTimeMinutes || undefined,
        mealCategory: data.mealCategory as MealCategory | undefined,
        source: 'Manual',
        tags: [],
        ingredients: data.ingredients?.map((ing, index) => ({
          name: ing.name,
          quantity: ing.quantity || null,
          unit: ing.unit || null,
          notes: ing.notes || null,
          order: index,
        })) || [],
        groupId: 1, // Placeholder
        createdByUserId: 1, // Placeholder
      };

      await createRecipe.mutateAsync(recipeData);
      toast.success('Rezept erfolgreich erstellt');
      onSuccess();
    } catch (error) {
      console.error('Error creating recipe:', error);
      toast.error('Fehler beim Erstellen des Rezepts');
    }
  };

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
        {/* Name */}
        <FormField
          control={form.control}
          name="name"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Name *</FormLabel>
              <FormControl>
                <Input
                  placeholder="z.B. Spaghetti Bolognese"
                  {...field}
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        {/* Beschreibung */}
        <FormField
          control={form.control}
          name="description"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Beschreibung</FormLabel>
              <FormControl>
                <Textarea
                  placeholder="Kurze Beschreibung des Gerichts..."
                  className="resize-none"
                  {...field}
                />
              </FormControl>
              <FormDescription>
                Optionale Beschreibung des Gerichts
              </FormDescription>
              <FormMessage />
            </FormItem>
          )}
        />

        {/* Mahlzeit */}
        <FormField
          control={form.control}
          name="mealCategory"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Mahlzeit</FormLabel>
              <Select
                onValueChange={(value) => field.onChange(value === 'all' ? undefined : value)}
                value={field.value ?? 'all'}
              >
                <FormControl>
                  <SelectTrigger>
                    <SelectValue placeholder="Optional" />
                  </SelectTrigger>
                </FormControl>
                <SelectContent>
                  <SelectItem value="all">Optional</SelectItem>
                  <SelectItem value="Breakfast">Frühstück</SelectItem>
                  <SelectItem value="Lunch">Mittag</SelectItem>
                  <SelectItem value="Dinner">Abend</SelectItem>
                </SelectContent>
              </Select>
              <FormDescription>
                Für welche Mahlzeit ist dieses Gericht gedacht?
              </FormDescription>
              <FormMessage />
            </FormItem>
          )}
        />

        {/* Portionen und Zeiten */}
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          <FormField
            control={form.control}
            name="servings"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Portionen</FormLabel>
                <FormControl>
                  <Input
                    type="number"
                    min="1"
                    placeholder="4"
                    {...field}
                    onChange={(e) => field.onChange(e.target.value ? Number(e.target.value) : undefined)}
                    value={field.value ?? ''}
                  />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />

          <FormField
            control={form.control}
            name="preparationTimeMinutes"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Vorbereitungszeit (Min.)</FormLabel>
                <FormControl>
                  <Input
                    type="number"
                    min="0"
                    placeholder="15"
                    {...field}
                    onChange={(e) => field.onChange(e.target.value ? Number(e.target.value) : undefined)}
                    value={field.value ?? ''}
                  />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />

          <FormField
            control={form.control}
            name="cookingTimeMinutes"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Kochzeit (Min.)</FormLabel>
                <FormControl>
                  <Input
                    type="number"
                    min="0"
                    placeholder="30"
                    {...field}
                    onChange={(e) => field.onChange(e.target.value ? Number(e.target.value) : undefined)}
                    value={field.value ?? ''}
                  />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
        </div>

        {/* Zutaten */}
        <div className="space-y-4">
          <div className="flex items-center justify-between">
            <Label className="text-base font-medium">Zutaten</Label>
            <Button
              type="button"
              variant="outline"
              size="sm"
              onClick={() => append({ name: '', quantity: undefined, unit: '', notes: '' })}
            >
              <Plus className="h-4 w-4 mr-2" />
              Zutat hinzufügen
            </Button>
          </div>

          {fields.length === 0 && (
            <p className="text-sm text-muted-foreground">
              Noch keine Zutaten hinzugefügt. Klicke auf "Zutat hinzufügen" um zu beginnen.
            </p>
          )}

          <div className="space-y-3">
            {fields.map((field, index) => (
              <div
                key={field.id}
                className="grid grid-cols-1 md:grid-cols-12 gap-2 p-3 border rounded-lg"
              >
                <FormField
                  control={form.control}
                  name={`ingredients.${index}.name`}
                  render={({ field }) => (
                    <FormItem className="md:col-span-4">
                      <FormControl>
                        <Input
                          placeholder="Zutat (z.B. Tomaten)"
                          {...field}
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />

                <FormField
                  control={form.control}
                  name={`ingredients.${index}.quantity`}
                  render={({ field }) => (
                    <FormItem className="md:col-span-2">
                      <FormControl>
                        <Input
                          type="number"
                          min="0"
                          step="0.1"
                          placeholder="Menge"
                          {...field}
                          onChange={(e) => field.onChange(e.target.value ? Number(e.target.value) : undefined)}
                          value={field.value ?? ''}
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />

                <FormField
                  control={form.control}
                  name={`ingredients.${index}.unit`}
                  render={({ field }) => (
                    <FormItem className="md:col-span-2">
                      <FormControl>
                        <Input
                          placeholder="Einheit (g, ml, Stk.)"
                          {...field}
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />

                <FormField
                  control={form.control}
                  name={`ingredients.${index}.notes`}
                  render={({ field }) => (
                    <FormItem className="md:col-span-3">
                      <FormControl>
                        <Input
                          placeholder="Notizen (optional)"
                          {...field}
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />

                <div className="md:col-span-1 flex items-center justify-end">
                  <Button
                    type="button"
                    variant="ghost"
                    size="icon"
                    onClick={() => remove(index)}
                  >
                    <Trash2 className="h-4 w-4 text-destructive" />
                  </Button>
                </div>
              </div>
            ))}
          </div>
        </div>

        {/* Zubereitung */}
        <FormField
          control={form.control}
          name="instructions"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Zubereitung</FormLabel>
              <FormControl>
                <Textarea
                  placeholder="Schritt-für-Schritt Anleitung..."
                  className="resize-none min-h-[120px]"
                  {...field}
                />
              </FormControl>
              <FormDescription>
                Detaillierte Anleitung zur Zubereitung des Gerichts
              </FormDescription>
              <FormMessage />
            </FormItem>
          )}
        />

        {/* Buttons */}
        <div className="flex justify-end gap-2 pt-4">
          <Button type="button" variant="outline" onClick={onCancel}>
            Abbrechen
          </Button>
          <Button type="submit" disabled={createRecipe.isPending}>
            {createRecipe.isPending ? 'Erstelle...' : 'Rezept erstellen'}
          </Button>
        </div>
      </form>
    </Form>
  );
}
