using ServiceAssessmentService.WebApp.Controllers.Book;
using ServiceAssessmentService.WebApp.Core;
using ServiceAssessmentService.WebApp.Models;

namespace ServiceAssessmentService.WebApp.Services.Book;

public class DummyInMemoryBookingRequestWriteService : IBookingRequestWriteService
{
    private readonly ILogger<DummyInMemoryBookingRequestWriteService> _logger;
    private readonly IDummyDataStore _dummyDataStore;

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

    public DummyInMemoryBookingRequestWriteService(
        ILogger<DummyInMemoryBookingRequestWriteService> logger,
        IDummyDataStore dummyDataStore
    )
    {
        _logger = logger;
        _dummyDataStore = dummyDataStore;
    }

    public async Task<IncompleteBookingRequest> CreateRequestAsync(Phase phaseConcluding, AssessmentType assessmentType)
    {
        _logger.LogInformation("Creating a new (empty) booking request");

        var newBookingRequest = await _dummyDataStore.CreateNewBookingRequest(phaseConcluding, assessmentType);

        // TODO: Consider creating a "booking request created" response type, which includes the ID in the response (as opposed to fetching it from the read API)...?
        // TODO: Consider the create API returning the newly created ID only? And all other updates returning no body?
        // TODO: Consider the API returning the response within a wrapper (meta/body)

        return newBookingRequest;
    }

    public async Task<ChangeRequestModel> UpdateRequestName(BookingRequestId id, string proposedName)
    {
        var response = new ChangeRequestModel();
        response.IsSuccessful = true;

        // Check if the request we're attempting to edit, exists.
        var bookingRequest = await _dummyDataStore.GetByIdAsync(id);
        if (bookingRequest is null)
        {
            response.IsSuccessful = false;
            response.Errors.Add(new ChangeRequestModel.Error($"Attempting to edit a request which does not appear to exist (ID: {id}) - unable to continue"));
        }


        // Validations
        if (string.IsNullOrWhiteSpace(proposedName))
        {
            response.IsSuccessful = false;
            response.Errors.Add(new ChangeRequestModel.Error("Enter a name, or skip and return to this question later"));
        }

        const int maxLengthChars = 100;
        if (!string.IsNullOrWhiteSpace(proposedName) && proposedName.Length > maxLengthChars)
        {
            response.IsSuccessful = false;
            response.Errors.Add(new ChangeRequestModel.Error($"Enter a name which is less than {maxLengthChars} characters long ({proposedName.Length} characters entered)"));
        }

        // TODO: Consider "intentionality" warnings? e.g., it's valid input but suspicious (e.g., all numbers, all punctuation, emoji, "smart" punctuation such as "smart quotes" and em/en-dashes, etc.)
        // TODO: Validation/warning about "special characters" (e.g., smart-quotes, em/en-dashes, etc.)...?
        // TODO: Validation/warning about characters outside of [a-zA-Z0-9 ] and "simple" punctuation...?
        // TODO: Validation/warning about multi-byte characters (which may affect calculated character length limits and not necessarily display correctly)...?
        // TODO: Validation/warning about emoji...?


        // If request found and no validation issues found, actually "do" the update
        if (bookingRequest is not null && response.IsSuccessful)
        {
            bookingRequest.Name = proposedName;
            await _dummyDataStore.Put(id, bookingRequest);
        }

        return response;
    }

