import { createFileRoute, Outlet } from '@tanstack/react-router';

export const Route = createFileRoute('/_anon/register')({
  component: RouteComponent,
});

function RouteComponent() {
  return <Outlet />;
}
