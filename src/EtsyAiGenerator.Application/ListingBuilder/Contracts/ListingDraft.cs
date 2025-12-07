using System.ComponentModel.DataAnnotations;

namespace EtsyAiGenerator.Application.ListingBuilder.Contracts;

public sealed record class ListingDraft
{
    [Required]
    [MaxLength(120)]
    public string Title { get; init; } = string.Empty;

    [Required]
    [MaxLength(80)]
    public string ProductType { get; init; } = string.Empty;

    [MaxLength(250)]
    public string? TargetCustomer { get; init; }
        = "modern Etsy shopper";

    [MaxLength(60)]
    public string Tone { get; init; } = "Friendly";

    public IReadOnlyCollection<string> Materials { get; init; } = Array.Empty<string>();

    public IReadOnlyCollection<string> Highlights { get; init; } = Array.Empty<string>();

    [Range(0, double.MaxValue)]
    public decimal? Price { get; init; }
        = null;
}
