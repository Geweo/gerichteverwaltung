import { Label } from '@/components/ui/label';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { Checkbox } from '@/components/ui/checkbox';
import { Input } from '@/components/ui/input';
import {
  Collapsible,
  CollapsibleContent,
  CollapsibleTrigger,
} from '@/components/ui/collapsible';
import { Button } from '@/components/ui/button';
import { ChevronDown, ChevronUp, X } from 'lucide-react';
import { Badge } from '@/components/ui/badge';
import { StarRating } from './StarRating';
import { useState } from 'react';

interface RecipeFiltersProps {
  search: {
    search?: string;
    mealCategory?: 'Breakfast' | 'Lunch' | 'Dinner';
    source?: 'Manual' | 'Generated' | 'Upload';
    favorites?: boolean;
    tagIds?: number[];
    minRating?: number;
    dietaryType?: ('vegetarisch' | 'vegan' | 'fleisch')[];
    effort?: ('schnell' | 'kurze Vorbereitungszeit' | 'wiederverwendbare Zutaten')[];
    style?: ('gesund' | 'fettig' | 'Fitness' | 'Low Carb' | 'eiweißreich')[];
  };
  updateTableFilters: (updates: {
    search?: string;
    mealCategory?: 'Breakfast' | 'Lunch' | 'Dinner' | undefined;
    source?: 'Manual' | 'Generated' | 'Upload' | undefined;
    favorites?: boolean;
    tagIds?: number[];
    minRating?: number | undefined;
    dietaryType?: ('vegetarisch' | 'vegan' | 'fleisch')[] | undefined;
    effort?: ('schnell' | 'kurze Vorbereitungszeit' | 'wiederverwendbare Zutaten')[] | undefined;
    style?: ('gesund' | 'fettig' | 'Fitness' | 'Low Carb' | 'eiweißreich')[] | undefined;
  }) => void;
}

/**
 * Filter component for recipes.
 * Based on ERNAEHRBAR-Components.md filters:
 * - Mahlzeit (Frühstück / Mittag / Abend)
 * - Tags
 * - Source (Generiert / Upload / Manuell)
 * - Favoriten (vom aktuellen User favorisiert)
 * - Bewertung (minRating)
 * - Ernährungsform, Aufwand, Stil
 * Filters are managed via URL search params and sent to backend.
 */
