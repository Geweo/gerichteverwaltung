import { Link, useNavigate, Outlet } from '@tanstack/react-router';
import { useAuth } from '@/components/hooks/useAuth';

/**
 * Main application layout with navigation for authenticated users.
 */
export function AppLayout()
{
  const { user, signOut } = useAuth();
  const navigate = useNavigate();

  const handleSignOut = async () =>
  {
    await signOut();
    navigate({ to: '/login' });
  };

  const navigationItems = [
    { to: '/dashboard', label: 'Dashboard', icon: '📊' },
    { to: '/recipes', label: 'Rezepte', icon: '🍳' },
    { to: '/meal-plan', label: 'Wochenplan', icon: '📅' },
    { to: '/shopping-list', label: 'Einkaufsliste', icon: '🛒' },
  ];

  return (
    <div className="min-h-screen bg-background">
      {/* Navigation */}
      <nav className="border-b border-border bg-card">
        <div className="container mx-auto px-4">
          <div className="flex h-16 items-center justify-between">
            <div className="flex items-center gap-8">
              <Link to="/dashboard" className="text-xl font-bold text-foreground">
                Ernährbär
              </Link>
              <div className="hidden md:flex gap-1">
                {navigationItems.map((item) => (
                  <Link
                    key={item.to}
                    to={item.to}
                    className="px-3 py-2 rounded-md text-sm font-medium text-muted-foreground hover:text-foreground hover:bg-accent transition-colors"
                    activeProps={{
                      className: 'text-foreground bg-accent',
                    }}
                  >
                    <span className="mr-2">{item.icon}</span>
                    {item.label}
                  </Link>
                ))}
              </div>
            </div>
            <div className="flex items-center gap-4">
              <span className="text-sm text-muted-foreground hidden sm:inline">
                {user?.email}
              </span>
              <button
                onClick={handleSignOut}
                className="px-4 py-2 text-sm font-medium text-muted-foreground hover:text-foreground transition-colors"
              >
                Abmelden
              </button>
            </div>
          </div>
        </div>
      </nav>

      {/* Mobile Navigation */}
      <div className="md:hidden border-b border-border bg-card">
        <div className="container mx-auto px-4">
          <div className="flex gap-1 overflow-x-auto py-2">
            {navigationItems.map((item) => (
              <Link
                key={item.to}
                to={item.to}
                className="px-3 py-2 rounded-md text-sm font-medium text-muted-foreground hover:text-foreground hover:bg-accent transition-colors whitespace-nowrap"
                activeProps={{
                  className: 'text-foreground bg-accent',
                }}
              >
                <span className="mr-1">{item.icon}</span>
                {item.label}
              </Link>
            ))}
          </div>
        </div>
      </div>

      {/* Main Content */}
      <main className="container mx-auto px-4 py-8">
        <Outlet />
      </main>
    </div>
  );
}
