import { createFileRoute } from '@tanstack/react-router';
import { MealPlan } from '@/features/meal-plan/components/MealPlan';

export const Route = createFileRoute('/_app/meal-plan/')({
  component: RouteComponent,
});

function RouteComponent() {
  return <MealPlan />;
}
