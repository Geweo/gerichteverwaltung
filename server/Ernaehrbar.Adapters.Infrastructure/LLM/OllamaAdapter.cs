using System.Net.Http.Json;
using System.Text.Json;
using Ernaehrbar.Parts.Ports;

namespace Ernaehrbar.Adapters.Infrastructure.LLM;

/// <summary>
/// Infrastructure adapter implementing ILLMService using Ollama.
/// </summary>
public class OllamaAdapter : ILLMService
{
    private readonly HttpClient _httpClient;
    private readonly string _ollamaUrl;
    private readonly string _modelName;

    public OllamaAdapter(HttpClient httpClient, string ollamaUrl = "http://localhost:11434", string modelName = "llama3.2")
    {
        _httpClient = httpClient;
        _ollamaUrl = ollamaUrl.TrimEnd('/');
        _modelName = modelName;
    }

    public async Task<List<GeneratedRecipe>> GenerateRecipesAsync(
        string prompt,
        List<MealCategory> mealCategories,
        int numberOfDays,
        CancellationToken cancellationToken = default)
    {
        var recipes = new List<GeneratedRecipe>();
        var tags = await ExtractTagsFromPromptAsync(prompt, cancellationToken);

        // Generate recipes for each day and meal category
        for (int day = 1; day <= numberOfDays; day++)
        {
            foreach (var category in mealCategories)
            {
                var recipe = await GenerateSingleRecipeAsync(prompt, category, day, tags, cancellationToken);
                recipes.Add(recipe);
            }
        }

        return recipes;
    }

    public async Task<List<string>> ExtractTagsFromPromptAsync(
        string prompt,
        CancellationToken cancellationToken = default)
    {
        var extractionPrompt = $@"Analysiere folgenden Prompt und extrahiere alle relevanten Tags für Rezepte.
Verwende nur diese Tags: vegetarisch, fleischhaltig, vegan, frisch, schnell, gesund, low-carb, high-protein, glutenfrei, laktosefrei, asiatisch, mediterran, italienisch, deutsch.

Prompt: {prompt}

Antworte NUR mit einer kommagetrennten Liste der zutreffenden Tags, keine weiteren Erklärungen.
Beispiel: vegetarisch, frisch, gesund";

        var response = await CallOllamaAsync(extractionPrompt, cancellationToken);
        var tags = response
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(t => t.ToLowerInvariant())
            .ToList();

        return tags;
    }

    public async Task<GeneratedRecipe> RegenerateRecipeAsync(
        string originalPrompt,
        string? newPrompt,
        MealCategory mealCategory,
        List<string> existingTags,
        CancellationToken cancellationToken = default)
    {
        var promptToUse = newPrompt ?? originalPrompt;
        return await GenerateSingleRecipeAsync(promptToUse, mealCategory, 1, existingTags, cancellationToken);
    }

    private async Task<GeneratedRecipe> GenerateSingleRecipeAsync(
        string prompt,
        MealCategory mealCategory,
        int dayNumber,
        List<string> tags,
        CancellationToken cancellationToken)
    {
        var categoryName = mealCategory switch
        {
            MealCategory.Breakfast => "Frühstück",
            MealCategory.Lunch => "Mittagessen",
            MealCategory.Dinner => "Abendessen",
            _ => "Mahlzeit"
        };

        var generationPrompt = $@"Du bist ein professioneller Koch und Rezept-Experte. Erstelle ein reales, kochbares Rezept für {categoryName}.

Benutzerwunsch: {prompt}
Tags: {string.Join(", ", tags)}

WICHTIG: 
- Erstelle NUR ein reales, kochbares Rezept mit echten Zutaten
- Wenn der Benutzerwunsch nicht sinnvoll ist, erstelle trotzdem ein passendes Rezept basierend auf den Tags
- Verwende nur echte, verfügbare Zutaten
- Das Rezept muss tatsächlich kochbar sein

Antworte im folgenden JSON-Format (keine Markdown, nur JSON):
{{
  ""name"": ""Rezeptname"",
  ""description"": ""Kurze Beschreibung des Gerichts"",
  ""ingredients"": [""Zutat 1"", ""Zutat 2"", ...],
  ""tags"": [""tag1"", ""tag2"", ...]
}}";

        var response = await CallOllamaAsync(generationPrompt, cancellationToken);

        // Parse JSON response
        try
        {
            var recipeData = JsonSerializer.Deserialize<RecipeResponse>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (recipeData is null)
            {
                throw new InvalidOperationException("Failed to parse recipe from LLM response");
            }

            return new GeneratedRecipe
            {
                Name = recipeData.Name ?? "Unbenanntes Rezept",
                Description = recipeData.Description ?? string.Empty,
                Ingredients = recipeData.Ingredients ?? new List<string>(),
                Tags = recipeData.Tags ?? tags,
                MealCategory = mealCategory,
                DayNumber = dayNumber,
            };
        }
        catch (JsonException)
        {
            // Fallback: Try to extract from text response
            return CreateFallbackRecipe(prompt, mealCategory, dayNumber, tags);
        }
    }

    private async Task<string> CallOllamaAsync(string prompt, CancellationToken cancellationToken)
    {
        var request = new
        {
            model = _modelName,
            prompt = prompt,
            stream = false,
        };

        var response = await _httpClient.PostAsJsonAsync(
            $"{_ollamaUrl}/api/generate",
            request,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<OllamaResponse>(cancellationToken: cancellationToken);
        return result?.Response ?? string.Empty;
    }

    private GeneratedRecipe CreateFallbackRecipe(
        string prompt,
        MealCategory mealCategory,
        int dayNumber,
        List<string> tags)
    {
        return new GeneratedRecipe
        {
            Name = $"Rezept basierend auf: {prompt[..Math.Min(50, prompt.Length)]}",
            Description = $"Generiertes Rezept für {mealCategory}",
            Ingredients = new List<string> { "Zutaten werden generiert..." },
            Tags = tags,
            MealCategory = mealCategory,
            DayNumber = dayNumber,
        };
    }

    private class OllamaResponse
    {
        public string Response { get; set; } = string.Empty;
    }

    private class RecipeResponse
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<string>? Ingredients { get; set; }
        public List<string>? Tags { get; set; }
    }
}
