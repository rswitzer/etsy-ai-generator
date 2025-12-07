using System.Text;
using EtsyAiGenerator.Application.ListingBuilder.Contracts;

namespace EtsyAiGenerator.Application.ListingBuilder.Services;

public sealed class StubListingBuilderService : IListingBuilderService
{
    private const int MaxTagCount = 13;

    public Task<ListingPreview> BuildPreviewAsync(ListingDraft draft, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(draft);
        cancellationToken.ThrowIfCancellationRequested();

        var title = BuildSuggestedTitle(draft);
        var description = BuildDescription(draft);
        var tags = BuildTags(draft);
        var preview = new ListingPreview(
            title,
            description,
            tags,
            draft.Tone,
            "Stub output. Wire up AI providers before shipping to production.");

        return Task.FromResult(preview);
    }

    private static string BuildSuggestedTitle(ListingDraft draft)
    {
        var baseTitle = string.IsNullOrWhiteSpace(draft.Title)
            ? draft.ProductType
            : draft.Title.Trim();

        if (!string.IsNullOrWhiteSpace(draft.ProductType) &&
            !baseTitle.Contains(draft.ProductType, StringComparison.OrdinalIgnoreCase))
        {
            baseTitle = $"{draft.ProductType} | {baseTitle}";
        }

        return baseTitle.Length <= 120 ? baseTitle : baseTitle[..120];
    }

    private static string BuildDescription(ListingDraft draft)
    {
        var builder = new StringBuilder();
        var customer = string.IsNullOrWhiteSpace(draft.TargetCustomer)
            ? "thoughtful shoppers"
            : draft.TargetCustomer.Trim();

        builder.AppendLine($"Thoughtfully crafted for {customer}.");

        if (draft.Highlights.Any())
        {
            builder.AppendLine();
            builder.AppendLine("Highlights:");
            foreach (var highlight in draft.Highlights)
            {
                builder.AppendLine($"- {highlight}");
            }
        }

        if (draft.Materials.Any())
        {
            builder.AppendLine();
            builder.AppendLine("Materials:");
            foreach (var material in draft.Materials)
            {
                builder.AppendLine($"- {material}");
            }
        }

        if (draft.Price.HasValue)
        {
            builder.AppendLine();
            builder.AppendLine($"Recommended price point: {draft.Price.Value:C} (validate against your cost model).");
        }

        builder.AppendLine();
        builder.AppendLine("This is a placeholder description. Hook up AI-backed copy once ready.");

        return builder.ToString().Trim();
    }

    private static IReadOnlyCollection<string> BuildTags(ListingDraft draft)
    {
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var ordered = new List<string>(capacity: MaxTagCount);

        void TryAdd(string? candidate)
        {
            if (string.IsNullOrWhiteSpace(candidate))
            {
                return;
            }

            if (seen.Add(candidate))
            {
                ordered.Add(candidate);
            }
        }

        foreach (var token in Tokenize(draft.ProductType))
        {
            TryAdd(token);
        }

        foreach (var token in Tokenize(draft.Title))
        {
            TryAdd(token);
        }

        foreach (var highlight in draft.Highlights.SelectMany(Tokenize))
        {
            TryAdd(highlight);
        }

        TryAdd(draft.TargetCustomer);

        return ordered.Take(MaxTagCount).ToArray();
    }

    private static IEnumerable<string> Tokenize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            yield break;
        }

        foreach (var token in value
                     .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                     .Select(token => token.Trim().ToLowerInvariant())
                     .Where(token => token.Length >= 3))
        {
            yield return token;
        }
    }
}