    public async Task<ChangeRequestModel> UpdateDescription(BookingRequestId id, string proposedDescription)
    {
        var response = new ChangeRequestModel();
        response.IsSuccessful = true;

        // Check if the request we're attempting to edit, exists.
        var bookingRequest = await _dummyDataStore.GetByIdAsync(id);
        if (bookingRequest is null)
        {
            response.IsSuccessful = false;
            response.Errors.Add(new ChangeRequestModel.Error($"Attempting to edit a request which does not appear to exist (ID: {id}) - unable to continue"));
        }


        // Validations
        if (string.IsNullOrWhiteSpace(proposedDescription))
        {
            response.IsSuccessful = false;
            response.Errors.Add(new ChangeRequestModel.Error("Enter a description, or skip and return to this question later"));
        }

        const int maxLengthChars = DescriptionDto.MaxLength;
        if (!string.IsNullOrWhiteSpace(proposedDescription) && proposedDescription.Length > maxLengthChars)
        {
            response.IsSuccessful = false;
            response.Errors.Add(new ChangeRequestModel.Error($"Enter a description which is less than {maxLengthChars} characters long ({proposedDescription.Length} characters entered)"));
        }

        // TODO: Consider "intentionality" warnings? e.g., it's valid input but suspicious (e.g., all numbers, all punctuation, emoji, "smart" punctuation such as "smart quotes" and em/en-dashes, etc.)
        // TODO: Validation/warning about "special characters" (e.g., smart-quotes, em/en-dashes, etc.)...?
        // TODO: Validation/warning about characters outside of [a-zA-Z0-9 ] and "simple" punctuation...?
        // TODO: Validation/warning about multi-byte characters (which may affect calculated character length limits and not necessarily display correctly)...?
        // TODO: Validation/warning about emoji...?


        // If request found and no validation issues found, actually "do" the update
        if (bookingRequest is not null && response.IsSuccessful)
        {
            bookingRequest.Description = proposedDescription;
            await _dummyDataStore.Put(id, bookingRequest);
        }

        return response;
    }

    public async Task<ChangeRequestModel> UpdateProjectCode(BookingRequestId id, bool? isProjectCodeKnown,
        string proposedProjectCode)
    {
        var response = new ChangeRequestModel();
        response.IsSuccessful = true;

        // Check if the request we're attempting to edit, exists.
        var bookingRequest = await _dummyDataStore.GetByIdAsync(id);
        if (bookingRequest is null)
        {
            response.IsSuccessful = false;
            response.Errors.Add(new ChangeRequestModel.Error($"Attempting to edit a request which does not appear to exist (ID: {id}) - unable to continue"));
        }


        // Validations
        if (isProjectCodeKnown == true)
        {
            if (string.IsNullOrWhiteSpace(proposedProjectCode))
            {
                response.IsSuccessful = false;
                response.Errors.Add(new ChangeRequestModel.Error("Enter a project code, or skip and return to this question later"));
            }
            else if (proposedProjectCode.Length > ProjectCodeDto.ProjectCodeMaxLength)
            {
                response.IsSuccessful = false;
                response.Errors.Add(new ChangeRequestModel.Error($"Enter a project code which is less than {ProjectCodeDto.ProjectCodeMaxLength} characters long ({proposedProjectCode.Length} characters entered)"));
            }
            else
            {
                // TODO: Expected to start with "DDaT"


                // TODO: Consider "intentionality" warnings? e.g., it's valid input but suspicious (e.g., all numbers, all punctuation, emoji, "smart" punctuation such as "smart quotes" and em/en-dashes, etc.)
                // TODO: Validation/warning about "special characters" (e.g., smart-quotes, em/en-dashes, etc.)...?
                // TODO: Validation/warning about characters outside of [a-zA-Z0-9 ] and "simple" punctuation...?
                // TODO: Validation/warning about multi-byte characters (which may affect calculated character length limits and not necessarily display correctly)...?
                // TODO: Validation/warning about emoji...?
            }
        }

        if (isProjectCodeKnown == false)
        {
            if (!string.IsNullOrWhiteSpace(proposedProjectCode))
            {
                // Expecting the project code to be null when the user does not know it
                // TODO: Consider just disregarding the given project code value, rather than erroring?
                response.IsSuccessful = false;
                response.Errors.Add(new ChangeRequestModel.Error("Project code should be empty if we declare that we do not know it"));
            }
        }

        if (isProjectCodeKnown is null)
        {
            response.IsSuccessful = false;
            response.Errors.Add(new ChangeRequestModel.Error("Select whether you know the project code, or skip and return to this question later"));
        }

        // If request found and no validation issues found, actually "do" the update
        if (bookingRequest is not null && response.IsSuccessful)
        {
            bookingRequest.IsProjectCodeKnown = isProjectCodeKnown;
            bookingRequest.ProjectCode = proposedProjectCode;
            await _dummyDataStore.Put(id, bookingRequest);
        }

        return response;
    }

