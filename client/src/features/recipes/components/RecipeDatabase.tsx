import { useState } from 'react';
import { Button } from '@/components/ui/button';
import { RecipeTable } from './RecipeTable';
import { RecipeFilters } from './RecipeFilters';
import { RecipeCreateDialog } from './RecipeCreateDialog';
import { useSearchParams } from '@/components/hooks/use-search-params';
import { useTableStateFromUrl } from '@/components/hooks/use-table-state-from-url';
import { useNavigate, useSearch } from '@tanstack/react-router';
import { useGetApiRecipes } from '@/generated/api/recipes/recipes';
import type { Recipe, PaginatedResult } from '../types';

/**
 * Main component for the Recipe Database feature.
 * Displays an editable table of recipes with filtering capabilities.
 * 
 * Based on: ERNAEHRBAR-Components.md - "Rezept- / Gerichtedatenbank"
 */
export function RecipeDatabase() {
  const navigate = useNavigate();
  const search = useSearch({ from: '/_app/recipes/' }) as {
    search?: string;
    mealCategory?: 'Breakfast' | 'Lunch' | 'Dinner';
    source?: 'Manual' | 'Generated' | 'Upload';
    favorites?: boolean;
    tagIds?: number[];
    minRating?: number;
    dietaryType?: ('vegetarisch' | 'vegan' | 'fleisch')[];
    effort?: ('schnell' | 'kurze Vorbereitungszeit' | 'wiederverwendbare Zutaten')[];
    style?: ('gesund' | 'fettig' | 'Fitness' | 'Low Carb' | 'eiweißreich')[];
    page?: string;
    pageSize?: string;
    sortBy?: string;
    sortDirection?: 'asc' | 'desc';
  };
  const [isCreateDialogOpen, setIsCreateDialogOpen] = useState(false);

  // TODO: Get groupId from user context/auth
  // For now, using a placeholder groupId (1)
  const groupId = 1;

  const { updateTableFilters } = useSearchParams({
    search,
    navigate,
  });

  const { pagination, sorting, handlePaginationChange, handleSortingChange } = useTableStateFromUrl({
    search,
    onPaginationChange: (nextPage, nextPageSize) => updateTableFilters({ page: nextPage, pageSize: nextPageSize }),
    onSortingChange: (sortBy, sortDirection) => updateTableFilters({ sortBy, sortDirection }),
  });

  // Build query params for generated API client (direct call like zentreo)
  const params: Record<string, unknown> = {
    groupId,
    page: Number(search.page) || 1,
    pageSize: Number(search.pageSize) || 10,
  };

  if (search.mealCategory) {
    params.mealCategory = search.mealCategory;
  }

  if (search.source) {
    params.source = search.source;
  }

  if (search.favorites) {
    params.favorites = search.favorites;
  }

  if (search.tagIds && search.tagIds.length > 0) {
    params.tagIds = search.tagIds;
  }

  if (search.search) {
    params.searchTerm = search.search;
  }

  if (search.minRating !== undefined && search.minRating !== null) {
    params.minRating = search.minRating;
  }

  // TODO: Convert dietaryType, effort, style to tagIds when Tag API is available
  // For now, we'll pass them as separate params if backend supports them
  // Otherwise, they need to be converted to tagIds based on tag names
  if (search.dietaryType && search.dietaryType.length > 0) {
    // TODO: Map to tagIds when Tag API is available
    // params.dietaryType = search.dietaryType;
  }

  if (search.effort && search.effort.length > 0) {
    // TODO: Map to tagIds when Tag API is available
    // params.effort = search.effort;
  }

  if (search.style && search.style.length > 0) {
    // TODO: Map to tagIds when Tag API is available
    // params.style = search.style;
  }

  if (search.sortBy) {
    params.sortBy = search.sortBy;
  }

  if (search.sortDirection) {
    params.sortDirection = search.sortDirection;
  }

  // Use generated API client hook directly (like zentreo)
  // The hook returns AxiosResponse<PaginatedResult<Recipe>>, so we need to access .data.data
  const query = useGetApiRecipes(params, {
    query: {
      retry: false,
      throwOnError: false,
    },
  });

  // Extract the actual data from AxiosResponse (like zentreo pattern: query.data.data)
  const recipesResult = query.data?.data as PaginatedResult<Recipe> | undefined;
  const recipes = recipesResult?.items ?? [];

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold">Rezept-Datenbank</h1>
          <p className="text-muted-foreground mt-1">
            Zentrale Verwaltung aller Gerichte (manuell + KI)
          </p>
        </div>
        <Button onClick={() => setIsCreateDialogOpen(true)}>
          <span className="mr-2">+</span>
          Neues Rezept
        </Button>
      </div>

      <RecipeFilters search={search} updateTableFilters={updateTableFilters} />

      {query.error && (
        <div className="rounded-md bg-destructive/15 p-4 text-destructive">
          Fehler beim Laden der Rezepte: {query.error.message}
        </div>
      )}

      <RecipeTable
        recipes={recipes}
        isLoading={query.isLoading}
        pagination={pagination}
        onPaginationChange={handlePaginationChange}
        sorting={sorting}
        onSortingChange={handleSortingChange}
        totalPages={recipesResult?.totalPages ?? 0}
        totalCount={recipesResult?.totalCount ?? 0}
      />

      <RecipeCreateDialog
        open={isCreateDialogOpen}
        onOpenChange={setIsCreateDialogOpen}
      />
    </div>
  );
}
