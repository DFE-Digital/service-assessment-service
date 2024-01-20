using Microsoft.AspNetCore.Mvc;
using ServiceAssessmentService.Data;

namespace ServiceAssessmentService.WebApp.Controllers;

public class BookController : Controller
{
    private readonly AssessmentRequestRepository _assessmentRequestRepository;
    private readonly ILogger<BookController> _logger;

    public BookController(AssessmentRequestRepository assessmentRequestRepository, ILogger<BookController> logger)
    {
        _logger = logger;
        _assessmentRequestRepository = assessmentRequestRepository;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> View(Guid id)
    {
        var assessmentRequest = await _assessmentRequestRepository.GetByIdAsync(id);
        if (assessmentRequest == null)
        {
            return NotFound();
        }

        return View(assessmentRequest);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Domain.Model.AssessmentRequest assessmentRequest)
    {
        if (!ModelState.IsValid)
        {
            return View(assessmentRequest);
        }

        assessmentRequest.Id = Guid.NewGuid();

        await _assessmentRequestRepository.CreateAsync(assessmentRequest);

//        return RedirectToAction(nameof(List));
        return RedirectToAction(nameof(View), new { assessmentRequest.Id });

    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var assessmentRequest = await _assessmentRequestRepository.DeleteAsync(id);
        if (assessmentRequest == null)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(List));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var assessmentRequest = await _assessmentRequestRepository.GetByIdAsync(id);
        if (assessmentRequest == null)
        {
            return NotFound();
        }

        return View(assessmentRequest);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, Domain.Model.AssessmentRequest assessmentRequest)
    {
        if (id != assessmentRequest.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(assessmentRequest);
        }

        await _assessmentRequestRepository.UpdateAsync(assessmentRequest);

        return RedirectToAction(nameof(View), new { id });
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var assessmentRequests = await _assessmentRequestRepository.GetAllAssessmentRequests();
        return View(assessmentRequests);
    }

}
