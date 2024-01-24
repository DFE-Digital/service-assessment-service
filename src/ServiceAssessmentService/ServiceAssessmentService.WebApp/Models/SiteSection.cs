namespace ServiceAssessmentService.WebApp.Models;

/// <summary>
/// This enum is used to signify the section of the site a page belongs.
/// - The navigation bar uses this to determine which (if any) nav item should be highlighted as "active".
/// </summary>
internal enum SiteSection
{
    Default,
    PublicStatic,
    Home,
    Dashboard,
    BookingRequest,
    MyAccount,
}
