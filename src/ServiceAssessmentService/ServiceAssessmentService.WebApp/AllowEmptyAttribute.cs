using System.ComponentModel.DataAnnotations;

namespace ServiceAssessmentService.WebApp;

/// <summary>
/// By default, ASP.NET Core will convert empty strings (including whitespace-only strings) to null when binding to a string property.
/// This attribute prevents that behaviour.
/// 
/// See also: https://github.com/dotnet/aspnetcore/issues/29948#issuecomment-1898216682
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
internal class AllowEmptyAttribute : DisplayFormatAttribute
{
    public AllowEmptyAttribute() : base()
    {
        ConvertEmptyStringToNull = false;
    }
}
