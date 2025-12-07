using EtsyAiGenerator.Application.ListingBuilder.Contracts;
using EtsyAiGenerator.Application.ListingBuilder.Services;

namespace EtsyAiGenerator.Application.Tests.ListingBuilder;

public sealed class StubListingBuilderServiceTests
{
    private readonly StubListingBuilderService _service = new();

    [Fact]
    public async Task BuildPreviewAsync_ShouldGenerateDeterministicContent()
    {
        var draft = new ListingDraft
        {
            Title = "Handmade Ceramic Mug",
            ProductType = "Mug",
            TargetCustomer = "coffee lovers",
            Materials = new[] { "Stoneware", "Lead-free glaze" },
            Highlights = new[] { "Dishwasher safe", "12oz capacity" },
            Tone = "Cheerful",
            Price = 34.95m
        };

        var result = await _service.BuildPreviewAsync(draft, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Contains("Mug", result.SuggestedTitle);
        Assert.Contains("Dishwasher safe", result.Description);
        Assert.Contains(result.Tags, tag => tag.Contains("mug", StringComparison.OrdinalIgnoreCase));
        Assert.True(result.Tags.Count <= 13, "Tag count should respect Etsy limits");
        Assert.Equal("Cheerful", result.Tone);
    }

    [Fact]
    public async Task BuildPreviewAsync_ShouldHonorCancellation()
    {
        var draft = new ListingDraft { Title = "Test", ProductType = "Poster" };
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() => _service.BuildPreviewAsync(draft, cts.Token));
    }
}
