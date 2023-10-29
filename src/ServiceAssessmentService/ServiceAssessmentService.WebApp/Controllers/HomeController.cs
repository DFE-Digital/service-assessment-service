using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ServiceAssessmentService.WebApp.Models;
using ServiceAssessmentService.WebApp.Services.Book;

namespace ServiceAssessmentService.WebApp.Controllers;

[Route("/")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IBookingRequestReadService _bookingRequestReadService;

    public HomeController(ILogger<HomeController> logger, IBookingRequestReadService bookingRequestReadService)
    {
        _logger = logger;
        _bookingRequestReadService = bookingRequestReadService;
    }


    [Route("")]
    public IActionResult Index()
    {
        return RedirectToAction(nameof(Dashboard));
    }

    [Route("Dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        var assessments = await _bookingRequestReadService.GetAllAssessments();

        ViewData["Assessments"] = assessments;

        return View();
    }

    [Route("AccessibilityStatement")]
    public IActionResult AccessibilityStatement()
    {
        return View();
    }

    [Route("CookiePolicy")]
    public IActionResult CookiePolicy()
    {
        return View();
    }

    [Route("Privacy")]
    public IActionResult Privacy()
    {
        return View();
    }

    [Route("Error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        _logger.LogError("An error occurred");
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
