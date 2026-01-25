import { createFileRoute, Outlet } from '@tanstack/react-router';
import { useAuth } from '@/components/hooks/useAuth';
import { useEffect } from 'react';

/**
 * Layout route for anonymous/unauthenticated users.
 * Redirects authenticated users away from these routes.
 */
export const Route = createFileRoute('/_anon')({
  component: RouteComponent,
});

function RouteComponent() {
  const { user } = useAuth();
  const navigate = Route.useNavigate();

  useEffect(() => {
    if (user) {
      navigate({ to: '/dashboard' });
    }
  }, [user, navigate]);

  if (user) {
    return null;
  }

  return (
    <div className="min-h-screen bg-background">
      <Outlet />
    </div>
  );
}
