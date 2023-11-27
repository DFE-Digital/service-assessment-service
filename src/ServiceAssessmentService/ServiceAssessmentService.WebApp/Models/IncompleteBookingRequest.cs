using System.Text;
using ServiceAssessmentService.WebApp.Core;
using ServiceAssessmentService.WebApp.Services.Lookups;

namespace ServiceAssessmentService.WebApp.Models;

public class IncompleteBookingRequest
{
    // dictionary of field name and completion status
    private readonly Dictionary<string, Func<IncompleteBookingRequest, bool>> _completionStatus = new()
    {
        // { nameof(AssessmentType), x => x.IsAssessmentTypeComplete() },
        // { nameof(PhaseConcluding), x => x.IsPhaseConcludingComplete() },
        { nameof(Name), x => x.IsNameComplete() },
        { nameof(Description), x => x.IsDescriptionComplete() },
        { nameof(IsProjectCodeKnown), x => x.IsProjectCodeComplete() },
        { nameof(StartDate), x => x.IsStartDateComplete() },
        { nameof(EndDate), x => x.IsEndDateComplete() },
        { nameof(ReviewDates), x => x.IsReviewDatesComplete() },
        { nameof(Portfolio), x => x.IsPortfolioComplete() },
    };

    public IncompleteBookingRequest(BookingRequestId requestId)
    {
        RequestId = requestId;
    }


    public BookingRequestId RequestId { get; init; }
    public AssessmentType? AssessmentType { get; set; } = null;
    public Phase? PhaseConcluding { get; set; } = null;
    public string? Name { get; set; } = null;
    public string? Description { get; set; } = null;
    public bool? IsProjectCodeKnown { get; set; } = null;
    public string? ProjectCode { get; set; } = null;

    public int? StartDateDay { get; set; } = null;
    public int? StartDateMonth { get; set; } = null;
    public int? StartDateYear { get; set; } = null;

    public DateOnly? StartDate
    {
        get
        {
            if (StartDateDay.HasValue && StartDateMonth.HasValue && StartDateYear.HasValue)
            {
                try
                {
                    return new DateOnly(StartDateYear.Value, StartDateMonth.Value, StartDateDay.Value);
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Invalid start date {0}/{1}/{2}", StartDateYear.Value, StartDateMonth.Value, StartDateDay.Value);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }

    public bool? IsEndDateKnown { get; set; } = null;
    public int? EndDateDay { get; set; } = null;
    public int? EndDateMonth { get; set; } = null;
    public int? EndDateYear { get; set; } = null;

    public DateOnly? EndDate
    {
        get
        {
            if (EndDateDay.HasValue && EndDateMonth.HasValue && EndDateYear.HasValue)
            {
                try
                {
                    return new DateOnly(EndDateYear.Value, EndDateMonth.Value, EndDateDay.Value);
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Invalid end date {0}/{1}/{2}", EndDateYear.Value, EndDateMonth.Value, EndDateDay.Value);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }

    public List<DateOnly> ReviewDates { get; set; } = new();
    public bool IsEndDateWithinNextFiveWeeks => EndDate is not null && EndDate.Value <= DateOnly.FromDateTime(DateTime.Today.AddDays(5 * 7));


    public Portfolio? Portfolio { get; set; } = null;

    public string GetDiscoveryOrService()
    {
        if (PhaseConcluding is null)
        {
            return string.Empty;
        }

        return "discovery".Equals(PhaseConcluding.Id, StringComparison.Ordinal)
            ? "discovery"
            : "service";
    }

    public string GetAssessmentName()
    {
        var n = new StringBuilder();
        if (PhaseConcluding is not null)
        {
            n.Append($"end of {PhaseConcluding.DisplayNameLowerCase}");
        }

        if (PhaseConcluding is not null && AssessmentType is not null)
        {
            n.Append(' ');
        }

        if (AssessmentType is not null)
        {
            n.Append($"{AssessmentType.DisplayNameLowerCase}");
        }

        return n.ToString();
    }

    public bool IsAssessmentTypeComplete()
    {
        return AssessmentType is not null;
    }

    public bool IsPhaseConcludingComplete()
    {
        return PhaseConcluding is not null;
    }

    public bool IsNameComplete()
    {
        return !string.IsNullOrWhiteSpace(Name);
    }

    public bool IsDescriptionComplete()
    {
        return !string.IsNullOrWhiteSpace(Description);
    }

    public bool IsProjectCodeComplete()
    {
        return IsProjectCodeKnown switch
        {
            null =>
                // Not yet declared if project code is known, thus is incomplete.
                false,
            false =>
                // Project code declared as not known, the actual project code is not required.
                true,
            true when !string.IsNullOrWhiteSpace(ProjectCode) =>
                // Project code declared as known, and the actual project code is provided -- all information provided, thus is complete.
                true,
            _ => throw new Exception(),
        };
    }

    public bool IsStartDateComplete()
    {
        return StartDate is not null;
    }

    public bool IsEndDateComplete()
    {
        return EndDate is not null;
    }

    public bool IsReviewDatesComplete()
    {
        if (IsEndDateWithinNextFiveWeeks)
        {
            // End date too soon - user cannot select review dates
            return true;
        }

        // User should be able to select at least one date
        return ReviewDates.Any();
    }

    public bool IsPortfolioComplete()
    {
        return Portfolio is not null;
    }

    public string GetOverallCompletionStatusDescription()
    {
        var count = StepCount();
        var countCompleted = CountOfCompleted();

        if (countCompleted == 0)
        {
            return "Not started";
        }
        else if (countCompleted == count)
        {
            return "Complete";
        }
        else
        {
            return $"In progress";
        }

    }

    public bool IsFullyComplete()
    {
        var count = StepCount();
        var countCompleted = CountOfCompleted();

        return (countCompleted == count);
    }
    public bool IsPartiallyComplete()
    {
        var count = StepCount();
        var countCompleted = CountOfCompleted();

        return (countCompleted != count) && (countCompleted > 0);
    }

    public int StepCount()
    {
        return _completionStatus.Count;
    }

    public int CountOfCompleted()
    {
        return _completionStatus.Count(x => x.Value(this));
    }
}
