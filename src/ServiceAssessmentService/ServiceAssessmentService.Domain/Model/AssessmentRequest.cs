using ServiceAssessmentService.Domain.Model.Validations;

namespace ServiceAssessmentService.Domain.Model;

public class AssessmentRequest
{
    // dictionary of field name and completion status
    private readonly Dictionary<string, Func<AssessmentRequest, bool>> _completionStatus = new()
    {
        // { nameof(AssessmentType), x => x.IsAssessmentTypeComplete() },
        // { nameof(PhaseConcluding), x => x.IsPhaseConcludingComplete() },
        {nameof(Name), x => x.IsNameComplete()},
        {nameof(Description), x => x.IsDescriptionComplete()},
        // { nameof(IsProjectCodeKnown), x => x.IsProjectCodeComplete() },
        {nameof(PhaseStartDate), x => x.IsPhaseStartDateComplete()},
        {nameof(PhaseEndDate), x => x.IsPhaseEndDateComplete()},
        // { nameof(ReviewDates), x => x.IsReviewDatesComplete() },
        // { nameof(Portfolio), x => x.IsPortfolioComplete() },
        // { nameof(DeputyDirector), x => x.IsDeputyDirectorComplete() },
        // { nameof(SeniorResponsibleOfficer), x => x.IsSeniorResponsibleOfficerComplete() },
        // { nameof(ProductOwnerManager), x => x.IsProductOwnerManagerComplete() },
        // { nameof(DeliveryManager), x => x.IsDeliveryManagerComplete() },
    };

    public Guid Id { get; set; }


    public Phase? PhaseConcluding { get; set; } = null;

    public AssessmentType? AssessmentType { get; set; } = null;

    // public bool? IsReassessment { get; set; } = null;


    #region Name

    public string Name { get; set; } = string.Empty;

    public TextValidationResult ValidateName()
    {
        var result = new TextValidationResult();
        result.IsValid = true;

        if (string.IsNullOrWhiteSpace(Name))
        {
            result.IsValid = false;
            result.ValidationErrors.Add(new ValidationError
            {
                FieldName = nameof(Name),
                ErrorMessage = "Name is required",
            });
        }
        else
        {
            if (Name.Any(c => c < 32 || 126 < c))
            {
                /*
                 * ASCII char codes 32-126 are standard printable characters (upper and lower case letters, numbers, typical punctuation, etc)
                 * Add a warning if any characters fall outside this range
                 * Note not an error as it may be desirable to use non-standard characters (e.g., accented characters or emoji)
                 */
                result.IsValid = false;
                result.ValidationWarnings.Add(new ValidationWarning
                {
                    FieldName = nameof(Name),
                    WarningMessage =
                        "Name contains non-standard ASCII characters -non-standard characters (e.g., \"smart quotes\" copy/pasted from MS Word) may not be intentional and may cause errors with values not be displayed correctly",
                });
            }

            // TODO: Consider max length (probably 100 chars?)
            // TODO: Consider rejecting newlines with an error, as name should normally be a short phrase only without newlines.
            // TODO: Consider handling of accented characters and multi-byte characters (e.g., emoji)
        }

        return result;
    }

    public bool IsNameComplete()
    {
        return !string.IsNullOrWhiteSpace(Name);
    }

    #endregion

    #region Description

    public const int DescriptionMaxLengthChars = 500;
    public string? Description { get; set; }

    public TextValidationResult ValidateDescription()
    {
        var result = new TextValidationResult();
        result.IsValid = true;

        if (string.IsNullOrWhiteSpace(Description))
        {
            result.IsValid = false;
            result.ValidationErrors.Add(new ValidationError
            {
                FieldName = nameof(Description),
                ErrorMessage = "Description is required",
            });
        }
        else
        {
            if (Description.Length > DescriptionMaxLengthChars)
            {
                result.IsValid = false;
                result.ValidationErrors.Add(new ValidationError
                {
                    FieldName = nameof(Description),
                    ErrorMessage = $"Description must be {DescriptionMaxLengthChars} characters or fewer",
                });
            }

            if (Description.Any(c => c < 32 || 126 < c))
            {
                /*
                 * ASCII char codes 32-126 are standard printable characters (upper and lower case letters, numbers, typical punctuation, etc)
                 * Add a warning if any characters fall outside this range
                 * Note not an error as it may be desirable to use non-standard characters (e.g., accented characters or emoji)
                 */
                result.IsValid = false;
                result.ValidationWarnings.Add(new ValidationWarning
                {
                    FieldName = nameof(Description),
                    WarningMessage =
                        "Description contains non-standard ASCII characters -non-standard characters (e.g., \"smart quotes\" copy/pasted from MS Word) may not be intentional and may cause errors with values not be displayed correctly",
                });
            }

            // TODO: Allow newlines without warning/error as this is a long text multi-line field
            // TODO: Consider handling of accented characters and multi-byte characters (e.g., emoji)
        }

        return result;
    }

