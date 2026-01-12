import { Navigate } from '@tanstack/react-router';
import { useAuth } from '@/components/hooks/useAuth';

interface ProtectedRouteProps
{
  children: React.ReactNode;
}

/**
 * Wrapper component that protects routes for authenticated users only.
 * Redirects to login if user is not authenticated.
 */
export function ProtectedRoute({ children }: ProtectedRouteProps)
{
  const { user, loading } = useAuth();

  if (loading)
  {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-center">
          <p className="text-muted-foreground">Lädt...</p>
        </div>
      </div>
    );
  }

  if (!user)
  {
    return <Navigate to="/login" />;
  }

  return <>{children}</>;
}
