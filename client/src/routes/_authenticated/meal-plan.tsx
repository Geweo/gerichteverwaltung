import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/_authenticated/meal-plan')({
  component: MealPlan,
});

function MealPlan()
{
  return (
    <div className="space-y-8">
      <div>
        <h1 className="text-3xl font-bold text-foreground mb-2">
          Wochenplan
        </h1>
        <p className="text-muted-foreground">
          Plane deine Mahlzeiten für die Woche
        </p>
      </div>

      <div className="bg-card border border-border rounded-lg p-8 text-center">
        <p className="text-muted-foreground">
          Noch kein Wochenplan erstellt. Erstelle deinen ersten Plan!
        </p>
      </div>
    </div>
  );
}
