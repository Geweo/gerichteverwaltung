import { Link } from '@tanstack/react-router';
import { useAuth } from '@/components/hooks/useAuth';

export function Dashboard()
{
  const { user } = useAuth();

  return (
    <div className="space-y-8">
      <div>
        <h1 className="text-3xl font-bold text-foreground mb-2">
          Dashboard
        </h1>
        <p className="text-muted-foreground">
          Willkommen zurück, {user?.email}!
        </p>
      </div>

      <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
        {/* Stats Cards */}
        <div className="bg-card border border-border rounded-lg p-6 shadow-sm hover:shadow-md transition-shadow">
          <div className="flex items-center justify-between">
            <div className="flex-1">
              <p className="text-sm font-medium text-muted-foreground">
                Gespeicherte Rezepte
              </p>
              <p className="text-2xl font-bold text-foreground mt-2">0</p>
              <p className="text-xs text-muted-foreground mt-1">
                Hochgeladene & gespeicherte Rezepte
              </p>
            </div>
            <div className="text-3xl">🍳</div>
          </div>
        </div>

        <div className="bg-card border border-border rounded-lg p-6 shadow-sm hover:shadow-md transition-shadow">
          <div className="flex items-center justify-between">
            <div className="flex-1">
              <p className="text-sm font-medium text-muted-foreground">
                Aktueller Wochenplan
              </p>
              <p className="text-2xl font-bold text-foreground mt-2">0 / 7</p>
              <p className="text-xs text-muted-foreground mt-1">
                Geplante Tage dieser Woche
              </p>
            </div>
            <div className="text-3xl">📅</div>
          </div>
        </div>

        <div className="bg-card border border-border rounded-lg p-6 shadow-sm hover:shadow-md transition-shadow">
          <div className="flex items-center justify-between">
            <div className="flex-1">
              <p className="text-sm font-medium text-muted-foreground">
                Aktive Einkaufsliste
              </p>
              <p className="text-2xl font-bold text-foreground mt-2">-</p>
              <p className="text-xs text-muted-foreground mt-1">
                Zutaten aus dem Wochenplan
              </p>
            </div>
            <div className="text-3xl">🛒</div>
          </div>
        </div>
      </div>

      {/* Quick Actions */}
      <div className="bg-card border border-border rounded-lg p-6 shadow-sm">
        <h2 className="text-xl font-semibold text-foreground mb-4">
          Schnellaktionen
        </h2>
        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
          <Link
            to="/recipes"
            className="flex items-center gap-3 p-4 border border-border rounded-lg hover:bg-accent transition-colors text-left"
          >
            <span className="text-2xl">🤖</span>
            <div>
              <p className="font-medium text-foreground">Rezepte generieren</p>
              <p className="text-sm text-muted-foreground">
                Mit KI Rezepte erstellen
              </p>
            </div>
          </Link>

          <button className="flex items-center gap-3 p-4 border border-border rounded-lg hover:bg-accent transition-colors text-left">
            <span className="text-2xl">➕</span>
            <div>
              <p className="font-medium text-foreground">Rezept hinzufügen</p>
              <p className="text-sm text-muted-foreground">
                Neues Rezept erstellen
              </p>
            </div>
          </button>

          <button className="flex items-center gap-3 p-4 border border-border rounded-lg hover:bg-accent transition-colors text-left">
            <span className="text-2xl">📅</span>
            <div>
              <p className="font-medium text-foreground">Wochenplan erstellen</p>
              <p className="text-sm text-muted-foreground">
                Plan für diese Woche
              </p>
            </div>
          </button>

          <button className="flex items-center gap-3 p-4 border border-border rounded-lg hover:bg-accent transition-colors text-left">
            <span className="text-2xl">🛒</span>
            <div>
              <p className="font-medium text-foreground">Einkaufsliste</p>
              <p className="text-sm text-muted-foreground">
                Zur Bring-Liste
              </p>
            </div>
          </button>
        </div>
      </div>
    </div>
  );
}
