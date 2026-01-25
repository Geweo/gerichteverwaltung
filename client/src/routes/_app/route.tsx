import { useEffect } from 'react';
import { createFileRoute, Outlet, redirect, useLocation } from '@tanstack/react-router';
import { AppLayout } from '@/components/custom/AppLayout';
import { useAuth } from '@/components/hooks/useAuth';

/**
 * Layout route for authenticated users.
 * All routes under this layout require authentication.
 */
export const Route = createFileRoute('/_app')({
  component: RouteComponent,
  beforeLoad: async ({ context, location }) => {
    // Check authentication status
    // If not authenticated, redirect to login
    // This will be handled by the ProtectedRoute component
  },
});

function RouteComponent() {
  const { user, loading } = useAuth();
  const navigate = Route.useNavigate();
  const { href } = useLocation();

  useEffect(() => {
    if (!loading && !user) {
      navigate({ to: '/login', search: { redirect: href } });
    }
  }, [user, loading, navigate, href]);

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-center">
          <p className="text-muted-foreground">Lädt...</p>
        </div>
      </div>
    );
  }

  if (!user) {
    return null;
  }

  return (
    <AppLayout>
      <Outlet />
    </AppLayout>
  );
}
