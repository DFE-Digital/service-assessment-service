using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ServiceAssessmentService.Data;
using ServiceAssessmentService.WebApp.Models;

namespace ServiceAssessmentService.WebApp.Controllers;

public class HomeController : Controller
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, AssessmentRequestRepository assessmentRequestRepository)
    {
        _logger = logger;
        _assessmentRequestRepository = assessmentRequestRepository;
    }


    [Route("")]
    public IActionResult Index()
    {
        return RedirectToAction(nameof(Dashboard));
    }

    [Route("Dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        var allAssessments = await _assessmentRequestRepository.GetAllAssessmentRequests();

        ViewBag.AllAssessments = allAssessments;

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

