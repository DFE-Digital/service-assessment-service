using ServiceAssessmentService.Domain.Model.Validations;

namespace ServiceAssessmentService.Domain.Model;

public class AssessmentRequest
{
    // dictionary of field name and completion status
    private readonly Dictionary<string, Func<AssessmentRequest, bool>> _completionStatus = new()
    {
        {nameof(AssessmentType), x => x.IsAssessmentTypeComplete()},
        {nameof(PhaseConcluding), x => x.IsPhaseConcludingComplete()},
        {nameof(Name), x => x.IsNameComplete()},
        {nameof(Description), x => x.IsDescriptionComplete()},
        {nameof(ProjectCode), x => x.IsProjectCodeComplete()},
        {nameof(PhaseStartDate), x => x.IsPhaseStartDateComplete()},
        {nameof(PhaseEndDate), x => x.IsPhaseEndDateComplete()},
        // { nameof(ReviewDates), x => x.IsReviewDatesComplete() },
        {nameof(Portfolio), x => x.IsPortfolioComplete()},
        {nameof(DeputyDirector), x => x.IsDeputyDirectorComplete()},
        { nameof(SeniorResponsibleOfficer), x => x.IsSeniorResponsibleOfficerComplete() },
        { nameof(ProductOwnerManager), x => x.IsProductOwnerManagerComplete() },
        // { nameof(DeliveryManager), x => x.IsDeliveryManagerComplete() },
    };

    public Guid Id { get; set; }


    #region Phase concluding

    public Phase? PhaseConcluding { get; set; } = null;

    public RadioValidationResult ValidatePhaseConcluding(IEnumerable<Phase?> availablePhases)
    {
        var result = new RadioValidationResult();
        result.IsValid = true;

        if (PhaseConcluding is null)
        {
            result.IsValid = false;
            result.ValidationErrors.Add(new ValidationError
            {
                FieldName = nameof(PhaseConcluding),
                ErrorMessage = "Assessment type is required",
            });
        }
        else
        {
            if (!availablePhases.Select(x => x?.Id).Contains(PhaseConcluding.Id))
            {
                result.IsValid = false;
                result.ValidationErrors.Add(new ValidationError
                {
                    FieldName = nameof(PhaseConcluding),
                    ErrorMessage = "Assessment type is not valid",
                });
            }
            else
            {
                // Assessment type set and no validation errors - valid.
            }
        }

        return result;
    }

    public bool IsPhaseConcludingComplete()
    {
        return PhaseConcluding is not null;
    }

    #endregion


    #region Assessment type

    public AssessmentType? AssessmentType { get; set; } = null;


    public RadioValidationResult ValidateAssessmentType(IEnumerable<AssessmentType?> availableAssessmentTypes)
    {
        var result = new RadioValidationResult();
        result.IsValid = true;

        if (AssessmentType is null)
        {
            result.IsValid = false;
            result.ValidationErrors.Add(new ValidationError
            {
                FieldName = nameof(AssessmentType),
                ErrorMessage = "Assessment type is required",
            });
        }
        else
        {
            if (!availableAssessmentTypes.Select(x => x?.Id).Contains(AssessmentType.Id))
            {
                result.IsValid = false;
                result.ValidationErrors.Add(new ValidationError
                {
                    FieldName = nameof(AssessmentType),
                    ErrorMessage = "Assessment type is not valid",
                });
            }
            else
            {
                // Assessment type set and no validation errors - valid.
            }
        }

        return result;
    }

    public bool IsAssessmentTypeComplete()
    {
        return AssessmentType is not null;
    }

    #endregion

    public bool? IsReassessment { get; set; } = null;


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
                        "Name contains non-standard ASCII characters -- non-standard characters (e.g., \"smart quotes\" copy/pasted from MS Word) may not be intentional and may cause errors with values not be displayed correctly",
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
            // UI count of characters may differ from actual count due to newline handling
            // TODO: Consistent newline char handling everywhere (e.g., normalise everywhere to \n)
            if (Description.ReplaceLineEndings("\n").Length > DescriptionMaxLengthChars)
            {
                result.IsValid = false;
                result.ValidationErrors.Add(new ValidationError
                {
                    FieldName = nameof(Description),
                    ErrorMessage = $"Description must be {DescriptionMaxLengthChars} characters or fewer",
                });
            }