    public bool IsDescriptionComplete()
    {
        return !string.IsNullOrWhiteSpace(Description);
    }

    #endregion


    #region Phase start date

    public DateOnly? PhaseStartDate { get; set; }

    public DateValidationResult ValidatePhaseStartDate()
    {
        var result = new DateValidationResult();
        result.IsValid = true;

        if (PhaseStartDate is null)
        {
            result.IsValid = false;
            result.DateValidationErrors.Add(new ValidationError
            {
                FieldName = nameof(PhaseStartDate),
                ErrorMessage = "Phase start date is required",
            });
        }
        else
        {
            // if (PhaseStartDate < DateOnly.FromDateTime(DateTime.UtcNow))
            // {
            //     result.IsValid = false;
            //     result.ValidationErrors.Add(new ValidationError
            //     {
            //         FieldName = nameof(PhaseStartDate),
            //         ErrorMessage = "Phase start date cannot be in the past",
            //     });
            // }


            // TODO: Now that the proposed year/month/day values result in a "valid" date, apply additional validations on the resulting date?
            // TODO: Consider if date is "recent" (i.e., within last x years?)
            // TODO: Consider if date is in the future? (not necessarily a problem if e.g., a discovery is starting next month and the team are being proactive in booking assessment? perhaps this is a warning on the task list page?)
            // TODO: Consider if date is absurdly far in the future? (e.g., 100 years from now)
            // TODO: Consider relation to other dates (e.g., start date expected to be before end date) - warning vs error?
        }

        return result;
    }

    public bool IsPhaseStartDateComplete()
    {
        return PhaseStartDate is not null;
    }

    #endregion

    #region Phase end date

    public bool? IsPhaseEndDateKnown { get; set; } = null;
    public DateOnly? PhaseEndDate { get; set; }

    public RadioConditionalValidationResult<DateValidationResult> ValidatePhaseEndDate()
    {
        var radioValidationResult = new RadioConditionalValidationResult<DateValidationResult>()
        {
            IsValid = true,
            NestedValidationResult = new DateValidationResult()
            {
                IsValid = true,
            }
        };

        if (IsPhaseEndDateKnown == false)
        {
            if (PhaseEndDate is not null)
            {
                // Phase end date not known - must not have an end date specified
                radioValidationResult.NestedValidationResult.IsValid = false;
                radioValidationResult.NestedValidationResult.DateValidationErrors.Add(new ValidationError
                {
                    FieldName = nameof(PhaseEndDate),
                    ErrorMessage =
                        "Phase end date must not be specified if the phase end date is declared as not known",
                });
            }
            else
            {
                // Phase declared as not known, and no date provided -- valid.
            }
        }
        else if (IsPhaseEndDateKnown == true)
        {
            // Phase end date known - must have an end date specified
            if (PhaseEndDate is null)
            {
                radioValidationResult.NestedValidationResult.IsValid = false;
                radioValidationResult.NestedValidationResult.DateValidationErrors.Add(new ValidationError
                {
                    FieldName = nameof(PhaseEndDate),
                    ErrorMessage = "Phase end date is required if the phase end date is declared as known",
                });
            }
            // else if (PhaseEndDate < DateOnly.FromDateTime(DateTime.UtcNow))
            // {
            //     result.IsValid = false;
            //     result.ValidationErrors.Add(new ValidationError
            //     {
            //         FieldName = nameof(PhaseEndDate),
            //         ErrorMessage = "Phase end date cannot be in the past",
            //     });
            // }
            // TODO: Now that the proposed year/month/day values result in a "valid" date, apply additional validations on the resulting date?
            // TODO: Consider if date is "recent" (i.e., within last x years?)
            // TODO: Consider if date is in the future? (not necessarily a problem if e.g., a discovery is ending next month and the team are being proactive in booking assessment? perhaps this is a warning on the task list page?)
            // TODO: Consider if date is absurdly far in the future? (e.g., 100 years from now)
            // TODO: Consider relation to other dates (e.g., end date expected to be before end date) - warning vs error?
            else
            {
                // Phase declared as known, and acceptable date provided -- valid.
            }
        }
        else
        {
            radioValidationResult.IsValid = false;
            radioValidationResult.RadioQuestionValidationErrors.Add(new ValidationError
            {
                FieldName = nameof(IsPhaseEndDateKnown),
                ErrorMessage = "Please select whether the phase end date is known",
            });
        }

        return radioValidationResult;
    }

