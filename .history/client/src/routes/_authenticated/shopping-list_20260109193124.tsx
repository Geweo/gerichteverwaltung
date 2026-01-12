import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/_authenticated/shopping-list')({
  component: ShoppingList,
});

function ShoppingList()
{
  return (
    <div className="space-y-8">
      <div>
        <h1 className="text-3xl font-bold text-foreground mb-2">
          Einkaufsliste
        </h1>
        <p className="text-muted-foreground">
          Deine Einkaufsliste für Bring.com
        </p>
      </div>

      <div className="bg-card border border-border rounded-lg p-8 text-center">
        <p className="text-muted-foreground">
          Noch keine Einkaufsliste vorhanden. Erstelle eine aus deinem Wochenplan!
        </p>
      </div>
    </div>
  );
}
