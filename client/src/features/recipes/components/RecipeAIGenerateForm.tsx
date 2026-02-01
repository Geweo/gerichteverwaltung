import { useForm } from 'react-hook-form';
import { valibotResolver } from '@hookform/resolvers/valibot';
import * as v from 'valibot';
import { useState } from 'react';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
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
import { Checkbox } from '@/components/ui/checkbox';
import { Label } from '@/components/ui/label';
import {
  Collapsible,
  CollapsibleContent,
  CollapsibleTrigger,
} from '@/components/ui/collapsible';
import { Loader2, Sparkles, ChevronDown, ChevronUp, Edit2 } from 'lucide-react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import type { MealCategory } from '../types';
import { useGenerateSingleRecipe, type GeneratedRecipe } from '../hooks/useGenerateSingleRecipe';
import { useCreateRecipe } from '../hooks/useCreateRecipe';
import { PromptInputDialog } from './PromptInputDialog';
import { toast } from 'sonner';

interface RecipeAIGenerateFormProps {
  onSuccess: () => void;
  onCancel: () => void;
}

// Valibot Schema für die Validierung
const aiGenerateFormSchema = v.object({
  prompt: v.pipe(v.string(), v.minLength(1, 'Bitte gib einen Prompt ein')),
  mealCategory: v.picklist(['Breakfast', 'Lunch', 'Dinner'] as const),
  dietaryType: v.optional(v.array(v.picklist(['vegetarisch', 'vegan', 'fleisch'] as const))),
  style: v.optional(v.array(v.picklist(['gesund', 'fettig', 'Fitness', 'Low Carb', 'eiweißreich'] as const))),
  effort: v.optional(v.array(v.picklist(['schnell', 'kurze Vorbereitungszeit', 'wiederverwendbare Zutaten'] as const))),
  tags: v.optional(v.string()),
});

type AIGenerateFormValues = v.InferOutput<typeof aiGenerateFormSchema>;

/**
 * Form for generating a recipe using AI.
 * Based on ERNAEHRBAR-Components.md - "KI-Gericht generieren"
 */
