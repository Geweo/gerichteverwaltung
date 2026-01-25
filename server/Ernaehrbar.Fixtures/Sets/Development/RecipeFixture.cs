using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Fixtures.Utilities;
using Ernaehrbar.Parts.Domain;
using Ernaehrbar.Parts.Ports;
using Entities = Ernaehrbar.Adapters.Infrastructure.Data.Entities;

namespace Ernaehrbar.Fixtures.Sets.Development;

/// <summary>
/// Fixture for Recipes.
/// </summary>
public class RecipeFixture : SeedableFixture<DevelopmentFixtureSet>
{
    public Recipe SpaghettiBolognese { get; private set; } = null!;
    public Recipe CaesarSalad { get; private set; } = null!;
    public Recipe Pancakes { get; private set; } = null!;

    protected override async Task SeedAsync(CancellationToken cancellationToken)
    {
        var groups = Parent.GroupFixture;
        var tags = Parent.TagFixture;

        // Spaghetti Bolognese
        SpaghettiBolognese = new Recipe
        {
            GroupId = groups.FamilieMueller.Id,
            Name = "Spaghetti Bolognese",
            Description = "Klassische italienische Pasta mit Hackfleischsoße",
            Instructions = "1. Zwiebeln und Knoblauch anbraten\n2. Hackfleisch hinzufügen und anbraten\n3. Tomaten und Gewürze hinzufügen\n4. 30 Minuten köcheln lassen\n5. Mit gekochten Spaghetti servieren",
            Servings = 4,
            PreparationTimeMinutes = 15,
            CookingTimeMinutes = 45,
            Source = RecipeSource.Manual,
            MealCategory = MealCategory.Dinner
        };
        await Context.Recipes.AddAsync(SpaghettiBolognese, cancellationToken);

        // Caesar Salad
        CaesarSalad = new Recipe
        {
            GroupId = groups.WGBerlin.Id,
            Name = "Caesar Salad",
            Description = "Frischer Salat mit Caesar-Dressing",
            Instructions = "1. Romana-Salat waschen und zerkleinern\n2. Dressing zubereiten\n3. Croutons und Parmesan hinzufügen\n4. Alles vermengen",
            Servings = 2,
            PreparationTimeMinutes = 10,
            CookingTimeMinutes = 0,
            Source = RecipeSource.Manual,
            MealCategory = MealCategory.Lunch
        };
        await Context.Recipes.AddAsync(CaesarSalad, cancellationToken);

        // Pancakes
        Pancakes = new Recipe
        {
            GroupId = groups.FamilieMueller.Id,
            Name = "Pancakes",
            Description = "Fluffige amerikanische Pfannkuchen",
            Instructions = "1. Mehl, Eier, Milch und Backpulver vermischen\n2. Teig in Pfanne gießen\n3. Beidseitig goldbraun braten\n4. Mit Ahornsirup servieren",
            Servings = 4,
            PreparationTimeMinutes = 10,
            CookingTimeMinutes = 15,
            Source = RecipeSource.Manual,
            MealCategory = MealCategory.Breakfast
        };
        await Context.Recipes.AddAsync(Pancakes, cancellationToken);

        await Context.SaveChangesAsync(cancellationToken);

        // Add ingredients
        var recipeIngredients = Parent.RecipeIngredientFixture;
        await recipeIngredients.AddIngredient(Context, SpaghettiBolognese, "Spaghetti", 400, "g", 1, cancellationToken);
        await recipeIngredients.AddIngredient(Context, SpaghettiBolognese, "Hackfleisch", 500, "g", 2, cancellationToken);
        await recipeIngredients.AddIngredient(Context, SpaghettiBolognese, "Zwiebeln", 2, "Stück", 3, cancellationToken);
        await recipeIngredients.AddIngredient(Context, SpaghettiBolognese, "Tomaten", 400, "g", 4, cancellationToken);

        await recipeIngredients.AddIngredient(Context, CaesarSalad, "Romana-Salat", 1, "Kopf", 1, cancellationToken);
        await recipeIngredients.AddIngredient(Context, CaesarSalad, "Parmesan", 50, "g", 2, cancellationToken);
        await recipeIngredients.AddIngredient(Context, CaesarSalad, "Croutons", 100, "g", 3, cancellationToken);

        await recipeIngredients.AddIngredient(Context, Pancakes, "Mehl", 200, "g", 1, cancellationToken);
        await recipeIngredients.AddIngredient(Context, Pancakes, "Eier", 2, "Stück", 2, cancellationToken);
        await recipeIngredients.AddIngredient(Context, Pancakes, "Milch", 250, "ml", 3, cancellationToken);

        // Add tags
        var recipeTags = Parent.RecipeTagFixture;
        await recipeTags.AddTag(Context, SpaghettiBolognese, tags.Schnell, cancellationToken);
        await recipeTags.AddTag(Context, CaesarSalad, tags.Vegetarisch, cancellationToken);
        await recipeTags.AddTag(Context, CaesarSalad, tags.Schnell, cancellationToken);
        await recipeTags.AddTag(Context, Pancakes, tags.Vegetarisch, cancellationToken);
        await recipeTags.AddTag(Context, Pancakes, tags.Einfach, cancellationToken);

        // Add nutrition info
        var nutritionInfo = Parent.NutritionInfoFixture;
        await nutritionInfo.AddNutritionInfo(Context, SpaghettiBolognese, 450, 25, 55, 12, 5, 8, 800, cancellationToken);
        await nutritionInfo.AddNutritionInfo(Context, CaesarSalad, 200, 8, 15, 10, 3, 2, 400, cancellationToken);
        await nutritionInfo.AddNutritionInfo(Context, Pancakes, 250, 8, 35, 8, 1, 10, 300, cancellationToken);
    }
}
