using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Humanizer;
using ServiceAssessmentService.WebApp.Areas.Book.Views.Shared;
using ServiceAssessmentService.WebApp.Models;

namespace ServiceAssessmentService.WebApp.Controllers.Book;

public partial class BookingRequestController : Controller
{

    private static SimpleRadiosQuestionModel NewScaffoldedPhaseConcludingModel() => new()
    {
        FormController = "BookingRequest",
        FormAction = nameof(PhaseConcluding),
        InputName = PhaseConcludingDto.FormName,
        QuestionText = "What phase are you currently in?",
        QuestionHintText = "Select one option. This is the phase you wish to be assessed.",
    };

    private static SimpleRadiosQuestionModel.RadioItem? PhaseToRadioItemModel(Phase? phase)
    {
        if (phase is null)
        {
            return null;
        }

        return new SimpleRadiosQuestionModel.RadioItem(
            phase.Id,
            phase.Id,
            phase.DisplayNameLowerCase.Transform(To.SentenceCase),
            phase.IsEnabled,
            phase.SortOrder
        );
    }


    [HttpGet]
    public async Task<IActionResult> PhaseConcluding()
    {
        var availablePhases = await _lookupsReadService.GetPhases();
        var radioItems = availablePhases.Select(PhaseToRadioItemModel);

        var model = NewScaffoldedPhaseConcludingModel();
        // model.BookingRequestId = bookingRequestId;
        // model.Value = bookingRequest.Name ?? string.Empty;
        model.Errors = new List<string>();
        model.AllowedValues = radioItems;

        return View("SimpleRadiosQuestion", model);
    }

    [HttpGet]
    public async Task<IActionResult> PhaseConcludingUnavailable(string phase)
    {
        _logger.LogInformation("assessment phase {PhaseConcludingId} is not available, but has been requested", phase);
        var availablePhases = await _lookupsReadService.GetPhases();
        var selectedPhaseConcluding = availablePhases.Single(x => x.Id == phase);

        return View(selectedPhaseConcluding);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PhaseConcluding([FromForm] PhaseConcludingDto dto)
    {
        var availablePhases = (await _lookupsReadService.GetPhases()).ToList();

        // Detect where the submitted value does not match a known/recognised value
        // Note: This is likely either a programmer error, or submission of mischievous input.
        var selectedPhaseConcluding = availablePhases.SingleOrDefault(x => x.Id == dto.Value);
        if (selectedPhaseConcluding is null)
        {
            _logger.LogWarning("submitted assessment phase {PhaseConcludingId} is not valid, select from the available options only -- likely programmer error / nefarious input", dto.Value);
            ModelState.AddModelError(PhaseConcludingDto.FormName, "The submitted assessment phase is not valid, select from the available options only");
        }

        // TODO: Submit proposed value change to API, then add any provided errors to ModelState -- do next????
        if (selectedPhaseConcluding is null || !ModelState.IsValid)
        {
            if (dto.Value is null)
            {
                // HTTP 400 - Bad Request, to indicate value is missing/not provided
                Response.StatusCode = 400;
            }
            else
            {
                // HTTP 422 - Unprocessable Entity, to indicate the submitted value is not acceptable/not valid
                Response.StatusCode = 422;
            }

            var radioItems = availablePhases.Select(PhaseToRadioItemModel);

            var model = NewScaffoldedPhaseConcludingModel();
            // model.BookingRequestId = bookingRequestId;
            // model.Value = bookingRequest.Name ?? string.Empty;
            model.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            model.AllowedValues = radioItems;

            // When prompting the user to correct any errors, pre-fill the form with the previously the submitted invalid value (i.e., not overwrite the form value from the database):
            model.Value = PhaseToRadioItemModel(selectedPhaseConcluding);

            return View("SimpleRadiosQuestion", model);
        }

        // TODO: Better feature flag implementation...
        // Also, name it specifically about creating a new assessment of this phase (viewing/admin'ing etc are different activities likely under a different feature flag)...
        var featureFlagPhaseConcludingEnabled = (selectedPhaseConcluding.IsEnabled);

        if (!selectedPhaseConcluding.IsEnabled || !featureFlagPhaseConcludingEnabled)
        {
            // Not all assessment phases are available to book (technically because not implemented yet, or disabled via configuration/feature flag..).
            // If the user selects the option that they wish to do this assessment phase, but it is not yet implemented,
            // then we should log it/capture metrics of attempts (to gauge interest) and tell them so.
            _logger.LogInformation("assessment phase {PhaseConcludingId} is not available, but has been requested", dto.Value);
            return RedirectToAction(nameof(PhaseConcludingUnavailable), new { phase = selectedPhaseConcluding.Id });
        }

        // Submit to server
        //var bookingRequest = await _bookingRequestWriteService.CreateRequestAsync();
        return RedirectToAction(nameof(AssessmentType));
    }
}

public sealed class PhaseConcludingDto
{
    /*
     * This is the name of the form field that the value will be bound to.
     * It is specified here to avoid divergence between the form and the controller/model.
     */
    public const string FormName = "phase_concluding";

    [Required(ErrorMessage = "Select your current phase")]
    [FromForm(Name = FormName)]
    public string Value { get; init; } = null!;
}
