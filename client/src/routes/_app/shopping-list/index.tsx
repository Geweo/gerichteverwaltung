import { createFileRoute } from '@tanstack/react-router';
import { ShoppingList } from '@/features/shopping-list/components/ShoppingList';

export const Route = createFileRoute('/_app/shopping-list/')({
  component: RouteComponent,
});

function RouteComponent() {
  return <ShoppingList />;
}
