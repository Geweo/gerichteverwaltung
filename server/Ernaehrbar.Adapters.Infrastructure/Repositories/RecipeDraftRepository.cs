using Ernaehrbar.Adapters.Infrastructure.Data;
using Ernaehrbar.Adapters.Infrastructure.Data.Entities;
using Ernaehrbar.Parts.Domain;
using Ernaehrbar.Parts.Ports;
using Microsoft.EntityFrameworkCore;
using DomainRecipeSource = Ernaehrbar.Parts.Domain.RecipeSource;
using DomainDraftStatus = Ernaehrbar.Parts.Domain.DraftStatus;
using InfrastructureRecipeSource = Ernaehrbar.Adapters.Infrastructure.Data.Entities.RecipeSource;
using InfrastructureDraftStatus = Ernaehrbar.Adapters.Infrastructure.Data.Entities.DraftStatus;

namespace Ernaehrbar.Adapters.Infrastructure.Repositories;

/// <summary>
/// Infrastructure adapter implementing IRecipeDraftRepository port.
/// </summary>
public class RecipeDraftRepository : IRecipeDraftRepository
{
    private readonly ApplicationDbContext _context;

    public RecipeDraftRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddAsync(RecipeDraftDto draft, CancellationToken cancellationToken = default)
    {
        var entity = new RecipeDraft
        {
            GroupId = draft.GroupId,
            CreatedByUserId = draft.CreatedByUserId,
            Name = draft.Name,
            Source = MapRecipeSourceToInfrastructure(draft.Source),
            Status = MapDraftStatusToInfrastructure(draft.Status),
            Description = draft.Description,
            Instructions = draft.Instructions,
            MealCategory = draft.MealCategory,
            OriginalData = draft.OriginalData,
            ReviewedByUserId = draft.ReviewedByUserId,
            ReviewedAt = draft.ReviewedAt
        };

        if (draft.Ingredients != null)
        {
            foreach (var ingredientDto in draft.Ingredients)
            {
                entity.Ingredients.Add(new RecipeDraftIngredient
                {
                    Name = ingredientDto.Name,
                    Quantity = ingredientDto.Quantity,
                    Unit = ingredientDto.Unit,
                    Notes = ingredientDto.Notes,
                    Order = ingredientDto.Order
                });
            }
        }

        _context.RecipeDrafts.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public async Task<RecipeDraftDto?> GetByIdAsync(int draftId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.RecipeDrafts
            .Include(rd => rd.Ingredients.OrderBy(i => i.Order))
            .FirstOrDefaultAsync(rd => rd.Id == draftId, cancellationToken);

        if (entity == null)
        {
            return null;
        }

        return MapToDto(entity);
    }

    public async Task UpdateAsync(RecipeDraftDto draft, CancellationToken cancellationToken = default)
    {
        if (draft.Id == null)
        {
            throw new ArgumentException("Draft ID is required for update", nameof(draft));
        }

        var entity = await _context.RecipeDrafts
            .Include(rd => rd.Ingredients)
            .FirstOrDefaultAsync(rd => rd.Id == draft.Id, cancellationToken);

        if (entity == null)
        {
            throw new InvalidOperationException($"RecipeDraft with ID {draft.Id} not found");
        }

        entity.Name = draft.Name;
        entity.Source = MapRecipeSourceToInfrastructure(draft.Source);
        entity.Status = MapDraftStatusToInfrastructure(draft.Status);
        entity.Description = draft.Description;
        entity.Instructions = draft.Instructions;
        entity.MealCategory = draft.MealCategory;
        entity.OriginalData = draft.OriginalData;
        entity.ReviewedByUserId = draft.ReviewedByUserId;
        entity.ReviewedAt = draft.ReviewedAt;

        // Update ingredients
        entity.Ingredients.Clear();
        if (draft.Ingredients != null)
        {
            foreach (var ingredientDto in draft.Ingredients)
            {
                entity.Ingredients.Add(new RecipeDraftIngredient
                {
                    Name = ingredientDto.Name,
                    Quantity = ingredientDto.Quantity,
                    Unit = ingredientDto.Unit,
                    Notes = ingredientDto.Notes,
                    Order = ingredientDto.Order
                });
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int draftId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.RecipeDrafts.FindAsync([draftId], cancellationToken);
        if (entity != null)
        {
            _context.RecipeDrafts.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    private static RecipeDraftDto MapToDto(RecipeDraft entity)
    {
        return new RecipeDraftDto(
            Id: entity.Id,
            GroupId: entity.GroupId,
            CreatedByUserId: entity.CreatedByUserId,
            Name: entity.Name,
            Source: MapRecipeSourceToDomain(entity.Source),
            Status: MapDraftStatusToDomain(entity.Status),
            Description: entity.Description,
            Instructions: entity.Instructions,
            MealCategory: entity.MealCategory,
            OriginalData: entity.OriginalData,
            ReviewedByUserId: entity.ReviewedByUserId,
            ReviewedAt: entity.ReviewedAt,
            Ingredients: entity.Ingredients.Select(i => new RecipeDraftIngredientDto(
                Id: i.Id,
                Name: i.Name,
                Quantity: i.Quantity,
                Unit: i.Unit,
                Notes: i.Notes,
                Order: i.Order
            )).ToList()
        );
    }

    private static DomainRecipeSource MapRecipeSourceToDomain(InfrastructureRecipeSource source)
    {
        return source switch
        {
            InfrastructureRecipeSource.Generated => DomainRecipeSource.Generated,
            InfrastructureRecipeSource.Upload => DomainRecipeSource.Upload,
            InfrastructureRecipeSource.Manual => DomainRecipeSource.Manual,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
        };
    }

    private static InfrastructureRecipeSource MapRecipeSourceToInfrastructure(DomainRecipeSource source)
    {
        return source switch
        {
            DomainRecipeSource.Generated => InfrastructureRecipeSource.Generated,
            DomainRecipeSource.Upload => InfrastructureRecipeSource.Upload,
            DomainRecipeSource.Manual => InfrastructureRecipeSource.Manual,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
        };
    }

    private static DomainDraftStatus MapDraftStatusToDomain(InfrastructureDraftStatus status)
    {
        return status switch
        {
            InfrastructureDraftStatus.Pending => DomainDraftStatus.Pending,
            InfrastructureDraftStatus.Approved => DomainDraftStatus.Approved,
            InfrastructureDraftStatus.Rejected => DomainDraftStatus.Rejected,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }

    private static InfrastructureDraftStatus MapDraftStatusToInfrastructure(DomainDraftStatus status)
    {
        return status switch
        {
            DomainDraftStatus.Pending => InfrastructureDraftStatus.Pending,
            DomainDraftStatus.Approved => InfrastructureDraftStatus.Approved,
            DomainDraftStatus.Rejected => InfrastructureDraftStatus.Rejected,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }
}
