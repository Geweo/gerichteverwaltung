import { createFileRoute } from '@tanstack/react-router';
import { Recipes } from '@/components/Recipes/Recipes';

export const Route = createFileRoute('/_authenticated/recipes')({
  component: Recipes,
});