    public async Task<ChangeRequestModel> UpdateStartDate(BookingRequestId id, string? proposedYear,
        string? proposedMonth, string? proposedDayOfMonth)
    {
        var response = new ChangeRequestModel();
        response.IsSuccessful = true;

        // Check if the request we're attempting to edit, exists.
        var bookingRequest = await _dummyDataStore.GetByIdAsync(id);
        if (bookingRequest is null)
        {
            response.IsSuccessful = false;
            response.Errors.Add(new ChangeRequestModel.Error($"Attempting to edit a request which does not appear to exist (ID: {id}) - unable to continue"));
        }


        // Validations
        var startDateErrors = new List<string>();
        var startDateDayErrors = new List<string>();
        var startDateMonthErrors = new List<string>();
        var startDateYearErrors = new List<string>();

        int day = 0;
        int month = 0;
        int year = 0;

        if (string.IsNullOrWhiteSpace(proposedDayOfMonth))
        {
            startDateDayErrors.Add("Enter the day");
        }
        else if (!int.TryParse(proposedDayOfMonth, out day))
        {
            startDateDayErrors.Add("Enter a valid day");
        }

        if (string.IsNullOrWhiteSpace(proposedMonth))
        {
            startDateMonthErrors.Add("Enter the month");
        }
        else if (MonthNameToNumberMapping.ContainsKey(proposedMonth.ToLowerInvariant()))
        {
            // Allows users to enter the month name, instead of the number
            month = MonthNameToNumberMapping[proposedMonth.ToLowerInvariant()];
        }
        else if (!int.TryParse(proposedMonth, out month))
        {
            // Return an error if the month name/number is not recognised as being valid
            startDateMonthErrors.Add("Enter a valid month");
        }

        if (string.IsNullOrWhiteSpace(proposedYear))
        {
            startDateYearErrors.Add("Enter the year");
        }
        else if (!int.TryParse(proposedYear, out year))
        {
            startDateYearErrors.Add("Enter a valid year");
        }
        else
        {
            // TODO: Consider making these date range checks more granular (e.g., +- days / weeks / months, not just by year)

            const int minimumPermittedYear = 2000; // TODO: Consider making this dynamic/relative, based on "current" year?
            if (year < minimumPermittedYear)
            {
                startDateYearErrors.Add($"Contact the team if you need to book an assessment for a discovery/service starting before 01/Jan/{minimumPermittedYear}");
            }

            const int maximumPermittedYear = 2025; // TODO: Make this dynamic/relative, based on "current" year?
            if (year > maximumPermittedYear)
            {
                startDateYearErrors.Add($"Contact the team if you need to book an assessment for a discovery/service starting after 31/Dec/{maximumPermittedYear}");
            }
        }


        // convert to DateOnly, and add model error if invalid date
        DateOnly? proposedDate = null;
        if (!startDateYearErrors.Any() && !startDateMonthErrors.Any() && !startDateDayErrors.Any())
        {
            // If all date parts provided without error, don't try to validate each part individually (taking into account months and leap year etc)
            // ... instead, defer to C# to test if it's a valid date.
            try
            {
                proposedDate = new DateOnly(year, month, day);

                // TODO: Now that the proposed year/month/day values result in a "valid" date, apply additional validations on the resulting date?
                // TODO: Consider if date is "recent" (i.e., within last x years?)
                // TODO: Consider if date is in the future? (not necessarily a problem if e.g., a discovery is starting next month and the team are being proactive in booking assessment? perhaps this is a warning on the task list page?)
                // TODO: Consider if date is absurdly far in the future? (e.g., 100 years from now)
                // TODO: Consider relation to other dates (e.g., start date expected to be before end date) - warning vs error?
            }
            catch (ArgumentOutOfRangeException)
            {
                startDateErrors.Add($"{day}/{month}/{year} is not recognised as a valid day/month/year date");
            }
        }


        // TODO/FIXME: Doesn't allow for errors on specific parts of the date, instead current implementation has all errors be grouped into a generic "Errors" collection.
        response.Errors.AddRange(startDateErrors.Select(x => new ChangeRequestModel.Error(x)));
        response.Errors.AddRange(startDateDayErrors.Select(x => new ChangeRequestModel.Error(x)));
        response.Errors.AddRange(startDateMonthErrors.Select(x => new ChangeRequestModel.Error(x)));
        response.Errors.AddRange(startDateYearErrors.Select(x => new ChangeRequestModel.Error(x)));


        // If request found and no validation issues found, actually "do" the update
        if (bookingRequest is not null && response.IsSuccessful)
        {
            bookingRequest.StartDateYear = year;
            bookingRequest.StartDateMonth = month;
            bookingRequest.StartDateDay = day;

            await _dummyDataStore.Put(id, bookingRequest);
        }

        return response;
    }