export function RecipeAIGenerateForm({ onSuccess, onCancel }: RecipeAIGenerateFormProps) {
  const [generatedRecipe, setGeneratedRecipe] = useState<GeneratedRecipe | null>(null);
  const [isPromptDialogOpen, setIsPromptDialogOpen] = useState(false);
  const [showAdvancedOptions, setShowAdvancedOptions] = useState(false);
  const generateRecipe = useGenerateSingleRecipe();
  const createRecipe = useCreateRecipe();

  const form = useForm<AIGenerateFormValues>({
    resolver: valibotResolver(aiGenerateFormSchema),
    defaultValues: {
      prompt: '',
      mealCategory: 'Lunch',
      dietaryType: [],
      style: [],
      effort: [],
      tags: '',
    },
  });

  const prompt = form.watch('prompt');

  const handlePromptConfirm = (newPrompt: string) => {
    form.setValue('prompt', newPrompt);
  };

  const onSubmit = async (data: AIGenerateFormValues) => {
    try {
      // Build prompt with all parameters
      let fullPrompt = data.prompt;
      
      if (data.dietaryType && data.dietaryType.length > 0) {
        fullPrompt += `, ${data.dietaryType.join(', ')}`;
      }
      
      if (data.style && data.style.length > 0) {
        fullPrompt += `, Stil: ${data.style.join(', ')}`;
      }
      
      if (data.effort && data.effort.length > 0) {
        fullPrompt += `, Aufwand: ${data.effort.join(', ')}`;
      }
      
      if (data.tags) {
        fullPrompt += `, Tags: ${data.tags}`;
      }

      // Build tags array
      const tags: string[] = [];
      if (data.dietaryType) tags.push(...data.dietaryType);
      if (data.style) tags.push(...data.style);
      if (data.effort) tags.push(...data.effort);
      if (data.tags) {
        const customTags = data.tags.split(',').map(t => t.trim()).filter(Boolean);
        tags.push(...customTags);
      }

      const result = await generateRecipe.mutateAsync({
        originalPrompt: fullPrompt,
        mealCategory: data.mealCategory,
        existingTags: tags,
      });

      setGeneratedRecipe(result);
      toast.success('Rezept erfolgreich generiert!');
    } catch (error) {
      console.error('Error generating recipe:', error);
      toast.error('Fehler beim Generieren des Rezepts');
    }
  };

  const handleSaveRecipe = async () => {
    if (!generatedRecipe) return;

    try {
      // TODO: Get groupId and createdByUserId from user context/auth
      await createRecipe.mutateAsync({
        name: generatedRecipe.name,
        description: generatedRecipe.description,
        instructions: undefined, // AI doesn't generate instructions yet
        servings: undefined,
        preparationTimeMinutes: undefined,
        cookingTimeMinutes: undefined,
        mealCategory: generatedRecipe.mealCategory,
        source: 'Generated',
        tags: [], // TODO: Map tags to tag IDs
        ingredients: generatedRecipe.ingredients.map((ing, index) => ({
          name: ing,
          quantity: null,
          unit: null,
          notes: null,
          order: index,
        })),
        groupId: 1, // Placeholder
        createdByUserId: 1, // Placeholder
      });

      toast.success('Rezept erfolgreich gespeichert');
      onSuccess();
    } catch (error) {
      console.error('Error saving recipe:', error);
      toast.error('Fehler beim Speichern des Rezepts');
    }
  };

  const handleRegenerate = () => {
    setGeneratedRecipe(null);
    form.handleSubmit(onSubmit)();
  };

  return (
    <div className="space-y-6">
      <PromptInputDialog
        open={isPromptDialogOpen}
        onOpenChange={setIsPromptDialogOpen}
        onConfirm={handlePromptConfirm}
        initialPrompt={prompt}
      />

      {!generatedRecipe ? (
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
            {/* Prompt / Spracheingabe */}
            <FormField
              control={form.control}
              name="prompt"
              render={({ field }) => (
                <FormItem>
                  <div className="flex items-center justify-between">
                    <FormLabel>Beschreibe dein Wunschgericht *</FormLabel>
                    <Button
                      type="button"
                      variant="ghost"
                      size="sm"
                      onClick={() => setIsPromptDialogOpen(true)}
                    >
                      <Edit2 className="h-4 w-4 mr-2" />
                      Bearbeiten
                    </Button>
                  </div>
                  <FormControl>
                    <div className="relative">
                      <div className="min-h-[80px] p-3 border rounded-md bg-muted/50 text-sm whitespace-pre-wrap">
                        {field.value || (
                          <span className="text-muted-foreground">
                            Klicke auf "Bearbeiten" um dein Wunschgericht zu beschreiben oder nutze die Spracheingabe
                          </span>
                        )}
                      </div>
                    </div>
                  </FormControl>
                  <FormDescription>
                    Beschreibe, was für ein Gericht du dir wünschst. Die KI wird basierend darauf ein Rezept generieren.
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
                  <FormLabel>Mahlzeit *</FormLabel>
                  <Select onValueChange={field.onChange} value={field.value}>
                    <FormControl>
                      <SelectTrigger>
                        <SelectValue placeholder="Wähle eine Mahlzeit" />
                      </SelectTrigger>
                    </FormControl>
                    <SelectContent>
                      <SelectItem value="Breakfast">Frühstück</SelectItem>
                      <SelectItem value="Lunch">Mittag</SelectItem>
                      <SelectItem value="Dinner">Abend</SelectItem>
                    </SelectContent>
                  </Select>
                  <FormMessage />
                </FormItem>
              )}
            />

            {/* Ernährungsform */}
            <FormField
              control={form.control}
              name="dietaryType"
              render={() => (
                <FormItem>
                  <FormLabel>Ernährungsform</FormLabel>
                  <div className="grid grid-cols-3 gap-3">
                    {['vegetarisch', 'vegan', 'fleisch'].map((type) => (
                      <FormField
                        key={type}
                        control={form.control}
                        name="dietaryType"
                        render={({ field }) => {
                          return (
                            <FormItem className="flex flex-row items-start space-x-3 space-y-0 rounded-md border p-3">
                              <FormControl>
                                <Checkbox
                                  checked={field.value?.includes(type as any)}
                                  onCheckedChange={(checked) => {
                                    return checked
                                      ? field.onChange([...(field.value || []), type as any])
                                      : field.onChange(
                                          field.value?.filter((value) => value !== type)
                                        );
                                  }}
                                />
                              </FormControl>
                              <FormLabel className="font-normal cursor-pointer flex-1">
                                {type.charAt(0).toUpperCase() + type.slice(1)}
                              </FormLabel>
                            </FormItem>
                          );
                        }}
                      />
                    ))}
                  </div>
                  <FormDescription>
                    Optional: Wähle eine oder mehrere Ernährungsformen
                  </FormDescription>
                  <FormMessage />
                </FormItem>
              )}
            />

            {/* Erweiterte Optionen */}
            <Collapsible open={showAdvancedOptions} onOpenChange={setShowAdvancedOptions}>
              <CollapsibleTrigger asChild>
                <Button type="button" variant="ghost" className="w-full justify-between">
                  <span>Erweiterte Optionen</span>
                  {showAdvancedOptions ? (
                    <ChevronUp className="h-4 w-4" />
                  ) : (
                    <ChevronDown className="h-4 w-4" />
                  )}
                </Button>
              </CollapsibleTrigger>
              <CollapsibleContent className="space-y-6 pt-4">
                {/* Stil */}
                <FormField
                  control={form.control}
                  name="style"
                  render={() => (
                    <FormItem>
                      <FormLabel>Stil</FormLabel>
                      <div className="grid grid-cols-2 md:grid-cols-3 gap-3">
                        {['gesund', 'fettig', 'Fitness', 'Low Carb', 'eiweißreich'].map((style) => (
                          <FormField
                            key={style}
                            control={form.control}
                            name="style"
                            render={({ field }) => {
                              return (
                                <FormItem className="flex flex-row items-start space-x-3 space-y-0 rounded-md border p-3">
                                  <FormControl>
                                    <Checkbox
                                      checked={field.value?.includes(style as any)}
                                      onCheckedChange={(checked) => {
                                        return checked
                                          ? field.onChange([...(field.value || []), style as any])
                                          : field.onChange(
                                              field.value?.filter((value) => value !== style)
                                            );
                                      }}
                                    />
                                  </FormControl>
                                  <FormLabel className="font-normal cursor-pointer flex-1">
                                    {style}
                                  </FormLabel>
                                </FormItem>
                              );
                            }}
                          />
                        ))}
                      </div>
                      <FormDescription>
                        Optional: Wähle einen oder mehrere Stile
                      </FormDescription>
                      <FormMessage />
                    </FormItem>
                  )}
                />

                {/* Aufwand */}
                <FormField
                  control={form.control}
                  name="effort"
                  render={() => (
                    <FormItem>
                      <FormLabel>Aufwand</FormLabel>
                      <div className="space-y-2">
                        {['schnell', 'kurze Vorbereitungszeit', 'wiederverwendbare Zutaten'].map((effort) => (
                          <FormField
                            key={effort}
                            control={form.control}
                            name="effort"
                            render={({ field }) => {
                              return (
                                <FormItem className="flex flex-row items-start space-x-3 space-y-0 rounded-md border p-3">
                                  <FormControl>
                                    <Checkbox
                                      checked={field.value?.includes(effort as any)}
                                      onCheckedChange={(checked) => {
                                        return checked
                                          ? field.onChange([...(field.value || []), effort as any])
                                          : field.onChange(
                                              field.value?.filter((value) => value !== effort)
                                            );
                                      }}
                                    />
                                  </FormControl>
                                  <FormLabel className="font-normal cursor-pointer flex-1">
                                    {effort}
                                  </FormLabel>
                                </FormItem>
                              );
                            }}
                          />
                        ))}
                      </div>
                      <FormDescription>
                        Optional: Wähle Aufwand-Kriterien
                      </FormDescription>
                      <FormMessage />
                    </FormItem>
                  )}
                />

                {/* Tags */}
                <FormField
                  control={form.control}
                  name="tags"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Zusätzliche Tags</FormLabel>
                      <FormControl>
                        <Input
                          placeholder="z.B. mediterran, asiatisch (kommagetrennt)"
                          {...field}
                        />
                      </FormControl>
                      <FormDescription>
                        Optional: Zusätzliche Tags, kommagetrennt
                      </FormDescription>
                      <FormMessage />
                    </FormItem>
                  )}
                />
              </CollapsibleContent>
            </Collapsible>

            {/* Buttons */}
            <div className="flex justify-end gap-2 pt-4">
              <Button type="button" variant="outline" onClick={onCancel}>
                Abbrechen
              </Button>
              <Button type="submit" disabled={generateRecipe.isPending}>
                {generateRecipe.isPending ? (
                  <>
                    <Loader2 className="h-4 w-4 mr-2 animate-spin" />
                    Generiere...
                  </>
                ) : (
                  <>
                    <Sparkles className="h-4 w-4 mr-2" />
                    Rezept generieren
                  </>
                )}
              </Button>
            </div>
          </form>
        </Form>
      ) : (
        <div className="space-y-4">
          <Card>
            <CardHeader>
              <div className="flex items-center justify-between">
                <div>
                  <CardTitle>{generatedRecipe.name}</CardTitle>
                  <CardDescription>{generatedRecipe.description}</CardDescription>
                </div>
                <Badge variant="secondary">KI-generiert</Badge>
              </div>
            </CardHeader>
            <CardContent className="space-y-4">
              {/* Tags */}
              {generatedRecipe.tags.length > 0 && (
                <div>
                  <Label className="text-sm font-medium mb-2 block">Tags</Label>
                  <div className="flex flex-wrap gap-2">
                    {generatedRecipe.tags.map((tag, index) => (
                      <Badge key={index} variant="outline">
                        {tag}
                      </Badge>
                    ))}
                  </div>
                </div>
              )}

              {/* Zutaten */}
              <div>
                <Label className="text-sm font-medium mb-2 block">Zutaten</Label>
                <ul className="list-disc list-inside space-y-1">
                  {generatedRecipe.ingredients.map((ingredient, index) => (
                    <li key={index} className="text-sm">
                      {ingredient}
                    </li>
                  ))}
                </ul>
              </div>
            </CardContent>
          </Card>

          {/* Action Buttons */}
          <div className="flex justify-end gap-2">
            <Button variant="outline" onClick={() => setGeneratedRecipe(null)}>
              Zurück
            </Button>
            <Button variant="outline" onClick={handleRegenerate} disabled={generateRecipe.isPending}>
              {generateRecipe.isPending ? (
                <>
                  <Loader2 className="h-4 w-4 mr-2 animate-spin" />
                  Regeneriere...
                </>
              ) : (
                'Neu generieren'
              )}
            </Button>
            <Button onClick={handleSaveRecipe} disabled={createRecipe.isPending}>
              {createRecipe.isPending ? 'Speichere...' : 'Rezept speichern'}
            </Button>
          </div>
        </div>
      )}
    </div>
  );
}
