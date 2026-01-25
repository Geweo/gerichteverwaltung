import { createFileRoute, Outlet } from '@tanstack/react-router';

export const Route = createFileRoute('/_app/shopping-list')({
  component: RouteComponent,
});

function RouteComponent() {
  return <Outlet />;
}