    public async Task<ChangeRequestModel> UpdateEndDate(BookingRequestId id, bool? isEndDateKnown, string? proposedYear, string? proposedMonth,
        string? proposedDayOfMonth)
    {

        var response = new ChangeRequestModel();
        response.IsSuccessful = true;

        // Check if the request we're attempting to edit, exists.
        var bookingRequest = await _dummyDataStore.GetByIdAsync(id);
        if (bookingRequest is null)
        {
            response.IsSuccessful = false;
            response.Errors.Add(new ChangeRequestModel.Error($"Attempting to edit a request which does not appear to exist (ID: {id}) - unable to continue"));
        }


        var isEndDateKnownErrors = new List<string>();
        var endDateErrors = new List<string>();
        var endDateDayErrors = new List<string>();
        var endDateMonthErrors = new List<string>();
        var endDateYearErrors = new List<string>();

        int day = 0;
        int month = 0;
        int year = 0;

        // Validations
        if (isEndDateKnown == true)
        {
            if (string.IsNullOrWhiteSpace(proposedDayOfMonth))
            {
                endDateDayErrors.Add("Enter the day");
            }
            else if (!int.TryParse(proposedDayOfMonth, out day))
            {
                endDateDayErrors.Add("Enter a valid day");
            }

            if (string.IsNullOrWhiteSpace(proposedMonth))
            {
                endDateMonthErrors.Add("Enter the month");
            }
            else if (MonthNameToNumberMapping.ContainsKey(proposedMonth.ToLowerInvariant()))
            {
                // Allows users to enter the month name, instead of the number
                month = MonthNameToNumberMapping[proposedMonth.ToLowerInvariant()];
            }
            else if (!int.TryParse(proposedMonth, out month))
            {
                // Return an error if the month name/number is not recognised as being valid
                endDateMonthErrors.Add("Enter a valid month");
            }

            if (string.IsNullOrWhiteSpace(proposedYear))
            {
                endDateYearErrors.Add("Enter the year");
            }
            else if (!int.TryParse(proposedYear, out year))
            {
                endDateYearErrors.Add("Enter a valid year");
            }
            else
            {
                // TODO: Consider making these date range checks more granular (e.g., +- days / weeks / months, not just by year)

                const int
                    minimumPermittedYear =
                        2000; // TODO: Consider making this dynamic/relative, based on "current" year?
                if (year < minimumPermittedYear)
                {
                    endDateYearErrors.Add(
                        $"Contact the team if you need to book an assessment for a discovery/service ending before 01/Jan/{minimumPermittedYear}");
                }

                const int maximumPermittedYear = 2025; // TODO: Make this dynamic/relative, based on "current" year?
                if (year > maximumPermittedYear)
                {
                    endDateYearErrors.Add(
                        $"Contact the team if you need to book an assessment for a discovery/service ending after 31/Dec/{maximumPermittedYear}");
                }
            }


            // convert to DateOnly, and add model error if invalid date
            DateOnly? proposedDate = null;
            if (!endDateYearErrors.Any() && !endDateMonthErrors.Any() && !endDateDayErrors.Any())
            {
                // If all date parts provided without error, don't try to validate each part individually (taking into account months and leap year etc)
                // ... instead, defer to C# to test if it's a valid date.
                try
                {
                    proposedDate = new DateOnly(year, month, day);

                    // TODO: Now that the proposed year/month/day values result in a "valid" date, apply additional validations on the resulting date?
                    // TODO: Consider if date is "recent" (i.e., within last x years?)
                    // TODO: Consider if date is in the future? (not necessarily a problem if e.g., a discovery is ending next month and the team are being proactive in booking assessment? perhaps this is a warning on the task list page?)
                    // TODO: Consider if date is absurdly far in the future? (e.g., 100 years from now)
                    // TODO: Consider relation to other dates (e.g., end date expected to be before end date) - warning vs error?
                }
                catch (ArgumentOutOfRangeException)
                {
                    endDateErrors.Add($"{day}/{month}/{year} is not recognised as a valid day/month/year date");
                }
            }
        }


        if (isEndDateKnown == false)
        {
            if (!string.IsNullOrWhiteSpace(proposedYear) || !string.IsNullOrWhiteSpace(proposedMonth) || !string.IsNullOrWhiteSpace(proposedDayOfMonth))
            {
                // Expecting the end date to be null when the user does not know it
                // TODO: Consider just disregarding the given end date value, rather than erroring?
                response.IsSuccessful = false;
                response.Errors.Add(new ChangeRequestModel.Error("End date should be empty if we declare that we do not know it"));
            }
        }

        if (isEndDateKnown is null)
        {
            response.IsSuccessful = false;
            response.Errors.Add(new ChangeRequestModel.Error("Select whether you know the end date, or skip and return to this question later"));
        }

        // TODO/FIXME: Doesn't allow for errors on specific parts of the date, instead current implementation has all errors be grouped into a generic "Errors" collection.
        response.Errors.AddRange(isEndDateKnownErrors.Select(x => new ChangeRequestModel.Error(x)));
        response.Errors.AddRange(endDateErrors.Select(x => new ChangeRequestModel.Error(x)));
        response.Errors.AddRange(endDateDayErrors.Select(x => new ChangeRequestModel.Error(x)));
        response.Errors.AddRange(endDateMonthErrors.Select(x => new ChangeRequestModel.Error(x)));
        response.Errors.AddRange(endDateYearErrors.Select(x => new ChangeRequestModel.Error(x)));


        // If request found and no validation issues found, actually "do" the update
        if (bookingRequest is not null && response.IsSuccessful)
        {
            bookingRequest.EndDateYear = year;
            bookingRequest.EndDateMonth = month;
            bookingRequest.EndDateDay = day;

            await _dummyDataStore.Put(id, bookingRequest);
        }

        return response;
    }