    public bool IsPhaseEndDateComplete()
    {
        if (IsPhaseEndDateKnown is true && PhaseEndDate is not null)
        {
            // Declared as known, and date provided - complete.
            return true;
        }

        if (IsPhaseEndDateKnown is false && PhaseEndDate is null)
        {
            // Declared as not known, and no date provided - complete.
            return true;
        }

        // Any other combination is incomplete.
        return false;
    }

    #endregion


    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }


    public bool IsAssessmentTypeComplete()
    {
        return AssessmentType is not null;
    }

    public bool IsPhaseConcludingComplete()
    {
        return PhaseConcluding is not null;
    }


    // public bool IsProjectCodeComplete()
    // {
    //     return IsProjectCodeKnown switch
    //     {
    //         null =>
    //             // Not yet declared if project code is known, thus is incomplete.
    //             false,
    //         false =>
    //             // Project code declared as not known, the actual project code is not required.
    //             true,
    //         true when !string.IsNullOrWhiteSpace(ProjectCode) =>
    //             // Project code declared as known, and the actual project code is provided -- all information provided, thus is complete.
    //             true,
    //         _ => throw new Exception(),
    //     };
    // }


    // public bool IsReviewDatesComplete()
    // {
    //     if (IsEndDateWithinNextFiveWeeks)
    //     {
    //         // End date too soon - user cannot select review dates
    //         return true;
    //     }
    //
    //     // User should be able to select at least one date
    //     return ReviewDates.Any();
    // }

    // public bool IsPortfolioComplete()
    // {
    //     return Portfolio is not null;
    // }

    // public bool IsDeputyDirectorComplete()
    // {
    //     return DeputyDirector is not null
    //            && !string.IsNullOrWhiteSpace(DeputyDirector.Name)
    //            && !string.IsNullOrWhiteSpace(DeputyDirector.Email);
    // }
    //
    // public bool IsSeniorResponsibleOfficerComplete()
    // {
    //     return IsDeputyDirectorTheSeniorResponsibleOfficer switch
    //     {
    //         null =>
    //             // Not yet declared if project code is known, thus is incomplete.
    //             false,
    //         true =>
    //             // SRO declared as same as DD, thus name/email not required.
    //             true,
    //         false =>
    //             // SRO declared as different to DD, thus name/email required.
    //             (!string.IsNullOrWhiteSpace(SeniorResponsibleOfficer?.Name) && !string.IsNullOrWhiteSpace(SeniorResponsibleOfficer?.Email)),
    //     };
    // }
    //
    // public bool IsProductOwnerManagerComplete()
    // {
    //     return HasProductOwnerManager switch
    //     {
    //         null =>
    //             // Not yet declared if product owner / project manager is known, thus is incomplete.
    //             false,
    //         false =>
    //             // Declared as having nil product owner manager, thus name/email not required.
    //             true,
    //         true =>
    //             // Declared as having a product owner manager, thus name/email required.
    //             (!string.IsNullOrWhiteSpace(ProductOwnerManager?.Name) && !string.IsNullOrWhiteSpace(ProductOwnerManager?.Email)),
    //     };
    // }
    //
    // public bool IsDeliveryManagerComplete()
    // {
    //     return HasDeliveryManager switch
    //     {
    //         null =>
    //             // Not yet declared if delivery manager is known, thus is incomplete.
    //             false,
    //         false =>
    //             // Declared as having nil delivery manager, thus name/email not required.
    //             true,
    //         true =>
    //             // Declared as having a delivery manager, thus name/email required.
    //             (!string.IsNullOrWhiteSpace(DeliveryManager?.Name) && !string.IsNullOrWhiteSpace(DeliveryManager?.Email)),
    //     };
    // }

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
