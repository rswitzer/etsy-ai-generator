using EtsyAiGenerator.Application.ListingBuilder.Contracts;

namespace EtsyAiGenerator.Application.ListingBuilder.Services;

public interface IListingBuilderService
{
    Task<ListingPreview> BuildPreviewAsync(ListingDraft draft, CancellationToken cancellationToken = default);
}
