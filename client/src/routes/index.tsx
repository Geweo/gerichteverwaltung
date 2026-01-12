import { createFileRoute, Link, useNavigate } from '@tanstack/react-router';
import { useAuth } from '@/components/hooks/useAuth';
import { useState } from 'react';
import { supabase } from '@/lib/supabase';

export const Route = createFileRoute('/')({
  component: Index,
});

// Test-User (funktionieren nicht, nur zum Anzeigen)
const testUsers = [
  { email: 'test1@example.com', password: 'test123', name: 'Test User 1' },
  { email: 'test2@example.com', password: 'test456', name: 'Test User 2' },
  { email: 'demo@ernaehrbar.de', password: 'demo123', name: 'Demo User' },
];

// Echter User (bereits registriert)
const realUser = {
  email: 'keilmannm@yahoo.de',
  password: 'duhunt111',
  name: 'Marc Keilmann',
};

function Index()
{
  const navigate = useNavigate();
  const { user, loading } = useAuth();
  const [testLoginLoading, setTestLoginLoading] = useState<string | null>(null);

  // Redirect authenticated users to dashboard
  if (user)
  {
    navigate({ to: '/dashboard' });
    return null;
  }

  if (loading)
  {
    return (
      <div className="container mx-auto p-8">
        <p>Lädt...</p>
      </div>
    );
  }

  const handleTestLogin = async (email: string, password: string) =>
  {
    setTestLoginLoading(email);
    try
    {
      const { error } = await supabase.auth.signInWithPassword({
        email,
        password,
      });

      if (error) throw error;

      navigate({ to: '/' });
    }
    catch (err)
    {
      console.error('Test login failed:', err);
      alert(`Login fehlgeschlagen: ${err instanceof Error ? err.message : 'Unbekannter Fehler'}`);
    }
    finally
    {
      setTestLoginLoading(null);
    }
  };

  return (
    <div className="container mx-auto p-8">
      <div className="flex justify-between items-center mb-8">
        <div>
          <h1 className="text-4xl font-bold mb-4">Ernährbär</h1>
          <p className="text-muted-foreground">
            Rezept- & Zutatenplaner mit Bring-Anbindung
          </p>
        </div>
        <div className="flex gap-4">
          {user ? (
            <>
              <span className="text-sm text-gray-600">
                Angemeldet als: {user.email}
              </span>
              <button
                onClick={() => signOut()}
                className="px-4 py-2 bg-red-600 text-white rounded hover:bg-red-700"
              >
                Abmelden
              </button>
            </>
          ) : (
            <>
              <Link
                to="/login"
                className="px-4 py-2 bg-primary text-primary-foreground rounded-lg hover:opacity-90 transition-opacity"
              >
                Anmelden
              </Link>
              <Link
                to="/register"
                className="px-4 py-2 border border-primary text-primary rounded-lg hover:bg-primary/10 transition-colors"
              >
                Registrieren
              </Link>
            </>
          )}
        </div>
      </div>

      {user ? (
        <div className="mt-8 p-4 bg-green-50 rounded-lg border border-green-200">
          <p className="text-green-800 font-medium">
            ✅ Du bist erfolgreich angemeldet!
          </p>
        </div>
      ) : (
        <div className="mt-8 space-y-6">
          {/* Echter User */}
          <div className="bg-white rounded-lg border border-slate-200 p-6 shadow-sm">
            <h2 className="text-xl font-semibold mb-4 text-slate-900">
              🟢 Echter User (bereits registriert)
            </h2>
            <div className="bg-green-50 border border-green-200 rounded-lg p-4">
              <div className="space-y-2">
                <div>
                  <span className="text-sm font-medium text-green-800">Email:</span>
                  <span className="ml-2 text-green-900">{realUser.email}</span>
                </div>
                <div>
                  <span className="text-sm font-medium text-green-800">Passwort:</span>
                  <span className="ml-2 text-green-900 font-mono">{realUser.password}</span>
                </div>
                <button
                  onClick={() => handleTestLogin(realUser.email, realUser.password)}
                  disabled={testLoginLoading === realUser.email}
                  className="mt-3 px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
                >
                  {testLoginLoading === realUser.email ? 'Wird angemeldet...' : 'Mit diesem User anmelden'}
                </button>
              </div>
            </div>
          </div>

          {/* Test-User */}
          <div className="bg-white rounded-lg border border-slate-200 p-6 shadow-sm">
            <h2 className="text-xl font-semibold mb-4 text-slate-900">
              🧪 Test-User (funktionieren nicht, nur zum Anzeigen)
            </h2>
            <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
              {testUsers.map((testUser) => (
                <div
                  key={testUser.email}
                  className="bg-slate-50 border border-slate-200 rounded-lg p-4"
                >
                  <div className="space-y-2">
                    <div className="text-sm font-medium text-slate-700">
                      {testUser.name}
                    </div>
                    <div className="text-xs text-slate-600">
                      <div>Email: {testUser.email}</div>
                      <div>Passwort: <span className="font-mono">{testUser.password}</span></div>
                    </div>
                    <button
                      onClick={() => handleTestLogin(testUser.email, testUser.password)}
                      disabled={testLoginLoading === testUser.email}
                      className="mt-2 w-full px-3 py-1.5 text-sm bg-slate-200 text-slate-700 rounded hover:bg-slate-300 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
                    >
                      {testLoginLoading === testUser.email ? 'Lädt...' : 'Test Login'}
                    </button>
                  </div>
                </div>
              ))}
            </div>
            <p className="mt-4 text-sm text-slate-500 italic">
              ⚠️ Diese Test-User existieren nicht in Supabase und werden beim Login fehlschlagen.
            </p>
          </div>
        </div>
      )}
    </div>
  );
}

