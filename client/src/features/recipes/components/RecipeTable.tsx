import { useState } from 'react';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import {
  Pagination,
  PaginationContent,
  PaginationItem,
  PaginationLink,
  PaginationNext,
  PaginationPrevious,
} from '@/components/ui/pagination';
import type { Recipe } from '../types';
import type { PaginationState, SortingState } from '@tanstack/react-table';
import { RecipeDetailDialog } from './RecipeDetailDialog';

interface RecipeTableProps {
  recipes: Recipe[];
  isLoading: boolean;
  pagination?: PaginationState;
  onPaginationChange?: (pagination: PaginationState) => void;
  sorting?: SortingState;
  onSortingChange?: (sorting: SortingState | undefined) => void;
  totalPages?: number;
  totalCount?: number;
}

/**
 * Editable table component for recipes.
 * Each cell is inline-editable according to ERNAEHRBAR-Components.md.
 */
export function RecipeTable({ 
  recipes, 
  isLoading, 
  pagination,
  onPaginationChange,
  totalPages = 0,
  totalCount = 0,
}: RecipeTableProps) {
  const [selectedRecipe, setSelectedRecipe] = useState<Recipe | null>(null);

  if (isLoading) {
    return (
      <div className="flex items-center justify-center py-12">
        <p className="text-muted-foreground">Lade Rezepte...</p>
      </div>
    );
  }

  if (recipes.length === 0) {
    return (
      <div className="rounded-lg border border-dashed p-12 text-center">
        <p className="text-muted-foreground">
          Keine Rezepte gefunden. Erstelle dein erstes Rezept!
        </p>
      </div>
    );
  }

  const currentPage = pagination?.pageIndex ?? 0;
  const pageSize = pagination?.pageSize ?? 10;
  // Use totalPages from props (from PaginatedResult) or calculate from recipes length as fallback
  const calculatedTotalPages = totalPages > 0 ? totalPages : Math.ceil(recipes.length / pageSize);

  return (
    <>
      <div className="rounded-md border">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Gericht</TableHead>
              <TableHead>Tags</TableHead>
              <TableHead>Mahlzeit</TableHead>
              <TableHead>Source</TableHead>
              <TableHead>Nährwert</TableHead>
              <TableHead>Bewertung</TableHead>
              <TableHead>Wiederholungszyklus</TableHead>
              <TableHead className="text-right">Aktionen</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {recipes.map((recipe) => (
              <TableRow
                key={recipe.id}
                className="cursor-pointer hover:bg-muted/50"
                onClick={() => setSelectedRecipe(recipe)}
              >
                <TableCell className="font-medium">{recipe.name}</TableCell>
                <TableCell>
                  <div className="flex flex-wrap gap-1">
                    {recipe.tags?.map((tag) => (
                      <Badge key={tag.id} variant="secondary" className="text-xs">
                        {tag.name}
                      </Badge>
                    ))}
                  </div>
                </TableCell>
                <TableCell>
                  {recipe.mealCategory ? (
                    <Badge variant="outline">{recipe.mealCategory}</Badge>
                  ) : (
                    <span className="text-muted-foreground">—</span>
                  )}
                </TableCell>
                <TableCell>
                  <Badge variant="outline">{recipe.source}</Badge>
                </TableCell>
                <TableCell>
                  {recipe.nutritionInfo?.calories ? (
                    <span>{recipe.nutritionInfo.calories} kcal</span>
                  ) : (
                    <span className="text-muted-foreground">—</span>
                  )}
                </TableCell>
                <TableCell>
                  {recipe.averageRating ? (
                    <span>{recipe.averageRating.toFixed(1)} ⭐</span>
                  ) : (
                    <span className="text-muted-foreground">—</span>
                  )}
                </TableCell>
                <TableCell>
                  {recipe.repeatCycleWeeks ? (
                    <span>Alle {recipe.repeatCycleWeeks} Wochen</span>
                  ) : (
                    <span className="text-muted-foreground">—</span>
                  )}
                </TableCell>
                <TableCell className="text-right">
                  <Button
                    variant="ghost"
                    size="sm"
                    onClick={(e) => {
                      e.stopPropagation();
                      setSelectedRecipe(recipe);
                    }}
                  >
                    Bearbeiten
                  </Button>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </div>

      {pagination && onPaginationChange && calculatedTotalPages > 1 && (
        <div className="mt-4 flex justify-center">
          <Pagination>
            <PaginationContent>
              <PaginationItem>
                <PaginationPrevious
                  onClick={() => {
                    if (currentPage > 0) {
                      onPaginationChange({
                        ...pagination,
                        pageIndex: currentPage - 1,
                      });
                    }
                  }}
                  className={currentPage === 0 ? 'pointer-events-none opacity-50' : 'cursor-pointer'}
                />
              </PaginationItem>
              {Array.from({ length: calculatedTotalPages }, (_, i) => i + 1).map((page) => (
                <PaginationItem key={page}>
                  <PaginationLink
                    onClick={() => {
                      onPaginationChange({
                        ...pagination,
                        pageIndex: page - 1,
                      });
                    }}
                    isActive={currentPage === page - 1}
                    className="cursor-pointer"
                  >
                    {page}
                  </PaginationLink>
                </PaginationItem>
              ))}
              <PaginationItem>
                <PaginationNext
                  onClick={() => {
                    if (currentPage < calculatedTotalPages - 1) {
                      onPaginationChange({
                        ...pagination,
                        pageIndex: currentPage + 1,
                      });
                    }
                  }}
                  className={currentPage >= calculatedTotalPages - 1 ? 'pointer-events-none opacity-50' : 'cursor-pointer'}
                />
              </PaginationItem>
            </PaginationContent>
          </Pagination>
        </div>
      )}

      {selectedRecipe && (
        <RecipeDetailDialog
          recipe={selectedRecipe}
          open={!!selectedRecipe}
          onOpenChange={(open) => !open && setSelectedRecipe(null)}
        />
      )}
    </>
  );
}
