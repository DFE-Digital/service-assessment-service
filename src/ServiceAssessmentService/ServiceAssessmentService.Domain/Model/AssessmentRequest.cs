namespace ServiceAssessmentService.Domain.Model;

public class AssessmentRequest
{
    // dictionary of field name and completion status
    private readonly Dictionary<string, Func<AssessmentRequest, bool>> _completionStatus = new()
    {
        // { nameof(AssessmentType), x => x.IsAssessmentTypeComplete() },
        // { nameof(PhaseConcluding), x => x.IsPhaseConcludingComplete() },
        { nameof(Name), x => x.IsNameComplete() },
        { nameof(Description), x => x.IsDescriptionComplete() },
        // { nameof(IsProjectCodeKnown), x => x.IsProjectCodeComplete() },
        { nameof(PhaseStartDate), x => x.IsPhaseStartDateComplete() },
        { nameof(PhaseEndDate), x => x.IsPhaseEndDateComplete() },
        // { nameof(ReviewDates), x => x.IsReviewDatesComplete() },
        // { nameof(Portfolio), x => x.IsPortfolioComplete() },
        // { nameof(DeputyDirector), x => x.IsDeputyDirectorComplete() },
        // { nameof(SeniorResponsibleOfficer), x => x.IsSeniorResponsibleOfficerComplete() },
        // { nameof(ProductOwnerManager), x => x.IsProductOwnerManagerComplete() },
        // { nameof(DeliveryManager), x => x.IsDeliveryManagerComplete() },
    };

    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public TextValidationResult ValidateName()
    {
        var result = new TextValidationResult();

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
            result.IsValid = true;
        }

        return result;
    }

    public Phase? PhaseConcluding { get; set; } = null;

    public AssessmentType? AssessmentType { get; set; } = null;

    public DateOnly? PhaseStartDate { get; set; }

    public DateOnly? PhaseEndDate { get; set; }



    public const int DescriptionMaxLengthChars = 500;
    public string? Description { get; set; }

    public TextValidationResult ValidateDescription()
    {
        var result = new TextValidationResult();

        if (string.IsNullOrWhiteSpace(Description))
        {
            result.IsValid = false;
            result.ValidationErrors.Add(new ValidationError
            {
                FieldName = nameof(Description),
                ErrorMessage = "Description is required",
            });
        }
        else if (Description.Length > DescriptionMaxLengthChars)
        {
            result.IsValid = false;
            result.ValidationErrors.Add(new ValidationError
            {
                FieldName = nameof(Description),
                ErrorMessage = $"Description must be {DescriptionMaxLengthChars} characters or fewer",
            });
        }
        else
        {
            result.IsValid = true;
        }

        return result;
    }



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

    public bool IsNameComplete()
    {
        return !string.IsNullOrWhiteSpace(Name);
    }

    public bool IsDescriptionComplete()
    {
        return !string.IsNullOrWhiteSpace(Description);
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

    public bool IsPhaseStartDateComplete()
    {
        return PhaseStartDate is not null;
    }

    public bool IsPhaseEndDateComplete()
    {
        return PhaseEndDate is not null;
    }

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



public class ValidationError
{
    public string FieldName { get; set; } = null!;
    public string ErrorMessage { get; set; } = null!;
}

public class ValidationWarning
{
    public string FieldName { get; set; } = null!;
    public string WarningMessage { get; set; } = null!;
}

public class TextValidationResult
{
    public bool IsValid { get; set; }

    public List<ValidationWarning> ValidationWarnings { get; set; } = new();
    public List<ValidationError> ValidationErrors { get; set; } = new();
}

