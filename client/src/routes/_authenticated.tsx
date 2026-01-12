import { createFileRoute, Outlet } from '@tanstack/react-router';
import { AppLayout } from '@/components/custom/AppLayout';
import { ProtectedRoute } from '@/components/custom/ProtectedRoute';

/**
 * Layout route for authenticated users.
 * All routes under this layout require authentication.
 */
export const Route = createFileRoute('/_authenticated')({
  component: AuthenticatedLayout,
});

function AuthenticatedLayout()
{
  return (
    <ProtectedRoute>
      <AppLayout />
    </ProtectedRoute>
  );
}
