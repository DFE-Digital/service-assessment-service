using Microsoft.AspNetCore.Mvc;
using ServiceAssessmentService.WebApp.Core;
using ServiceAssessmentService.WebApp.Services.Book;
using ServiceAssessmentService.WebApp.Services.Lookups;

namespace ServiceAssessmentService.WebApp.Controllers.Book;

[Area("Book")]
public partial class BookingRequestController : Controller
{
    private readonly ILogger<BookingRequestController> _logger;

    private readonly IBookingRequestReadService _bookingRequestReadService;
    private readonly IBookingRequestWriteService _bookingRequestWriteService;
    private readonly ILookupsReadService _lookupsReadService;

    private static readonly Dictionary<string, int> MonthNameToNumberMapping = new Dictionary<string, int>()
    {
        {"jan", 1}, {"january", 1},
        {"feb", 2}, {"february", 2},
        {"mar", 3}, {"march", 3},
        {"apr", 4}, {"april", 4},
        {"may", 5},
        {"jun", 6}, {"june", 6},
        {"jul", 7}, {"july", 7},
        {"aug", 8}, {"august", 8},
        {"sep", 9}, {"september", 9},
        {"oct", 10}, {"october", 10},
        {"nov", 11}, {"november", 11},
        {"dec", 12}, {"december", 12},
    };

    public BookingRequestController(
        ILogger<BookingRequestController> logger,
        IBookingRequestReadService bookingRequestReadService,
        IBookingRequestWriteService bookingRequestWriteService, ILookupsReadService lookupsReadService)
    {
        _logger = logger;
        _bookingRequestReadService = bookingRequestReadService;
        _bookingRequestWriteService = bookingRequestWriteService;
        _lookupsReadService = lookupsReadService;
    }

    [HttpGet]
    public Task<IActionResult> Index()
    {
        return Task.FromResult<IActionResult>(View());
    }

    // TODO: Have this be HttpPost, and take the assessment type/phase as arguments
    [HttpGet]
    public async Task<IActionResult> CreateRequest()
    {
        // Can only create discovery peer reviews -- TODO: parameterise this....
        var phases = await _lookupsReadService.GetPhases();
        var assessmentTypes = await _lookupsReadService.GetAssessmentTypes();

        var discoveryPhase = phases.SingleOrDefault(x => x.Id == "discovery");
        var peerReviewAssessmentType = assessmentTypes.SingleOrDefault(x => x.Id == "peer_review");

        if (discoveryPhase is null || peerReviewAssessmentType is null)
        {
            throw new Exception();
        }

        var bookingRequest = await _bookingRequestWriteService.CreateRequestAsync(
            phaseConcluding: discoveryPhase,
            assessmentType: peerReviewAssessmentType
        );

        return RedirectToAction(nameof(RequestStarted), new { id = bookingRequest.RequestId });
    }


    [HttpGet]
    public async Task<IActionResult> RequestStarted(Guid id)
    {
        var bookingRequestId = new BookingRequestId(id);
        _logger.LogInformation("starting assessment request for id {BookingRequestId}", bookingRequestId);

        var bookingRequest = await _bookingRequestReadService.GetByIdAsync(bookingRequestId);

        if (bookingRequest is null)
        {
            return NotFound($"Booking request with ID {bookingRequestId} not found");
        }

        return View(bookingRequest);
    }


    [HttpGet]
    public async Task<IActionResult> Tasks(Guid id)
    {
        var bookingRequestId = new BookingRequestId(id);
        _logger.LogDebug("viewing task list for id {BookingRequestId}", bookingRequestId);

        var bookingRequest = await _bookingRequestReadService.GetByIdAsync(bookingRequestId);
        if (bookingRequest is null)
        {
            return NotFound($"Booking request with ID {bookingRequestId} not found");
        }

        var phaseNames = await _lookupsReadService.GetPhases();
        var assessmentTypes = await _lookupsReadService.GetAssessmentTypes();
        ViewData.Add("phase_type_human_name", phaseNames.SingleOrDefault(x => x.Id == bookingRequest.PhaseConcluding?.Id));
        ViewData.Add("assessment_type_human_name", assessmentTypes.SingleOrDefault(x => x.Id == bookingRequest.AssessmentType?.Id));


        return View(bookingRequest);
    }