    public async Task<ChangeRequestModel> UpdateReviewDates(BookingRequestId id, List<DateOnly>? proposedReviewDates)
    {
        var response = new ChangeRequestModel();
        response.IsSuccessful = true;

        // Check if the request we're attempting to edit, exists.
        var bookingRequest = await _dummyDataStore.GetByIdAsync(id);
        if (bookingRequest is null)
        {
            response.IsSuccessful = false;
            response.Errors.Add(new ChangeRequestModel.Error($"Attempting to edit a request which does not appear to exist (ID: {id}) - unable to continue"));
        }


        // Validations
        if(proposedReviewDates is null || proposedReviewDates.Count == 0)
        {
            response.IsSuccessful = false;
            response.Errors.Add(new ChangeRequestModel.Error("Select at least one review date, or skip and return to this question later"));
        }
        
        // TODO: More validations
        // TODO: Should be "week beginning" dates, not midweek?
        // TODO: Should be within permissible date range
        // TODO: Consider what happens if a previously valid value is no longer valid (e.g., end date is now within 5weeks and no selections are valid)
        // TODO: Consider what happens if the end date changes and the selected value is no longer valid (e.g., newly inputted end date is after (or otherwise incompatible with) a selected review date)
        // TODO: Reject (or discard) duplicated values
        
        
        // If request found and no validation issues found, actually "do" the update
        if (bookingRequest is not null && response.IsSuccessful)
        {
            bookingRequest.ReviewDates = proposedReviewDates;

            await _dummyDataStore.Put(id, bookingRequest);
        }

        return response;
    }
}
