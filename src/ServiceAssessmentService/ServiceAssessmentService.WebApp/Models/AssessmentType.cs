namespace ServiceAssessmentService.WebApp.Models;

public class AssessmentType
{
    public string Id { get; init; } = null!;
    public string DisplayNameLowerCase { get; init; } = null!;
    public string DisplayNameWithIndefiniteArticleLowerCase
    {
        get
        {
            if (string.IsNullOrWhiteSpace(DisplayNameLowerCase))
            {
                return "";
            }

            // If first letter is a vowel, use "an" instead of "a"
            var indefiniteArticle = "a";
            var firstLetter = DisplayNameLowerCase[0];
            if (firstLetter is 'a' or 'e' or 'i' or 'o' or 'u')
            {
                indefiniteArticle = "an";
            }

            return $"{indefiniteArticle} {DisplayNameLowerCase}";
        }
    }

    public bool IsEnabled { get; init; } = false;
    public int SortOrder { get; init; } = 0;
}