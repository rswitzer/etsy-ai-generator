namespace EtsyAiGenerator.Application.ListingBuilder.Contracts;

public sealed record class ListingPreview(
    string SuggestedTitle,
    string Description,
    IReadOnlyCollection<string> Tags,
    string Tone,
    string Notes);
