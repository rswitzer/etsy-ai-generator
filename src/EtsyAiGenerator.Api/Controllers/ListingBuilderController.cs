using EtsyAiGenerator.Application.ListingBuilder.Contracts;
using EtsyAiGenerator.Application.ListingBuilder.Services;
using Microsoft.AspNetCore.Mvc;

namespace EtsyAiGenerator.Api.Controllers;

[ApiController]
[Route("api/listing-builder")]
public sealed class ListingBuilderController : ControllerBase
{
    private readonly IListingBuilderService _listingBuilderService;

    public ListingBuilderController(IListingBuilderService listingBuilderService)
    {
        _listingBuilderService = listingBuilderService;
    }

    [HttpPost("preview")]
    [ProducesResponseType(typeof(ListingPreview), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ListingPreview>> BuildPreviewAsync(
        [FromBody] ListingDraft draft,
        CancellationToken cancellationToken)
    {
        var preview = await _listingBuilderService.BuildPreviewAsync(draft, cancellationToken);
        return Ok(preview);
    }
}
