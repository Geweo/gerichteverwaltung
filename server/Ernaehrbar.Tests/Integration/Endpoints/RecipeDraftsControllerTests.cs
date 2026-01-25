using System.Net;
using System.Net.Http.Json;
using Ernaehrbar.Adapters.Infrastructure.Data;
using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Tests.Integration.TestHelpers;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;
using Xunit.Abstractions;
using Entities = Ernaehrbar.Adapters.Infrastructure.Data.Entities;

namespace Ernaehrbar.Tests.Integration.Endpoints;

/// <summary>
/// Integration-Tests für RecipeDraft-Endpoints.
/// </summary>
public class RecipeDraftsControllerTests(
    ITestOutputHelper output,
    CustomWebApplicationFactory<global::Program> factory
) : BaseE2ETest(output, factory)
{
    [Fact]
    public async Task CreateRecipeDraft_WithValidRequest_ReturnsCreated()
    {
        // Arrange - Use fixture data
        var user = Fixtures.UserFixture.MaxMueller;
        var group = Fixtures.GroupFixture.FamilieMueller;
        var client = GetAuthenticatedClientForUser(user);

        var request = new
        {
            GroupId = group.Id,
            CreatedByUserId = user.Id,
            Name = "Test Recipe Draft",
            Source = "Generated",
            Description = "Test Description",
            Instructions = "Test Instructions",
            MealCategory = "Lunch",
            Ingredients = new[]
            {
                new { Name = "Tomaten", Quantity = 500, Unit = "g" },
                new { Name = "Zwiebeln", Quantity = 2, Unit = "Stück" }
            }
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/recipe-drafts", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<CreateRecipeDraftResponse>();
        result.ShouldNotBeNull();
        result.Id.ShouldBeGreaterThan(0);
        result.Status.ShouldBe("Pending");

        // Verify in database
        var draft = await DbContext.RecipeDrafts
            .Include(rd => rd.Ingredients)
            .FirstOrDefaultAsync(rd => rd.Id == result.Id);

        draft.ShouldNotBeNull();
        draft.Name.ShouldBe("Test Recipe Draft");
        draft.Source.ShouldBe(Entities.RecipeSource.Generated);
        draft.Ingredients.Count.ShouldBe(2);
    }

    [Fact]
    public async Task CreateRecipeDraft_WithoutAuthentication_ReturnsUnauthorized()
    {
        // Arrange
        var client = Factory.CreateClient(); // Kein Auth-Header

        var request = new
        {
            GroupId = 1,
            CreatedByUserId = 1,
            Name = "Test Recipe",
            Source = "Generated"
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/recipe-drafts", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ApproveRecipeDraft_WithValidDraft_CreatesRecipe()
    {
        // Arrange - Use fixture data
        var user = Fixtures.UserFixture.MaxMueller;
        var group = Fixtures.GroupFixture.FamilieMueller;
        
        // Use the pending draft from fixtures
        var draft = Fixtures.RecipeDraftFixture.PendingDraft;
        var client = GetAuthenticatedClientForUser(user);

        var approveRequest = new
        {
            ApprovedByUserId = user.Id
        };

        // Act
        var response = await client.PostAsJsonAsync($"/api/recipe-drafts/{draft.Id}/approve", approveRequest);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<ApproveRecipeDraftResponse>();
        result.ShouldNotBeNull();
        result.RecipeId.ShouldBeGreaterThan(0);
        result.DraftId.ShouldBe(draft.Id);

        // Verify draft is approved
        var updatedDraft = await DbContext.RecipeDrafts.FindAsync(draft.Id);
        updatedDraft.ShouldNotBeNull();
        updatedDraft.Status.ShouldBe(Entities.DraftStatus.Approved);
        updatedDraft.ReviewedByUserId.ShouldBe(user.Id);

        // Verify recipe was created
        var recipe = await DbContext.Recipes
            .Include(r => r.Ingredients)
            .FirstOrDefaultAsync(r => r.Id == result.RecipeId);
        recipe.ShouldNotBeNull();
        recipe.Name.ShouldBe(draft.Name); // Recipe name should match draft name ("Lasagne")
        recipe.Ingredients.Count.ShouldBe(2); // PendingDraft has 2 ingredients
    }
}

// Response DTOs (vereinfacht für Tests)
public record CreateRecipeDraftResponse(int Id, string Status);
public record ApproveRecipeDraftResponse(int RecipeId, int DraftId);