            // Allow newlines without warning/error as this is a long text multi-line field
            if (Description.Any(c => (c != '\n' && c != '\r') && (c < 32 || 126 < c)))
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
                        "Description contains non-standard ASCII characters -- non-standard characters (e.g., curly \"smart quotes\" copy/pasted from MS Word) may not be intentional and may cause errors with values not be displayed correctly",
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

    #region Project code

    public bool? IsProjectCodeKnown { get; set; } = null;

    public string? ProjectCode { get; set; } = string.Empty;

    public RadioConditionalValidationResult<TextValidationResult> ValidateProjectCode()
    {
        var radioValidationResult = new RadioConditionalValidationResult<TextValidationResult>()
        {
            IsValid = true,
            NestedValidationResult = new TextValidationResult()
            {
                IsValid = true,
            }
        };

        if (IsProjectCodeKnown == false)
        {
            if (!string.IsNullOrWhiteSpace(ProjectCode))
            {
                // Project code not known - must not have a project code specified
                radioValidationResult.NestedValidationResult.IsValid = false;
                radioValidationResult.NestedValidationResult.ValidationErrors.Add(new ValidationError
                {
                    FieldName = nameof(ProjectCode),
                    ErrorMessage =
                        "Project code must not be specified if the project code is declared as not known",
                });
            }
            else
            {
                // Project declared as not known, and no code provided -- valid.
            }
        }
        else if (IsProjectCodeKnown == true)
        {
            // Project code known - must have a project code specified
            if (string.IsNullOrWhiteSpace(ProjectCode))
            {
                radioValidationResult.NestedValidationResult.IsValid = false;
                radioValidationResult.NestedValidationResult.ValidationErrors.Add(new ValidationError
                {
                    FieldName = nameof(ProjectCode),
                    ErrorMessage = "Project code is required if the project code is declared as known",
                });
            }
            else
            {
                // Project declared as known, and code provided -- valid.
            }
        }
        else
        {
            radioValidationResult.IsValid = false;
            radioValidationResult.RadioQuestionValidationErrors.Add(new ValidationError
            {
                FieldName = nameof(IsProjectCodeKnown),
                ErrorMessage = "Please select whether the project code is known",
            });
        }

        return radioValidationResult;
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

    public bool IsProjectCodeComplete()
    {
        if (IsProjectCodeKnown is true && !string.IsNullOrWhiteSpace(ProjectCode))
        {
            // Declared as known, and code provided - complete.
            return true;
        }

        if (IsProjectCodeKnown is false && string.IsNullOrWhiteSpace(ProjectCode))
        {
            // Declared as not known, and no code provided - complete.
            return true;
        }

        // Any other combination is incomplete.
        return false;
    }

    #endregion

    #region Phase start date

    public DateOnly? PhaseStartDate { get; set; }

    public static readonly DateOnly
        EarliestPermittedPhaseStartDate = new(2000, 1, 1); // TODO: Consider making these relative to the current date

    public static readonly DateOnly
        LatestPermittedPhaseStartDate = new(2050, 1, 1); // TODO: Consider making these relative to the current date

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
        else if (PhaseStartDate < EarliestPermittedPhaseStartDate)
        {
            result.IsValid = false;
            result.DateValidationErrors.Add(new ValidationError
            {
                FieldName = nameof(PhaseStartDate),
                ErrorMessage = $"Phase start date cannot be before {EarliestPermittedPhaseStartDate}",
            });
        }
        else if (PhaseStartDate > LatestPermittedPhaseStartDate)
        {
            result.IsValid = false;
            result.DateValidationErrors.Add(new ValidationError
            {
                FieldName = nameof(PhaseStartDate),
                ErrorMessage = $"Phase start date cannot be after {LatestPermittedPhaseStartDate}",
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

    public static readonly DateOnly
        EarliestPermittedPhaseEndDate = new(2000, 1, 1); // TODO: Consider making these relative to the current date

    public static readonly DateOnly
        LatestPermittedPhaseEndDate = new(2050, 1, 1); // TODO: Consider making these relative to the current date

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
            else if (PhaseEndDate < EarliestPermittedPhaseEndDate)
            {
                radioValidationResult.NestedValidationResult.IsValid = false;
                radioValidationResult.NestedValidationResult.DateValidationErrors.Add(new ValidationError
                {
                    FieldName = nameof(PhaseEndDate),
                    ErrorMessage = $"Phase end date cannot be before {EarliestPermittedPhaseEndDate}",
                });
            }
            else if (PhaseEndDate > LatestPermittedPhaseEndDate)
            {
                radioValidationResult.NestedValidationResult.IsValid = false;
                radioValidationResult.NestedValidationResult.DateValidationErrors.Add(new ValidationError
                {
                    FieldName = nameof(PhaseEndDate),
                    ErrorMessage = $"Phase end date cannot be after {LatestPermittedPhaseEndDate}",
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

    #region Portfolio

    public Portfolio? Portfolio { get; set; } = null;


    public RadioValidationResult ValidatePortfolio(IEnumerable<Portfolio?> availablePortfolios)
    {
        var result = new RadioValidationResult();
        result.IsValid = true;

        if (Portfolio is null)
        {
            result.IsValid = false;
            result.ValidationErrors.Add(new ValidationError
            {
                FieldName = nameof(Portfolio),
                ErrorMessage = "Portfolio is required",
            });
        }
        else
        {
            if (!availablePortfolios.Select(x => x?.Id).Contains(Portfolio.Id))
            {
                result.IsValid = false;
                result.ValidationErrors.Add(new ValidationError
                {
                    FieldName = nameof(Portfolio),
                    ErrorMessage = "Portfolio is not valid",
                });
            }
            else
            {
                // Portfolio set and no validation errors - valid.
            }
        }

        return result;
    }

    public bool IsPortfolioComplete()
    {
        return Portfolio is not null;
    }

    #endregion


    #region DeputyDirector

    public Person? DeputyDirector { get; set; }

    public PersonValidationResult ValidateDeputyDirector()
    {
        var result = new PersonValidationResult();
        result.IsValid = true;

        if (DeputyDirector is null)
        {
            result.IsValid = false;
            result.ValidationErrors.Add(new ValidationError
            {
                FieldName = nameof(DeputyDirector),
                ErrorMessage = "DeputyDirector is required",
            });
        }
        else
        {
            if (string.IsNullOrWhiteSpace(DeputyDirector.PersonalName))
            {
                result.IsValid = false;
                result.PersonalNameErrors.Add(new ValidationError
                {
                    FieldName = nameof(DeputyDirector.PersonalName),
                    ErrorMessage = "Please enter the deputy director's personal name.",
                });
            }
            else
            {
                if (DeputyDirector.PersonalName.Any(c => c < 32 || 126 < c))
                {
                    /*
                     * ASCII char codes 32-126 are standard printable characters (upper and lower case letters, numbers, typical punctuation, etc)
                     * Add a warning if any characters fall outside this range
                     * Note not an error as it may be desirable to use non-standard characters (e.g., accented characters or emoji)
                     */
                    result.IsValid = false;
                    result.PersonalNameWarnings.Add(new ValidationWarning
                    {
                        FieldName = nameof(DeputyDirector.PersonalName),
                        WarningMessage =
                            "The deputy director's personal name contains non-standard ASCII characters -- non-standard characters (e.g., \"smart quotes\" copy/pasted from MS Word) may not be intentional and may cause errors with values not be displayed correctly",
                    });
                }
            }


            if (string.IsNullOrWhiteSpace(DeputyDirector.FamilyName))
            {
                result.IsValid = false;
                result.FamilyNameErrors.Add(new ValidationError
                {
                    FieldName = nameof(DeputyDirector.FamilyName),
                    ErrorMessage = "Please enter the deputy director's family name.",
                });
            }
            else
            {
                if (DeputyDirector.FamilyName.Any(c => c < 32 || 126 < c))
                {
                    /*
                     * ASCII char codes 32-126 are standard printable characters (upper and lower case letters, numbers, typical punctuation, etc)
                     * Add a warning if any characters fall outside this range
                     * Note not an error as it may be desirable to use non-standard characters (e.g., accented characters or emoji)
                     */
                    result.IsValid = false;
                    result.FamilyNameWarnings.Add(new ValidationWarning
                    {
                        FieldName = nameof(DeputyDirector.FamilyName),
                        WarningMessage =
                            "The deputy director's family name contains non-standard ASCII characters -- non-standard characters (e.g., \"smart quotes\" copy/pasted from MS Word) may not be intentional and may cause errors with values not be displayed correctly",
                    });
                }
            }

            if (string.IsNullOrWhiteSpace(DeputyDirector.Email))
            {
                result.IsValid = false;
                result.EmailErrors.Add(new ValidationError
                {
                    FieldName = nameof(DeputyDirector.Email),
                    ErrorMessage = "Please enter the deputy director's email address.",
                });
            }
            else
            {
                if (DeputyDirector.Email.Any(c => c < 32 || 126 < c))
                {
                    /*
                     * ASCII char codes 32-126 are standard printable characters (upper and lower case letters, numbers, typical punctuation, etc)
                     * Add a warning if any characters fall outside this range
                     * Note not an error as it may be desirable to use non-standard characters (e.g., accented characters or emoji)
                     */
                    result.IsValid = false;
                    result.EmailWarnings.Add(new ValidationWarning
                    {
                        FieldName = nameof(DeputyDirector.Email),
                        WarningMessage =
                            "The deputy director's email address contains non-standard ASCII characters -- non-standard characters (e.g., \"smart quotes\" copy/pasted from MS Word) may not be intentional and may cause errors with values not be displayed correctly",
                    });
                }

                if (!EmailValidationUtilities.IsValidEmail(DeputyDirector.Email))
                {
                    result.IsValid = false;
                    result.EmailErrors.Add(new ValidationError
                    {
                        FieldName = nameof(DeputyDirector.Email),
                        ErrorMessage = "The deputy director's email address is not recognised as being in a valid email format.",
                    });
                }

                if (!EmailValidationUtilities.IsValidDomain(DeputyDirector.Email))
                {
                    result.IsValid = false;
                    result.EmailErrors.Add(new ValidationError
                    {
                        FieldName = nameof(DeputyDirector.Email),
                        ErrorMessage = "The deputy director's email address is not recognised as having a recognised/acceptable domain.",
                    });
                }

                // TODO: Validate email format
                // TODO: Validate email domain is DfE
            }


            // TODO: Consider max length (probably 100 chars?)
            // TODO: Consider rejecting newlines with an error, as deputyDirector should normally be a short phrase only without newlines.
            // TODO: Consider handling of accented characters and multi-byte characters (e.g., emoji)
        }

        return result;
    }

    public bool IsDeputyDirectorComplete()
    {
        return DeputyDirector is not null
               && DeputyDirector.IsComplete();
    }

    #endregion


    #region SeniorResponsibleOfficer

    public bool? IsDeputyDirectorTheSeniorResponsibleOfficer { get; set; } = null;

    public Person? SeniorResponsibleOfficer { get; set; }

    public RadioConditionalValidationResult<PersonValidationResult> ValidateSeniorResponsibleOfficer()
    {

        var radioValidationResult = new RadioConditionalValidationResult<PersonValidationResult>()
        {
            IsValid = true,
            NestedValidationResult = new PersonValidationResult()
            {
                IsValid = true,
            }
        };


        if (IsDeputyDirectorTheSeniorResponsibleOfficer == null)
        {
            // Not yet declared if the DD is SRO - incomplete
            radioValidationResult.IsValid = false;
            radioValidationResult.RadioQuestionValidationErrors.Add(new ValidationError
            {
                FieldName = nameof(IsDeputyDirectorTheSeniorResponsibleOfficer),
                ErrorMessage = "Please select whether the deputy director is the senior responsible officer",
            });
        }
        else if (IsDeputyDirectorTheSeniorResponsibleOfficer == true)
        {
            if (SeniorResponsibleOfficer is not null)
            {
                radioValidationResult.IsValid = false;
                radioValidationResult.RadioQuestionValidationErrors.Add(new ValidationError
                {
                    FieldName = nameof(IsDeputyDirectorTheSeniorResponsibleOfficer),
                    ErrorMessage = "When declaring the deputy director to be the senior responsible officer, please do not provide the senior responsible officer's details.",
                });
                radioValidationResult.NestedValidationResult.IsValid = false;
                radioValidationResult.NestedValidationResult.ValidationErrors.Add(new ValidationError
                {
                    FieldName = nameof(SeniorResponsibleOfficer),
                    ErrorMessage = "SRO details must not be specified if the deputy director is declared as the SRO",
                });
            }
            else if (DeputyDirector is null)
            {
                // must provide dd details when declaring SRO is the DD
                radioValidationResult.IsValid = false;
                radioValidationResult.RadioQuestionValidationErrors.Add(new ValidationError
                {
                    FieldName = nameof(IsDeputyDirectorTheSeniorResponsibleOfficer),
                    ErrorMessage = "Please return to the deputy director question and enter the deputy director's details, before declaring the deputy director to be the senior responsible officer.",
                });
                radioValidationResult.RadioQuestionValidationErrors.Add(new ValidationError
                {
                    FieldName = nameof(DeputyDirector),
                    ErrorMessage = "When declaring the deputy director to be the senior responsible officer, please provide the deputy director's details.",
                });
            }
            else
            {
                // SRO declared as same as DD, thus SRO details not required.
            }
        }
        else if (IsDeputyDirectorTheSeniorResponsibleOfficer == false)
        {

            if (SeniorResponsibleOfficer is null)
            {
                radioValidationResult.NestedValidationResult.IsValid = false;
                radioValidationResult.NestedValidationResult.ValidationErrors.Add(new ValidationError
                {
                    FieldName = nameof(SeniorResponsibleOfficer),
                    ErrorMessage = "SeniorResponsibleOfficer is required",
                });
            }
            else
            {
                if (string.IsNullOrWhiteSpace(SeniorResponsibleOfficer.PersonalName))
                {
                    radioValidationResult.NestedValidationResult.IsValid = false;
                    radioValidationResult.NestedValidationResult.PersonalNameErrors.Add(new ValidationError
                    {
                        FieldName = nameof(SeniorResponsibleOfficer.PersonalName),
                        ErrorMessage = "Please enter the senior responsible officer's personal name.",
                    });
                }
                else
                {
                    if (SeniorResponsibleOfficer.PersonalName.Any(c => c < 32 || 126 < c))
                    {
                        /*
                         * ASCII char codes 32-126 are standard printable characters (upper and lower case letters, numbers, typical punctuation, etc)
                         * Add a warning if any characters fall outside this range
                         * Note not an error as it may be desirable to use non-standard characters (e.g., accented characters or emoji)
                         */
                        radioValidationResult.NestedValidationResult.IsValid = false;
                        radioValidationResult.NestedValidationResult.PersonalNameWarnings.Add(new ValidationWarning
                        {
                            FieldName = nameof(SeniorResponsibleOfficer.PersonalName),
                            WarningMessage =
                                "The senior responsible officer's personal name contains non-standard ASCII characters -- non-standard characters (e.g., \"smart quotes\" copy/pasted from MS Word) may not be intentional and may cause errors with values not be displayed correctly",
                        });
                    }
                }


                if (string.IsNullOrWhiteSpace(SeniorResponsibleOfficer.FamilyName))
                {
                    radioValidationResult.NestedValidationResult.IsValid = false;
                    radioValidationResult.NestedValidationResult.FamilyNameErrors.Add(new ValidationError
                    {
                        FieldName = nameof(SeniorResponsibleOfficer.FamilyName),
                        ErrorMessage = "Please enter the senior responsible officer's family name.",
                    });
                }
                else
                {
                    if (SeniorResponsibleOfficer.FamilyName.Any(c => c < 32 || 126 < c))
                    {
                        /*
                         * ASCII char codes 32-126 are standard printable characters (upper and lower case letters, numbers, typical punctuation, etc)
                         * Add a warning if any characters fall outside this range
                         * Note not an error as it may be desirable to use non-standard characters (e.g., accented characters or emoji)
                         */
                        radioValidationResult.NestedValidationResult.IsValid = false;
                        radioValidationResult.NestedValidationResult.FamilyNameWarnings.Add(new ValidationWarning
                        {
                            FieldName = nameof(SeniorResponsibleOfficer.FamilyName),
                            WarningMessage =
                                "The senior responsible officer's family name contains non-standard ASCII characters -- non-standard characters (e.g., \"smart quotes\" copy/pasted from MS Word) may not be intentional and may cause errors with values not be displayed correctly",
                        });
                    }
                }

                if (string.IsNullOrWhiteSpace(SeniorResponsibleOfficer.Email))
                {
                    radioValidationResult.NestedValidationResult.IsValid = false;
                    radioValidationResult.NestedValidationResult.EmailErrors.Add(new ValidationError
                    {
                        FieldName = nameof(SeniorResponsibleOfficer.Email),
                        ErrorMessage = "Please enter the senior responsible officer's email address.",
                    });
                }
                else
                {
                    if (SeniorResponsibleOfficer.Email.Any(c => c < 32 || 126 < c))
                    {
                        /*
                         * ASCII char codes 32-126 are standard printable characters (upper and lower case letters, numbers, typical punctuation, etc)
                         * Add a warning if any characters fall outside this range
                         * Note not an error as it may be desirable to use non-standard characters (e.g., accented characters or emoji)
                         */
                        radioValidationResult.NestedValidationResult.IsValid = false;
                        radioValidationResult.NestedValidationResult.EmailWarnings.Add(new ValidationWarning
                        {
                            FieldName = nameof(SeniorResponsibleOfficer.Email),
                            WarningMessage =
                                "The senior responsible officer's email address contains non-standard ASCII characters -- non-standard characters (e.g., \"smart quotes\" copy/pasted from MS Word) may not be intentional and may cause errors with values not be displayed correctly",
                        });
                    }

                    if (!EmailValidationUtilities.IsValidEmail(SeniorResponsibleOfficer.Email))
                    {
                        radioValidationResult.NestedValidationResult.IsValid = false;
                        radioValidationResult.NestedValidationResult.EmailErrors.Add(new ValidationError
                        {
                            FieldName = nameof(SeniorResponsibleOfficer.Email),
                            ErrorMessage =
                                "The senior responsible officer's email address is not recognised as being in a valid email format.",
                        });
                    }

                    if (!EmailValidationUtilities.IsValidDomain(SeniorResponsibleOfficer.Email))
                    {
                        radioValidationResult.NestedValidationResult.IsValid = false;
                        radioValidationResult.NestedValidationResult.EmailErrors.Add(new ValidationError
                        {
                            FieldName = nameof(SeniorResponsibleOfficer.Email),
                            ErrorMessage =
                                "The senior responsible officer's email address is not recognised as having a recognised/acceptable domain.",
                        });
                    }

                    // TODO: Validate email format
                    // TODO: Validate email domain is DfE
                }


                // TODO: Consider max length (probably 100 chars?)
                // TODO: Consider rejecting newlines with an error, as seniorResponsibleOfficer should normally be a short phrase only without newlines.
                // TODO: Consider handling of accented characters and multi-byte characters (e.g., emoji)
            }
        }

        return radioValidationResult;
    }



    public bool IsSeniorResponsibleOfficerComplete()
    {
        return IsDeputyDirectorTheSeniorResponsibleOfficer switch
        {
            null =>
                // Not yet declared if project code is known, thus is incomplete.
                false,
            true =>
                // SRO declared as same as DD, thus SRO details not required.
                // NOTE: Currently not valid to declare DD as SRO... AND also provide both details... May reconsider implementation later.
                (SeniorResponsibleOfficer is null && DeputyDirector is not null && DeputyDirector.IsComplete()),
            false =>
                // SRO declared as different to DD, thus SRO details required.
                (SeniorResponsibleOfficer is not null && SeniorResponsibleOfficer.IsComplete()),
        };
    }

    #endregion



    #region ProductManager

    public bool? HasProductOwnerManager { get; set; } = null;

    public Person? ProductOwnerManager { get; set; }

    public RadioConditionalValidationResult<PersonValidationResult> ValidateProductManager()
    {

        var radioValidationResult = new RadioConditionalValidationResult<PersonValidationResult>()
        {
            IsValid = true,
            NestedValidationResult = new PersonValidationResult()
            {
                IsValid = true,
            }
        };


        if (HasProductOwnerManager == null)
        {
            // Not yet declared if has a Product Manager - incomplete
            radioValidationResult.IsValid = false;
            radioValidationResult.RadioQuestionValidationErrors.Add(new ValidationError
            {
                FieldName = nameof(HasProductOwnerManager),
                ErrorMessage = "Please select whether the team has a product owner or product manager",
            });
        }
        else if (HasProductOwnerManager == true)
        {
            if (ProductOwnerManager is null)
            {
                radioValidationResult.IsValid = false;
                radioValidationResult.RadioQuestionValidationErrors.Add(new ValidationError
                {
                    FieldName = nameof(HasProductOwnerManager),
                    ErrorMessage = "When declaring the project has a product owner or product manager, please provide their details.",
                });
                radioValidationResult.NestedValidationResult.IsValid = false;
                radioValidationResult.NestedValidationResult.ValidationErrors.Add(new ValidationError
                {
                    FieldName = nameof(ProductOwnerManager),
                    ErrorMessage = "When declaring the project has a product owner or product manager, please provide their details.",
                });
            }
            else
            {
                if (string.IsNullOrWhiteSpace(ProductOwnerManager.PersonalName))
                {
                    radioValidationResult.NestedValidationResult.IsValid = false;
                    radioValidationResult.NestedValidationResult.PersonalNameErrors.Add(new ValidationError
                    {
                        FieldName = nameof(ProductOwnerManager.PersonalName),
                        ErrorMessage = "Please enter the product manager's personal name.",
                    });
                }
                else
                {
                    if (ProductOwnerManager.PersonalName.Any(c => c < 32 || 126 < c))
                    {
                        /*
                         * ASCII char codes 32-126 are standard printable characters (upper and lower case letters, numbers, typical punctuation, etc)
                         * Add a warning if any characters fall outside this range
                         * Note not an error as it may be desirable to use non-standard characters (e.g., accented characters or emoji)
                         */
                        radioValidationResult.NestedValidationResult.IsValid = false;
                        radioValidationResult.NestedValidationResult.PersonalNameWarnings.Add(new ValidationWarning
                        {
                            FieldName = nameof(ProductOwnerManager.PersonalName),
                            WarningMessage =
                                "The product manager's personal name contains non-standard ASCII characters -- non-standard characters (e.g., \"smart quotes\" copy/pasted from MS Word) may not be intentional and may cause errors with values not be displayed correctly",
                        });
                    }
                }

                if (string.IsNullOrWhiteSpace(ProductOwnerManager.FamilyName))
                {
                    radioValidationResult.NestedValidationResult.IsValid = false;
                    radioValidationResult.NestedValidationResult.FamilyNameErrors.Add(new ValidationError
                    {
                        FieldName = nameof(ProductOwnerManager.FamilyName),
                        ErrorMessage = "Please enter the product manager's family name.",
                    });
                }
                else
                {
                    if (ProductOwnerManager.FamilyName.Any(c => c < 32 || 126 < c))
                    {
                        /*
                         * ASCII char codes 32-126 are standard printable characters (upper and lower case letters, numbers, typical punctuation, etc)
                         * Add a warning if any characters fall outside this range
                         * Note not an error as it may be desirable to use non-standard characters (e.g., accented characters or emoji)
                         */
                        radioValidationResult.NestedValidationResult.IsValid = false;
                        radioValidationResult.NestedValidationResult.FamilyNameWarnings.Add(new ValidationWarning
                        {
                            FieldName = nameof(ProductOwnerManager.FamilyName),
                            WarningMessage =
                                "The product manager's family name contains non-standard ASCII characters -- non-standard characters (e.g., \"smart quotes\" copy/pasted from MS Word) may not be intentional and may cause errors with values not be displayed correctly",
                        });
                    }
                }

                if (string.IsNullOrWhiteSpace(ProductOwnerManager.Email))
                {
                    radioValidationResult.NestedValidationResult.IsValid = false;
                    radioValidationResult.NestedValidationResult.EmailErrors.Add(new ValidationError
                    {
                        FieldName = nameof(ProductOwnerManager.Email),
                        ErrorMessage = "Please enter the product manager's email address.",
                    });
                }
                else
                {
                    if (ProductOwnerManager.Email.Any(c => c < 32 || 126 < c))
                    {
                        /*
                         * ASCII char codes 32-126 are standard printable characters (upper and lower case letters, numbers, typical punctuation, etc)
                         * Add a warning if any characters fall outside this range
                         * Note not an error as it may be desirable to use non-standard characters (e.g., accented characters or emoji)
                         */
                        radioValidationResult.NestedValidationResult.IsValid = false;
                        radioValidationResult.NestedValidationResult.EmailWarnings.Add(new ValidationWarning
                        {
                            FieldName = nameof(ProductOwnerManager.Email),
                            WarningMessage =
                                "The product manager's email address contains non-standard ASCII characters -- non-standard characters (e.g., \"smart quotes\" copy/pasted from MS Word) may not be intentional and may cause errors with values not be displayed correctly",
                        });
                    }

                    if (!EmailValidationUtilities.IsValidEmail(ProductOwnerManager.Email))
                    {
                        radioValidationResult.NestedValidationResult.IsValid = false;
                        radioValidationResult.NestedValidationResult.EmailErrors.Add(new ValidationError
                        {
                            FieldName = nameof(ProductOwnerManager.Email),
                            ErrorMessage =
                                "The product manager's email address is not recognised as being in a valid email format.",
                        });
                    }

                    if (!EmailValidationUtilities.IsValidDomain(ProductOwnerManager.Email))
                    {
                        radioValidationResult.NestedValidationResult.IsValid = false;
                        radioValidationResult.NestedValidationResult.EmailErrors.Add(new ValidationError
                        {
                            FieldName = nameof(ProductOwnerManager.Email),
                            ErrorMessage =
                                "The product manager's email address is not recognised as having a recognised/acceptable domain.",
                        });
                    }

                    // TODO: Validate email format
                    // TODO: Validate email domain is DfE
                }


                // TODO: Consider max length (probably 100 chars?)
                // TODO: Consider rejecting newlines with an error, as productManager should normally be a short phrase only without newlines.
                // TODO: Consider handling of accented characters and multi-byte characters (e.g., emoji)

            }
        }
        else if (HasProductOwnerManager == false)
        {

            if (ProductOwnerManager is not null)
            {
                radioValidationResult.IsValid = false;
                radioValidationResult.RadioQuestionValidationErrors.Add(new ValidationError
                {
                    FieldName = nameof(HasProductOwnerManager),
                    ErrorMessage = "When declaring the project has no product owner or product manager, please do not provide the product owner or product manager's details.",
                });
                radioValidationResult.NestedValidationResult.IsValid = false;
                radioValidationResult.NestedValidationResult.ValidationErrors.Add(new ValidationError
                {
                    FieldName = nameof(ProductOwnerManager),
                    ErrorMessage = "When declaring the project has no product owner or product manager, please do not provide the product owner or product manager's details.",
                });
            }
            else
            {
                // No product manager declared, and no details provided -- valid.
            }
        }

        return radioValidationResult;
    }



    public bool IsProductOwnerManagerComplete()
    {
        return HasProductOwnerManager switch
        {
            null =>
                // Not yet declared if project code is known, thus is incomplete.
                false,
            true =>
                // Declared as known, and present - complete.
                (ProductOwnerManager is not null && ProductOwnerManager.IsComplete()),
            false =>
                // Declared as not known, and no details provided - complete.
                (ProductOwnerManager is null),
        };
    }

    #endregion


    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }


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