export function RecipeFilters({ search, updateTableFilters }: RecipeFiltersProps) {
  const [showAdvancedFilters, setShowAdvancedFilters] = useState(false);

  const hasActiveFilters = 
    search.minRating !== undefined ||
    (search.dietaryType && search.dietaryType.length > 0) ||
    (search.effort && search.effort.length > 0) ||
    (search.style && search.style.length > 0);

  const clearFilter = (filterType: 'minRating' | 'dietaryType' | 'effort' | 'style') => {
    updateTableFilters({ [filterType]: undefined });
  };

  return (
    <div className="space-y-4">
      {/* Hauptfilter - immer sichtbar */}
      <div className="flex flex-wrap items-center gap-4 rounded-lg border p-4">
      <div className="flex items-center gap-2">
        <Label htmlFor="search">Suche:</Label>
        <Input
          id="search"
          placeholder="Rezept suchen..."
          value={search.search || ''}
          onChange={(e) =>
            updateTableFilters({
              search: e.target.value || undefined,
            })
          }
          className="w-[200px]"
        />
      </div>

      <div className="flex items-center gap-2">
        <Label htmlFor="meal-category">Mahlzeit:</Label>
        <Select
          value={search.mealCategory ?? 'all'}
          onValueChange={(value) =>
            updateTableFilters({
              mealCategory: value === 'all' ? undefined : (value as 'Breakfast' | 'Lunch' | 'Dinner'),
            })
          }
        >
          <SelectTrigger id="meal-category" className="w-[140px]">
            <SelectValue placeholder="Alle" />
          </SelectTrigger>
          <SelectContent>
            <SelectItem value="all">Alle</SelectItem>
            <SelectItem value="Breakfast">Frühstück</SelectItem>
            <SelectItem value="Lunch">Mittag</SelectItem>
            <SelectItem value="Dinner">Abend</SelectItem>
          </SelectContent>
        </Select>
      </div>

      <div className="flex items-center gap-2">
        <Label htmlFor="source">Source:</Label>
        <Select
          value={search.source ?? 'all'}
          onValueChange={(value) =>
            updateTableFilters({
              source: value === 'all' ? undefined : (value as 'Manual' | 'Generated' | 'Upload'),
            })
          }
        >
          <SelectTrigger id="source" className="w-[140px]">
            <SelectValue placeholder="Alle" />
          </SelectTrigger>
          <SelectContent>
            <SelectItem value="all">Alle</SelectItem>
            <SelectItem value="Manual">Manuell</SelectItem>
            <SelectItem value="Generated">Generiert</SelectItem>
            <SelectItem value="Upload">Upload</SelectItem>
          </SelectContent>
        </Select>
      </div>

      <div className="flex items-center gap-2">
        <Checkbox
          id="favorites"
          checked={search.favorites ?? false}
          onCheckedChange={(checked) =>
            updateTableFilters({
              favorites: checked === true ? true : undefined,
            })
          }
        />
        <Label htmlFor="favorites" className="cursor-pointer">
          Nur Favoriten
        </Label>
      </div>

      {/* Bewertung */}
      <div className="flex items-center gap-2">
        <Label htmlFor="min-rating">Mind. Bewertung:</Label>
        <div className="flex items-center gap-2">
          <StarRating
            rating={search.minRating}
            onRate={(rating) => {
              updateTableFilters({
                minRating: rating > 0 ? rating : undefined,
              });
            }}
            size="sm"
            showAverage={false}
          />
          {search.minRating !== undefined && search.minRating > 0 && (
            <Button
              variant="ghost"
              size="sm"
              className="h-6 w-6 p-0"
              onClick={() => clearFilter('minRating')}
            >
              <X className="h-3 w-3" />
            </Button>
          )}
        </div>
      </div>

      {/* Erweiterte Filter */}
      <Collapsible open={showAdvancedFilters} onOpenChange={setShowAdvancedFilters}>
        <CollapsibleTrigger asChild>
          <Button variant="ghost" size="sm" className="w-full justify-between">
            <span>Erweiterte Filter {hasActiveFilters && '• Aktiv'}</span>
            {showAdvancedFilters ? (
              <ChevronUp className="h-4 w-4" />
            ) : (
              <ChevronDown className="h-4 w-4" />
            )}
          </Button>
        </CollapsibleTrigger>
        <CollapsibleContent className="space-y-4 pt-4">
          {/* Ernährungsform */}
          <div className="space-y-2">
            <Label>Ernährungsform</Label>
            <div className="flex flex-wrap gap-2">
              {['vegetarisch', 'vegan', 'fleisch'].map((type) => {
                const isSelected = search.dietaryType?.includes(type as any);
                return (
                  <Badge
                    key={type}
                    variant={isSelected ? 'default' : 'outline'}
                    className="cursor-pointer"
                    onClick={() => {
                      const current = search.dietaryType || [];
                      const newValue = isSelected
                        ? current.filter((t) => t !== type)
                        : [...current, type as any];
                      updateTableFilters({
                        dietaryType: newValue.length > 0 ? newValue : undefined,
                      });
                    }}
                  >
                    {type}
                    {isSelected && (
                      <X className="ml-1 h-3 w-3" />
                    )}
                  </Badge>
                );
              })}
            </div>
          </div>

          {/* Aufwand */}
          <div className="space-y-2">
            <Label>Aufwand</Label>
            <div className="flex flex-wrap gap-2">
              {['schnell', 'kurze Vorbereitungszeit', 'wiederverwendbare Zutaten'].map((effort) => {
                const isSelected = search.effort?.includes(effort as any);
                return (
                  <Badge
                    key={effort}
                    variant={isSelected ? 'default' : 'outline'}
                    className="cursor-pointer"
                    onClick={() => {
                      const current = search.effort || [];
                      const newValue = isSelected
                        ? current.filter((e) => e !== effort)
                        : [...current, effort as any];
                      updateTableFilters({
                        effort: newValue.length > 0 ? newValue : undefined,
                      });
                    }}
                  >
                    {effort}
                    {isSelected && (
                      <X className="ml-1 h-3 w-3" />
                    )}
                  </Badge>
                );
              })}
            </div>
          </div>

          {/* Stil */}
          <div className="space-y-2">
            <Label>Stil</Label>
            <div className="flex flex-wrap gap-2">
              {['gesund', 'fettig', 'Fitness', 'Low Carb', 'eiweißreich'].map((style) => {
                const isSelected = search.style?.includes(style as any);
                return (
                  <Badge
                    key={style}
                    variant={isSelected ? 'default' : 'outline'}
                    className="cursor-pointer"
                    onClick={() => {
                      const current = search.style || [];
                      const newValue = isSelected
                        ? current.filter((s) => s !== style)
                        : [...current, style as any];
                      updateTableFilters({
                        style: newValue.length > 0 ? newValue : undefined,
                      });
                    }}
                  >
                    {style}
                    {isSelected && (
                      <X className="ml-1 h-3 w-3" />
                    )}
                  </Badge>
                );
              })}
            </div>
          </div>
        </CollapsibleContent>
      </Collapsible>
      </div>

      {/* Aktive Filter anzeigen */}
      {hasActiveFilters && (
        <div className="flex flex-wrap items-center gap-2 rounded-lg border p-2 bg-muted/50">
          <span className="text-sm font-medium">Aktive Filter:</span>
          {search.minRating !== undefined && search.minRating > 0 && (
            <Badge variant="secondary" className="gap-1">
              Mind. {search.minRating} ⭐
              <button
                onClick={() => clearFilter('minRating')}
                className="ml-1 hover:bg-destructive/20 rounded-full p-0.5"
              >
                <X className="h-3 w-3" />
              </button>
            </Badge>
          )}
          {search.dietaryType?.map((type) => (
            <Badge key={type} variant="secondary" className="gap-1">
              {type}
              <button
                onClick={() => {
                  const newValue = search.dietaryType?.filter((t) => t !== type);
                  updateTableFilters({
                    dietaryType: newValue && newValue.length > 0 ? newValue : undefined,
                  });
                }}
                className="ml-1 hover:bg-destructive/20 rounded-full p-0.5"
              >
                <X className="h-3 w-3" />
              </button>
            </Badge>
          ))}
          {search.effort?.map((effort) => (
            <Badge key={effort} variant="secondary" className="gap-1">
              {effort}
              <button
                onClick={() => {
                  const newValue = search.effort?.filter((e) => e !== effort);
                  updateTableFilters({
                    effort: newValue && newValue.length > 0 ? newValue : undefined,
                  });
                }}
                className="ml-1 hover:bg-destructive/20 rounded-full p-0.5"
              >
                <X className="h-3 w-3" />
              </button>
            </Badge>
          ))}
          {search.style?.map((style) => (
            <Badge key={style} variant="secondary" className="gap-1">
              {style}
              <button
                onClick={() => {
                  const newValue = search.style?.filter((s) => s !== style);
                  updateTableFilters({
                    style: newValue && newValue.length > 0 ? newValue : undefined,
                  });
                }}
                className="ml-1 hover:bg-destructive/20 rounded-full p-0.5"
              >
                <X className="h-3 w-3" />
              </button>
            </Badge>
          ))}
        </div>
      )}
    </div>
  );
}