    [HttpGet]
    public async Task<IActionResult> Portfolio(Guid id)
    {
        var bookingRequestId = new BookingRequestId(id);
        var bookingRequest = await _bookingRequestReadService.GetByIdAsync(bookingRequestId);

        if (bookingRequest is null)
        {
            return NotFound($"Booking request with ID {bookingRequestId} not found");
        }

        return View(bookingRequest);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PortfolioSubmit(Guid id)
    {
        var bookingRequestId = new BookingRequestId(id);
        var bookingRequest = await _bookingRequestReadService.GetByIdAsync(bookingRequestId);

        if (bookingRequest is null)
        {
            return NotFound($"Booking request with ID {bookingRequestId} not found");
        }

        return RedirectToAction(nameof(DeputyDirector), new { id = bookingRequestId });
    }

    [HttpGet]
    public async Task<IActionResult> DeputyDirector(Guid id)
    {
        var bookingRequestId = new BookingRequestId(id);
        var bookingRequest = await _bookingRequestReadService.GetByIdAsync(bookingRequestId);

        if (bookingRequest is null)
        {
            return NotFound($"Booking request with ID {bookingRequestId} not found");
        }

        return View(bookingRequest);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeputyDirectorSubmit(Guid id)
    {
        var bookingRequestId = new BookingRequestId(id);
        var bookingRequest = await _bookingRequestReadService.GetByIdAsync(bookingRequestId);

        if (bookingRequest is null)
        {
            return NotFound($"Booking request with ID {bookingRequestId} not found");
        }

        return RedirectToAction(nameof(SeniorResponsibleOfficer), new { id = bookingRequestId });
    }

    [HttpGet]
    public async Task<IActionResult> SeniorResponsibleOfficer(Guid id)
    {
        var bookingRequestId = new BookingRequestId(id);
        var bookingRequest = await _bookingRequestReadService.GetByIdAsync(bookingRequestId);

        if (bookingRequest is null)
        {
            return NotFound($"Booking request with ID {bookingRequestId} not found");
        }

        return View(bookingRequest);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SeniorResponsibleOfficerSubmit(Guid id)
    {
        var bookingRequestId = new BookingRequestId(id);
        var bookingRequest = await _bookingRequestReadService.GetByIdAsync(bookingRequestId);

        if (bookingRequest is null)
        {
            return NotFound($"Booking request with ID {bookingRequestId} not found");
        }

        return RedirectToAction(nameof(ProductOwnerManager), new { id = bookingRequestId });
    }

    [HttpGet]
    public async Task<IActionResult> ProductOwnerManager(Guid id)
    {
        var bookingRequestId = new BookingRequestId(id);
        var bookingRequest = await _bookingRequestReadService.GetByIdAsync(bookingRequestId);

        if (bookingRequest is null)
        {
            return NotFound($"Booking request with ID {bookingRequestId} not found");
        }

        return View(bookingRequest);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProductOwnerManagerSubmit(Guid id)
    {
        var bookingRequestId = new BookingRequestId(id);
        var bookingRequest = await _bookingRequestReadService.GetByIdAsync(bookingRequestId);

        if (bookingRequest is null)
        {
            return NotFound($"Booking request with ID {bookingRequestId} not found");
        }

        return RedirectToAction(nameof(DeliveryManager), new { id = bookingRequestId });
    }

    [HttpGet]
    public async Task<IActionResult> DeliveryManager(Guid id)
    {
        var bookingRequestId = new BookingRequestId(id);
        var bookingRequest = await _bookingRequestReadService.GetByIdAsync(bookingRequestId);

        if (bookingRequest is null)
        {
            return NotFound($"Booking request with ID {bookingRequestId} not found");
        }

        return View(bookingRequest);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeliveryManagerSubmit(Guid id)
    {
        var bookingRequestId = new BookingRequestId(id);
        var bookingRequest = await _bookingRequestReadService.GetByIdAsync(bookingRequestId);

        if (bookingRequest is null)
        {
            return NotFound($"Booking request with ID {bookingRequestId} not found");
        }

        return RedirectToAction(nameof(Tasks), new { id = bookingRequestId });
    }
}
