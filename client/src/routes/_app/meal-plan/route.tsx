import { createFileRoute, Outlet } from '@tanstack/react-router';

export const Route = createFileRoute('/_app/meal-plan')({
  component: RouteComponent,
});

function RouteComponent() {
  return <Outlet />;
}
