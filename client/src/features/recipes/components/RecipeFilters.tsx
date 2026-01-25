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

interface RecipeFiltersProps {
  search: {
    search?: string;
    mealCategory?: 'Breakfast' | 'Lunch' | 'Dinner';
    source?: 'Manual' | 'Generated' | 'Upload';
    favorites?: boolean;
    tagIds?: number[];
  };
  updateTableFilters: (updates: {
    search?: string;
    mealCategory?: 'Breakfast' | 'Lunch' | 'Dinner' | undefined;
    source?: 'Manual' | 'Generated' | 'Upload' | undefined;
    favorites?: boolean;
    tagIds?: number[];
  }) => void;
}

/**
 * Filter component for recipes.
 * Based on ERNAEHRBAR-Components.md filters:
 * - Mahlzeit (Frühstück / Mittag / Abend)
 * - Tags
 * - Source (Generiert / Upload / Manuell)
 * - Favoriten (vom aktuellen User favorisiert)
 * Filters are managed via URL search params and sent to backend.
 */
export function RecipeFilters({ search, updateTableFilters }: RecipeFiltersProps) {
  return (
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

      {/* TODO: Add Tags filter when Tag API is available */}
    </div>
  );
}
