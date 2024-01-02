using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Humanizer;
using ServiceAssessmentService.WebApp.Areas.Book.Views.Shared;
using ServiceAssessmentService.WebApp.Models;

namespace ServiceAssessmentService.WebApp.Controllers.Book;

public partial class BookingRequestController : Controller
{

    private static SimpleRadiosQuestionModel NewScaffoldedAssessmentTypeModel() => new()
    {
        FormController = "BookingRequest",
        FormAction = nameof(AssessmentType),
        InputName = AssessmentTypeDto.FormName,
        QuestionText = "What type of assessment do you wish to book?",
        QuestionHintText = "Select one option.",
    };

    private static SimpleRadiosQuestionModel.RadioItem? AssessmentTypeToRadioItemModel(AssessmentType? assessmentType)
    {
        if (assessmentType is null)
        {
            return null;
        }

        return new SimpleRadiosQuestionModel.RadioItem(
            assessmentType.Id,
            assessmentType.Id,
            assessmentType.DisplayNameLowerCase.Transform(To.SentenceCase),
            assessmentType.IsEnabled,
            assessmentType.SortOrder
        );
    }


    [HttpGet]
    public async Task<IActionResult> AssessmentType()
    {
        var availableTypes = await _lookupsReadService.GetAssessmentTypes();
        var radioItems = availableTypes.Select(AssessmentTypeToRadioItemModel);

        var model = NewScaffoldedAssessmentTypeModel();
        // model.BookingRequestId = bookingRequestId;
        // model.Value = bookingRequest.Name ?? string.Empty;
        model.Errors = new List<string>();
        model.AllowedValues = radioItems;

        return View("SimpleRadiosQuestion", model);
    }

    [HttpGet]
    public async Task<IActionResult> AssessmentTypeUnavailable(string type)
    {
        _logger.LogInformation("assessment type {AssessmentTypeId} is not available, but has been requested", type);
        var availableTypes = await _lookupsReadService.GetAssessmentTypes();
        var selectedAssessmentType = availableTypes.Single(x => x.Id == type);

        return View(selectedAssessmentType);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AssessmentType([FromForm] AssessmentTypeDto dto)
    {
        var availableTypes = (await _lookupsReadService.GetAssessmentTypes()).ToList();

        // Detect where the submitted value does not match a known/recognised value
        // Note: This is likely either a programmer error, or submission of mischievous input.
        var selectedAssessmentType = availableTypes.SingleOrDefault(x => x.Id == dto.Value);
        if (selectedAssessmentType is null)
        {
            _logger.LogWarning("submitted assessment type {AssessmentTypeId} is not valid, select from the available options only -- likely programmer error / nefarious input", dto.Value);
            ModelState.AddModelError(AssessmentTypeDto.FormName, "The submitted assessment type is not valid, select from the available options only");
        }

        // TODO: Submit proposed value change to API, then add any provided errors to ModelState -- do next????
        if (selectedAssessmentType is null || !ModelState.IsValid)
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



            var radioItems = availableTypes.Select(AssessmentTypeToRadioItemModel);

            var model = NewScaffoldedAssessmentTypeModel();
            // model.BookingRequestId = bookingRequestId;
            // model.Value = bookingRequest.Name ?? string.Empty;
            model.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            model.AllowedValues = radioItems;

            // When prompting the user to correct any errors, pre-fill the form with the previously the submitted invalid value (i.e., not overwrite the form value from the database):
            model.Value = AssessmentTypeToRadioItemModel(selectedAssessmentType);

            return View("SimpleRadiosQuestion", model);
        }

        // TODO: Better feature flag implementation...
        // Also, name it specifically about creating a new assessment of this type (viewing/admin'ing etc are different activities likely under a different feature flag)...
        var featureFlagAssessmentTypeEnabled = (selectedAssessmentType.IsEnabled);

        if (!selectedAssessmentType.IsEnabled || !featureFlagAssessmentTypeEnabled)
        {
            // Not all assessment types are available to book (technically because not implemented yet, or disabled via configuration/feature flag..).
            // If the user selects the option that they wish to do this assessment type, but it is not yet implemented,
            // then we should log it/capture metrics of attempts (to gauge interest) and tell them so.
            _logger.LogInformation("assessment type {AssessmentTypeId} is not available, but has been requested", dto.Value);
            return RedirectToAction(nameof(AssessmentTypeUnavailable), new { type = selectedAssessmentType.Id });
        }

        // Submit to server
        //var bookingRequest = await _bookingRequestWriteService.CreateRequestAsync();
        //return RedirectToAction(nameof(RequestStarted), new {id = bookingRequest.RequestId});
        return RedirectToAction(nameof(CreateRequest));
    }
}

public sealed class AssessmentTypeDto
{
    /*
     * This is the name of the form field that the value will be bound to.
     * It is specified here to avoid divergence between the form and the controller/model.
     */
    public const string FormName = "assessment_type";

    [Required(ErrorMessage = "Select the type of assessment you wish to book")]
    [FromForm(Name = FormName)]
    public string Value { get; init; } = null!;
}
