using System.Text.RegularExpressions;

namespace ServiceAssessmentService.WebApp;

/// <summary>
/// This class is used to transform the URL parameters to lowercase and replace spaces with hyphens.
///
/// See also the GDS guidance on URL, which requires lowercase and dash-separated words
/// https://www.gov.uk/guidance/content-design/url-standards-for-gov-uk
/// 
/// For example:
/// <code>
///     /AssessmentRequest/Details/1
///     vs 
///     /assessment-request/details/1
/// </code>
///
/// Implementation is based on the following article:
/// https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-8.0#use-a-parameter-transformer-to-customize-token-replacement
/// </summary>
public class SlugifyParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if (value == null) { return null; }

        return Regex.Replace(value.ToString()!,
            "([a-z])([A-Z])",
            "$1-$2",
            RegexOptions.CultureInvariant,
            TimeSpan.FromMilliseconds(100)).ToLowerInvariant();
    }
}
