import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/')({
  component: Index,
});

function Index() {
  return (
    <div className="container mx-auto p-8">
      <h1 className="text-4xl font-bold mb-4">Ernährbär</h1>
      <p className="text-muted-foreground">
        Rezept- & Zutatenplaner mit Bring-Anbindung
      </p>
    </div>
  );
}

