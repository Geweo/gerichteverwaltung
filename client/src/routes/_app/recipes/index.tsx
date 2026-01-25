import { createFileRoute, stripSearchParams } from '@tanstack/react-router';
import * as v from 'valibot';
import { RecipeDatabase } from '@/features/recipes/components/RecipeDatabase';

const searchSchema = v.object({
  search: v.optional(v.fallback(v.string(), ''), ''),
  mealCategory: v.optional(v.picklist(['Breakfast', 'Lunch', 'Dinner'])),
  source: v.optional(v.picklist(['Manual', 'Generated', 'Upload'])),
  favorites: v.optional(v.fallback(v.boolean(), false), false),
  tagIds: v.optional(v.fallback(v.array(v.number()), []), [] as number[]),
  page: v.optional(v.fallback(v.string(), '1'), '1'),
  pageSize: v.optional(v.fallback(v.string(), '10'), '10'),
  sortBy: v.optional(v.string()),
  sortDirection: v.optional(v.picklist(['asc', 'desc'])),
});

type SearchParams = v.InferOutput<typeof searchSchema>;
const searchSchemaDefaults = v.getDefaults(searchSchema);

export const Route = createFileRoute('/_app/recipes/')({
  component: RouteComponent,
  validateSearch: searchSchema,
  search: {
    middlewares: [stripSearchParams(searchSchemaDefaults)],
  },
});

function RouteComponent() {
  return <RecipeDatabase />;
}
