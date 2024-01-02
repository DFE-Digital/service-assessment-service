using Microsoft.AspNetCore.Mvc;
using ServiceAssessmentService.WebApp.Core;
using System.Net;
using ServiceAssessmentService.WebApp.Models;
using ServiceAssessmentService.WebApp.Services.Lookups;

namespace ServiceAssessmentService.WebApp.Controllers.Book;

public partial class BookingRequestController : Controller
{

    [HttpGet]
    public async Task<IActionResult> Portfolio(Guid id)
    {
        var bookingRequestId = new BookingRequestId(id);
        var bookingRequest = await _bookingRequestReadService.GetByIdAsync(bookingRequestId);
        if (bookingRequest is null)
        {
            return NotFound($"Booking request with ID {bookingRequestId} not found");
        }

        var portfolioOptions = (await _lookupsReadService.GetPortfolioOptions()).ToList();

        var model = new PortfolioViewModel()
        {
            RequestId = bookingRequestId,
            MainPortfolioOptions = portfolioOptions.Where(p => p.IsInMainSection).ToList(),
            OtherPortfolioOptions = portfolioOptions.Where(p => !p.IsInMainSection).ToList(),

            PortfolioId = bookingRequest.Portfolio?.Id ?? string.Empty,
            Errors = new List<string>(),
        };



        return View("Portfolio", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Portfolio(Guid id, [FromForm] PortfolioDto dto)
    {
        var bookingRequestId = new BookingRequestId(id);
        var bookingRequest = await _bookingRequestReadService.GetByIdAsync(bookingRequestId);
        if (bookingRequest is null)
        {
            return NotFound($"Booking request with ID {bookingRequestId} not found");
        }

        var bookingRequestChangeResult = await _bookingRequestWriteService.UpdatePortfolio(bookingRequestId, dto.Value);
        if (bookingRequestChangeResult.HasErrors)
        {
            var portfolioOptions = (await _lookupsReadService.GetPortfolioOptions()).ToList();

            var model = new PortfolioViewModel()
            {
                RequestId = bookingRequestId,
                MainPortfolioOptions = portfolioOptions.Where(p => p.IsInMainSection).ToList(),
                OtherPortfolioOptions = portfolioOptions.Where(p => !p.IsInMainSection).ToList(),

                // When prompting the user to correct any errors, pre-fill the form with the previously the submitted invalid value (i.e., not overwrite the form value from the database):
                PortfolioId = dto.Value,

                Errors = bookingRequestChangeResult.Errors.Select(e => e.Message),
            };

            return View("Portfolio", model);
        }

        return RedirectToAction(nameof(Description), new { id = bookingRequestId });
    }
}


public sealed class PortfolioViewModel
{
    public BookingRequestId RequestId { get; init; }
    public string? PortfolioId { get; set; }


    public IEnumerable<Portfolio> MainPortfolioOptions { get; init; } = new List<Portfolio>();
    public IEnumerable<Portfolio> OtherPortfolioOptions { get; init; } = new List<Portfolio>();

    public IEnumerable<Portfolio> AllPortfolioOptions => MainPortfolioOptions
        .Concat(OtherPortfolioOptions);

    public string FormElementName => PortfolioDto.FormName;


    public IEnumerable<string> Errors { get; set; } = new List<string>();
}

public sealed class PortfolioDto
{
    /*
     * This is the name of the form field that the value will be bound to.
     * It is specified here to avoid divergence between the form and the controller/model.
     */
    public const string FormName = "portfolio_name";

    // Validations (e.g., length, allowable characters, etc.) are implemented via API
    [FromForm(Name = FormName)]
    public string Value { get; init; } = null!;
}
