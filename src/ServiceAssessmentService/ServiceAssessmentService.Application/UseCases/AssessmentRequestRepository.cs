﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceAssessmentService.Application.Database;
using ServiceAssessmentService.Domain.Model;
using ServiceAssessmentService.Domain.Model.Validations;
using AssessmentType = ServiceAssessmentService.Application.Database.Entities.AssessmentType;

namespace ServiceAssessmentService.Application.UseCases;

public class AssessmentRequestRepository
{
    private readonly DataContext _dbContext;
    private readonly ILogger<AssessmentRequestRepository> _logger;

    public AssessmentRequestRepository(DataContext dbContext, ILogger<AssessmentRequestRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }


    public async Task<IEnumerable<Domain.Model.AssessmentRequest>> GetAllAssessmentRequests()
    {
        var allAssessmentRequests = await _dbContext.AssessmentRequests
            .Include(e => e.PhaseConcluding)
            .Include(e => e.AssessmentTypeRequested)
            .Include(e => e.Portfolio)
            .Include(e => e.DeputyDirector)
            .Include(e => e.SeniorResponsibleOfficer)
            .Include(e => e.ProductOwnerManager)
            .Include(e => e.DeliveryManager)
            .Select(e => e.ToDomainModel())
            .ToListAsync();

        return allAssessmentRequests;
    }

    public async Task<Domain.Model.AssessmentRequest?> GetByIdAsync(Guid id)
    {
        var assessmentRequest = await _dbContext.AssessmentRequests
            .Include(e => e.PhaseConcluding)
            .Include(e => e.AssessmentTypeRequested)
            .Include(e => e.Portfolio)
            .Include(e => e.DeputyDirector)
            .Include(e => e.SeniorResponsibleOfficer)
            .Include(e => e.ProductOwnerManager)
            .Include(e => e.DeliveryManager)
            .Where(e => e.Id == id)
            .Select(e => e.ToDomainModel())
            .SingleOrDefaultAsync();

        return assessmentRequest;
    }

    public async Task<Domain.Model.AssessmentRequest?> CreateAsync()
    {
        var entity = Database.Entities.AssessmentRequest.FromDomain(new Domain.Model.AssessmentRequest()
        {
            Id = Guid.NewGuid(),
        });

        _dbContext.AssessmentRequests.Add(entity);
        await _dbContext.SaveChangesAsync();

        return entity.ToDomainModel();
    }

    public async Task<Domain.Model.AssessmentRequest?> DeleteAsync(Guid id)
    {
        var assessmentRequest = await _dbContext.AssessmentRequests
            .Where(e => e.Id == id)
            .SingleOrDefaultAsync();

        if (assessmentRequest is null)
        {
            return null;
        }

        _dbContext.AssessmentRequests.Remove(assessmentRequest);
        await _dbContext.SaveChangesAsync();

        // Return the deleted entity
        return assessmentRequest.ToDomainModel();
    }

    public async Task<Domain.Model.AssessmentRequest?> UpdateAsync(Domain.Model.AssessmentRequest assessmentRequest)
    {
        // Get the original
        var entity = await _dbContext.AssessmentRequests.SingleOrDefaultAsync(e => e.Id == assessmentRequest.Id);
        if (entity is null)
        {
            return null;
        }

        // Update the values
        entity.Name = assessmentRequest.Name;
        entity.Description = assessmentRequest.Description;
        entity.PhaseConcluding = Database.Entities.Phase.FromDomain(assessmentRequest.PhaseConcluding);
        entity.AssessmentTypeRequested = Database.Entities.AssessmentType.FromDomain(assessmentRequest.AssessmentType);
        entity.PhaseStartDate = assessmentRequest.PhaseStartDate;
        entity.PhaseEndDate = assessmentRequest.PhaseEndDate;

        // Save it to the database
        _dbContext.AssessmentRequests.Update(entity);
        await _dbContext.SaveChangesAsync();

        // Return the updated entity
        return await GetByIdAsync(entity.Id);
    }

    public async Task<TextValidationResult> UpdateDescriptionAsync(Guid id, string newDescription)
    {
        // Validate the assessment request being edited, exists
        var assessmentRequest = await _dbContext.AssessmentRequests.SingleOrDefaultAsync(e => e.Id == id);
        if (assessmentRequest is null)
        {
            var validationResult = new TextValidationResult();
            validationResult.ValidationErrors.Add(new()
            {
                FieldName = nameof(assessmentRequest.Id),
                ErrorMessage = $"Assessment request with ID {id} not found",
            });
            validationResult.IsValid = false;

            return validationResult;
        }

        // Do specific update
        assessmentRequest.Description = newDescription;

        // Validate the new value
        var domainModel = assessmentRequest.ToDomainModel();
        var validateDescriptionResult = domainModel.ValidateDescription();

        // If valid, save it to the database
        if (validateDescriptionResult.IsValid)
        {
            _dbContext.AssessmentRequests.Update(assessmentRequest);
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            _logger.LogInformation("Attempted to update assessment request with ID {Id}, but it was not valid", id);
        }

        // Return the result
        return validateDescriptionResult;
    }

    public async Task<TextValidationResult> UpdateNameAsync(Guid id, string newName)
    {
        // Validate the assessment request being edited, exists
        var assessmentRequest = await _dbContext.AssessmentRequests.SingleOrDefaultAsync(e => e.Id == id);
        if (assessmentRequest is null)
        {
            var validationResult = new TextValidationResult();
            validationResult.ValidationErrors.Add(new()
            {
                FieldName = nameof(assessmentRequest.Id),
                ErrorMessage = $"Assessment request with ID {id} not found",
            });
            validationResult.IsValid = false;

            return validationResult;
        }

        // Do specific update
        assessmentRequest.Name = newName;

        // Validate the new value
        var domainModel = assessmentRequest.ToDomainModel();
        var validateDescriptionResult = domainModel.ValidateName();

        // If valid, save it to the database
        if (validateDescriptionResult.IsValid)
        {
            _dbContext.AssessmentRequests.Update(assessmentRequest);
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            _logger.LogInformation("Attempted to update assessment request with ID {Id}, but it was not valid", id);
        }

        // Return the result
        return validateDescriptionResult;
    }

    public async Task<DateValidationResult> UpdateStartDateByPartsAsync(Guid id, string? newYear, string? newMonth,
        string? newDay)
    {
        // Validate the assessment request being edited, exists
        var assessmentRequest = await _dbContext.AssessmentRequests.SingleOrDefaultAsync(e => e.Id == id);
        if (assessmentRequest is null)
        {
            var validationResult = new DateValidationResult();
            validationResult.DateValidationErrors.Add(new()
            {
                FieldName = nameof(assessmentRequest.Id),
                ErrorMessage = $"Assessment request with ID {id} not found",
            });
            validationResult.IsValid = false;

            return validationResult;
        }

        DateOnly? proposedDate = null;
        (var datePartsValidationResult, proposedDate) = ValidateDateParts(newYear, newMonth, newDay, proposedDate);
        if (!datePartsValidationResult.IsValid)
        {
            return datePartsValidationResult;
        }

        // Do specific update
        assessmentRequest.PhaseStartDate = proposedDate;

        // Validate the new value
        var domainModel = assessmentRequest.ToDomainModel();
        var validateDescriptionResult = domainModel.ValidatePhaseStartDate();

        // If valid, save it to the database
        if (validateDescriptionResult.IsValid)
        {
            _dbContext.AssessmentRequests.Update(assessmentRequest);
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            _logger.LogInformation("Attempted to update assessment request with ID {Id}, but it was not valid", id);
        }

        // Return the result
        return validateDescriptionResult;
    }


    public async Task<RadioConditionalValidationResult<DateValidationResult>> UpdateEndDateByPartsAsync(Guid id,
        bool? isEndDateKnown, string? newYear, string? newMonth, string? newDay)
    {
        // Validate the assessment request being edited, exists
        var assessmentRequest = await _dbContext.AssessmentRequests.SingleOrDefaultAsync(e => e.Id == id);
        if (assessmentRequest is null)
        {
            var validationResult = new RadioConditionalValidationResult<DateValidationResult>()
            {
                IsValid = false,
                NestedValidationResult = new()
                {
                    IsValid = false,
                },
            };

            validationResult.RadioQuestionValidationErrors.Add(new()
            {
                FieldName = nameof(assessmentRequest.Id),
                ErrorMessage = $"Assessment request with ID {id} not found",
            });

            return validationResult;
        }


        if ((isEndDateKnown != true) && (newYear is not null || newMonth is not null || newDay is not null))
        {
            // Fail-fast if any date parts are given, but the end date is not declared as known (no/false, or blank/null)
            // TODO: Consider just discarding/dumping the date parts if user selects no/false, rather than returning an error
            // NOTE: Thorough validation is performed later (including if setting a date is valid in combination with the "is phase end date known" question)
            var validationResult = new RadioConditionalValidationResult<DateValidationResult>()
            {
                IsValid = false,
                NestedValidationResult = new DateValidationResult()
                {
                    IsValid = false,
                },
            };

            validationResult.RadioQuestionValidationErrors.Add(new()
            {
                FieldName = nameof(isEndDateKnown),
                ErrorMessage = "Declare the end date as known if providing a date",
            });
            validationResult.IsValid = false;
            validationResult.NestedValidationResult.IsValid = false;
            validationResult.NestedValidationResult.DateValidationErrors.Add(new()
            {
                FieldName = "Date",
                ErrorMessage = "Clear the date parts if declaring the end date is not known",
            });

            return validationResult;
        }


        DateOnly? proposedDate = null;
        if (newYear is not null || newMonth is not null || newDay is not null)
        {
            (var datePartsValidationResult, proposedDate) = ValidateDateParts(newYear, newMonth, newDay, proposedDate);
            // Date parts provided, but unable to parse them as a valid date
            // Fail-fast here, due to not being able to construct a DateOnly from the parts for assignment to the domain model
            if (!datePartsValidationResult.IsValid)
            {
                return new RadioConditionalValidationResult<DateValidationResult>()
                {
                    IsValid = true,
                    NestedValidationResult = datePartsValidationResult,
                };
            }
        }

        /*
         * TODO: Consider ways to remove this validation logic from here and keep it all within the domain
         * - e.g., using a method to assign the phase end date from parts would still require checking the result
         *   (unless the domain model has properties/fields representing individual date parts rather than a DateTime?)
         */

        // Do specific update
        assessmentRequest.IsPhaseEndDateKnown = isEndDateKnown;
        assessmentRequest.PhaseEndDate = proposedDate;

        // Validate the new value
        var domainModel = assessmentRequest.ToDomainModel();
        var validateDescriptionResult = domainModel.ValidatePhaseEndDate();

        // If valid, save it to the database
        if (validateDescriptionResult.IsValid)
        {
            _dbContext.AssessmentRequests.Update(assessmentRequest);
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            _logger.LogInformation("Attempted to update assessment request with ID {Id}, but it was not valid", id);
        }

        // Return the result
        return validateDescriptionResult;
    }

    private static (DateValidationResult datePartsValidationResult, DateOnly? proposedDate) ValidateDateParts(
        string? newYear,
        string? newMonth,
        string? newDay,
        DateOnly? proposedDate
    )
    {
        // Validate we can parse the new values as a date
        var datePartsValidationResult = new DateValidationResult();
        int day = 0;
        int month = 0;
        int year = 0;

        if (string.IsNullOrWhiteSpace(newYear))
        {
            datePartsValidationResult.IsValid = false;
            datePartsValidationResult.YearValidationErrors.Add(new()
            {
                FieldName = nameof(newYear),
                ErrorMessage = "Enter the year",
            });
        }
        else if (!int.TryParse(newYear, out year))
        {
            datePartsValidationResult.IsValid = false;
            datePartsValidationResult.YearValidationErrors.Add(new()
            {
                FieldName = nameof(newYear),
                ErrorMessage = "Enter a valid year",
            });
        }

        if (string.IsNullOrWhiteSpace(newMonth))
        {
            datePartsValidationResult.IsValid = false;
            datePartsValidationResult.MonthValidationErrors.Add(new()
            {
                FieldName = nameof(newMonth),
                ErrorMessage = "Enter the month",
            });
        }
        else if (!int.TryParse(newMonth, out month))
        {
            datePartsValidationResult.IsValid = false;
            datePartsValidationResult.MonthValidationErrors.Add(new()
            {
                FieldName = nameof(newMonth),
                ErrorMessage = "Enter a valid month",
            });
        }

        if (string.IsNullOrWhiteSpace(newDay))
        {
            datePartsValidationResult.IsValid = false;
            datePartsValidationResult.DayValidationErrors.Add(new()
            {
                FieldName = nameof(newDay),
                ErrorMessage = "Enter the day",
            });
        }
        else if (!int.TryParse(newDay, out day))
        {
            datePartsValidationResult.IsValid = false;
            datePartsValidationResult.DayValidationErrors.Add(new()
            {
                FieldName = nameof(newDay),
                ErrorMessage = "Enter a valid day",
            });
        }

        if (!datePartsValidationResult.IsValid)
        {
            return (datePartsValidationResult, proposedDate);
        }


        try
        {
            // If all date parts provided without error, don't try to validate each part individually (taking into account months and leap year etc)
            // ... instead, defer to C# to test if it's a valid date.
            proposedDate = new DateOnly(year, month, day);
        }
        catch (ArgumentOutOfRangeException)
        {
            var dateValidationResult = new DateValidationResult();
            dateValidationResult.IsValid = false;
            dateValidationResult.DateValidationErrors.Add(new()
            {
                FieldName = "Date",
                ErrorMessage = $"{day}/{month}/{year} is not recognised as a valid day/month/year date between {AssessmentRequest.EarliestPermittedPhaseEndDate} and {AssessmentRequest.LatestPermittedPhaseEndDate}.",
            });

            return (dateValidationResult, proposedDate);
        }

        // If we got this far, the given date parts have been able to be parsed into a valid date
        return (datePartsValidationResult, proposedDate);
    }

    public async Task<RadioConditionalValidationResult<TextValidationResult>> UpdateProjectCode(Guid id, bool? newIsProjectCodeKnown, string? newProjectCode)
    {
        // Validate the assessment request being edited, exists
        var assessmentRequest = await _dbContext.AssessmentRequests.SingleOrDefaultAsync(e => e.Id == id);
        if (assessmentRequest is null)
        {
            var validationResult = new RadioConditionalValidationResult<TextValidationResult>()
            {
                IsValid = false,
                NestedValidationResult = new()
                {
                    IsValid = false,
                },
            };

            validationResult.RadioQuestionValidationErrors.Add(new()
            {
                FieldName = nameof(assessmentRequest.Id),
                ErrorMessage = $"Assessment request with ID {id} not found",
            });

            return validationResult;
        }

        // Do specific update
        assessmentRequest.IsProjectCodeKnown = newIsProjectCodeKnown;
        assessmentRequest.ProjectCode = newProjectCode;

        // Validate the new value
        var domainModel = assessmentRequest.ToDomainModel();
        var validateProjectCodeResult = domainModel.ValidateProjectCode();

        // If valid, save it to the database
        if (validateProjectCodeResult.IsValid)
        {
            _dbContext.AssessmentRequests.Update(assessmentRequest);
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            _logger.LogInformation("Attempted to update assessment request with ID {Id}, but it was not valid", id);
        }

        // Return the result
        return validateProjectCodeResult;
    }

    public async Task<RadioValidationResult> UpdateAssessmentTypeAsync(Guid id, string? newAssessmentTypeId)
    {
        // Validate the assessment request being edited, exists
        var assessmentRequest = await _dbContext.AssessmentRequests.SingleOrDefaultAsync(e => e.Id == id);
        if (assessmentRequest is null)
        {
            var validationResult = new RadioValidationResult();
            validationResult.ValidationErrors.Add(new()
            {
                FieldName = nameof(assessmentRequest.Id),
                ErrorMessage = $"Assessment request with ID {id} not found",
            });
            validationResult.IsValid = false;

            return validationResult;
        }

        var availableAssessmentTypes = await _dbContext.AssessmentTypes.ToListAsync();

        // Do specific update
        if (newAssessmentTypeId is null)
        {
            assessmentRequest.AssessmentTypeRequestedId = null;
            assessmentRequest.AssessmentTypeRequested = null;
        }
        else
        {
            assessmentRequest.AssessmentTypeRequestedId = Guid.Parse(newAssessmentTypeId);
            assessmentRequest.AssessmentTypeRequested = availableAssessmentTypes.SingleOrDefault(e => e.Id.ToString() == newAssessmentTypeId);
        }

        // Validate the new value
        var domainModel = assessmentRequest.ToDomainModel();
        var validateDescriptionResult = domainModel.ValidateAssessmentType(availableAssessmentTypes.Select(x => x.ToDomainModel()));

        // If valid, save it to the database
        if (validateDescriptionResult.IsValid)
        {
            _dbContext.AssessmentRequests.Update(assessmentRequest);
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            _logger.LogInformation("Attempted to update assessment request with ID {Id}, but it was not valid", id);
        }

        // Return the result
        return validateDescriptionResult;
    }

    public async Task<RadioValidationResult> UpdatePhaseConcludingAsync(Guid id, string? newPhaseConcludingId)
    {
        // Validate the assessment request being edited, exists
        var assessmentRequest = await _dbContext.AssessmentRequests.SingleOrDefaultAsync(e => e.Id == id);
        if (assessmentRequest is null)
        {
            var validationResult = new RadioValidationResult();
            validationResult.ValidationErrors.Add(new()
            {
                FieldName = nameof(assessmentRequest.Id),
                ErrorMessage = $"Assessment request with ID {id} not found",
            });
            validationResult.IsValid = false;

            return validationResult;
        }

        var availablePhaseConcludings = await _dbContext.Phases.ToListAsync();

        // Do specific update
        if (newPhaseConcludingId is null)
        {
            assessmentRequest.PhaseConcludingId = null;
            assessmentRequest.PhaseConcluding = null;
        }
        else
        {
            assessmentRequest.PhaseConcludingId = Guid.Parse(newPhaseConcludingId);
            assessmentRequest.PhaseConcluding = availablePhaseConcludings.SingleOrDefault(e => e.Id.ToString() == newPhaseConcludingId);
        }

        // Validate the new value
        var domainModel = assessmentRequest.ToDomainModel();
        var validateDescriptionResult = domainModel.ValidatePhaseConcluding(availablePhaseConcludings.Select(x => x.ToDomainModel()));

        // If valid, save it to the database
        if (validateDescriptionResult.IsValid)
        {
            _dbContext.AssessmentRequests.Update(assessmentRequest);
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            _logger.LogInformation("Attempted to update assessment request with ID {Id}, but it was not valid", id);
        }

        // Return the result
        return validateDescriptionResult;
    }

    public async Task<RadioValidationResult> UpdatePortfolioAsync(Guid id, string? newPortfolioId)
    {
        // Validate the assessment request being edited, exists
        var assessmentRequest = await _dbContext.AssessmentRequests.SingleOrDefaultAsync(e => e.Id == id);
        if (assessmentRequest is null)
        {
            var validationResult = new RadioValidationResult();
            validationResult.ValidationErrors.Add(new()
            {
                FieldName = nameof(assessmentRequest.Id),
                ErrorMessage = $"Assessment request with ID {id} not found",
            });
            validationResult.IsValid = false;

            return validationResult;
        }

        var availablePortfolios = await _dbContext.Portfolios.ToListAsync();

        // Do specific update
        if (newPortfolioId is null)
        {
            assessmentRequest.PortfolioId = null;
            assessmentRequest.Portfolio = null;
        }
        else
        {
            assessmentRequest.PortfolioId = Guid.Parse(newPortfolioId);
            assessmentRequest.Portfolio = availablePortfolios.SingleOrDefault(e => e.Id.ToString() == newPortfolioId);
        }

        // Validate the new value
        var domainModel = assessmentRequest.ToDomainModel();
        var validateDescriptionResult = domainModel.ValidatePortfolio(availablePortfolios.Select(x => x.ToDomainModel()));

        // If valid, save it to the database
        if (validateDescriptionResult.IsValid)
        {
            _dbContext.AssessmentRequests.Update(assessmentRequest);
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            _logger.LogInformation("Attempted to update assessment request with ID {Id}, but it was not valid", id);
        }

        // Return the result
        return validateDescriptionResult;
    }

    public async Task<PersonValidationResult> UpdateDeputyDirectorAsync(Guid id, string newPersonalName, string newFamilyName, string newEmail)
    {
        // Validate the assessment request being edited, exists
        var assessmentRequest = await _dbContext.AssessmentRequests.SingleOrDefaultAsync(e => e.Id == id);
        if (assessmentRequest is null)
        {
            var validationResult = new PersonValidationResult();
            validationResult.ValidationErrors.Add(new()
            {
                FieldName = nameof(assessmentRequest.Id),
                ErrorMessage = $"Assessment request with ID {id} not found",
            });
            validationResult.IsValid = false;

            return validationResult;
        }

        // Do specific update
        assessmentRequest.DeputyDirector = new Database.Entities.Person()
        {
            PersonalName = newPersonalName,
            FamilyName = newFamilyName,
            Email = newEmail,
        };

        // Validate the new value
        var domainModel = assessmentRequest.ToDomainModel();
        var validateDescriptionResult = domainModel.ValidateDeputyDirector();

        // If valid, save it to the database
        if (validateDescriptionResult.IsValid)
        {
            _dbContext.AssessmentRequests.Update(assessmentRequest);
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            _logger.LogInformation("Attempted to update assessment request with ID {Id}, but it was not valid", id);
        }

        // Return the result
        return validateDescriptionResult;
    }

    public async Task<RadioConditionalValidationResult<PersonValidationResult>> UpdateSeniorResponsibleOfficerAsync(Guid id, bool? isDdTheSro, string newPersonalName, string newFamilyName, string newEmail)
    {

        // Validate the assessment request being edited, exists
        var assessmentRequest = await _dbContext.AssessmentRequests
            .Include(e => e.DeputyDirector)
            .Include(e => e.SeniorResponsibleOfficer)
            .SingleOrDefaultAsync(e => e.Id == id);
        if (assessmentRequest is null)
        {
            var validationResult = new RadioConditionalValidationResult<PersonValidationResult>
            {
                NestedValidationResult = new()
                {
                    IsValid = true,
                },
            };
            validationResult.RadioQuestionValidationErrors.Add(new()
            {
                FieldName = nameof(assessmentRequest.Id),
                ErrorMessage = $"Assessment request with ID {id} not found",
            });
            validationResult.IsValid = false;

            return validationResult;
        }

        // Do specific update
        assessmentRequest.IsDeputyDirectorTheSeniorResponsibleOfficer = isDdTheSro;

        if (
            string.IsNullOrWhiteSpace(newPersonalName)
            && string.IsNullOrWhiteSpace(newFamilyName)
            && string.IsNullOrWhiteSpace(newEmail)
        )
        {
            // No person details provided, therefore declaring that the SRO is null.
            assessmentRequest.SeniorResponsibleOfficer = null;
        }
        else
        {
            // At least one detail about the SRO has been provided, therefore...
            // Either use the existing SRO (fetched via query above) and overwrite details, or create a new one and supply details for the first time.
            assessmentRequest.SeniorResponsibleOfficer ??= new Database.Entities.Person();
            assessmentRequest.SeniorResponsibleOfficer.PersonalName = newPersonalName;
            assessmentRequest.SeniorResponsibleOfficer.FamilyName = newFamilyName;
            assessmentRequest.SeniorResponsibleOfficer.Email = newEmail;
        }


        // Validate the new value
        var domainModel = assessmentRequest.ToDomainModel();
        var validateDescriptionResult = domainModel.ValidateSeniorResponsibleOfficer();

        // If valid, save it to the database
        if (validateDescriptionResult.IsValid)
        {
            _dbContext.AssessmentRequests.Update(assessmentRequest);
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            _logger.LogInformation("Attempted to update assessment request with ID {Id}, but it was not valid", id);
        }

        // Return the result
        return validateDescriptionResult;
    }

    public async Task<RadioConditionalValidationResult<PersonValidationResult>> UpdateProductOwnerManagerAsync(Guid id, bool? hasProductOwnerManager, string newPersonalName, string newFamilyName, string newEmail)
    {
        // Validate the assessment request being edited, exists
        var assessmentRequest = await _dbContext.AssessmentRequests
            .Include(e => e.ProductOwnerManager)
            .SingleOrDefaultAsync(e => e.Id == id);
        if (assessmentRequest is null)
        {
            var validationResult = new RadioConditionalValidationResult<PersonValidationResult>
            {
                NestedValidationResult = new()
                {
                    IsValid = true,
                },
            };
            validationResult.RadioQuestionValidationErrors.Add(new()
            {
                FieldName = nameof(assessmentRequest.Id),
                ErrorMessage = $"Assessment request with ID {id} not found",
            });
            validationResult.IsValid = false;

            return validationResult;
        }

        // Do specific update
        assessmentRequest.HasProductOwnerManager = hasProductOwnerManager;

        if (
            string.IsNullOrWhiteSpace(newPersonalName)
            && string.IsNullOrWhiteSpace(newFamilyName)
            && string.IsNullOrWhiteSpace(newEmail)
        )
        {
            // No person details provided, therefore declaring that the SRO is null.
            assessmentRequest.ProductOwnerManager = null;
        }
        else
        {
            // At least one detail about the SRO has been provided, therefore...
            // Either use the existing SRO (fetched via query above) and overwrite details, or create a new one and supply details for the first time.
            assessmentRequest.ProductOwnerManager ??= new Database.Entities.Person();
            assessmentRequest.ProductOwnerManager.PersonalName = newPersonalName;
            assessmentRequest.ProductOwnerManager.FamilyName = newFamilyName;
            assessmentRequest.ProductOwnerManager.Email = newEmail;
        }

        // Validate the new value
        var domainModel = assessmentRequest.ToDomainModel();
        var validateDescriptionResult = domainModel.ValidateProductOwnerManager();

        // If valid, save it to the database
        if (validateDescriptionResult.IsValid)
        {
            _dbContext.AssessmentRequests.Update(assessmentRequest);
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            _logger.LogInformation("Attempted to update assessment request with ID {Id}, but it was not valid", id);
        }

        // Return the result
        return validateDescriptionResult;
    }

    public async Task<RadioConditionalValidationResult<PersonValidationResult>> UpdateDeliveryManagerAsync(Guid id, bool? hasProductOwnerManager, string newPersonalName, string newFamilyName, string newEmail)
    {
        // Validate the assessment request being edited, exists
        var assessmentRequest = await _dbContext.AssessmentRequests
            .Include(e => e.DeliveryManager)
            .SingleOrDefaultAsync(e => e.Id == id);
        if (assessmentRequest is null)
        {
            var validationResult = new RadioConditionalValidationResult<PersonValidationResult>
            {
                NestedValidationResult = new()
                {
                    IsValid = true,
                },
            };
            validationResult.RadioQuestionValidationErrors.Add(new()
            {
                FieldName = nameof(assessmentRequest.Id),
                ErrorMessage = $"Assessment request with ID {id} not found",
            });
            validationResult.IsValid = false;

            return validationResult;
        }

        // Do specific update
        assessmentRequest.HasDeliveryManager = hasProductOwnerManager;

        if (
            string.IsNullOrWhiteSpace(newPersonalName)
            && string.IsNullOrWhiteSpace(newFamilyName)
            && string.IsNullOrWhiteSpace(newEmail)
        )
        {
            // No person details provided, therefore declaring that the SRO is null.
            assessmentRequest.DeliveryManager = null;
        }
        else
        {
            // At least one detail about the SRO has been provided, therefore...
            // Either use the existing SRO (fetched via query above) and overwrite details, or create a new one and supply details for the first time.
            assessmentRequest.DeliveryManager ??= new Database.Entities.Person();
            assessmentRequest.DeliveryManager.PersonalName = newPersonalName;
            assessmentRequest.DeliveryManager.FamilyName = newFamilyName;
            assessmentRequest.DeliveryManager.Email = newEmail;
        }

        // Validate the new value
        var domainModel = assessmentRequest.ToDomainModel();
        var validateDescriptionResult = domainModel.ValidateProductOwnerManager();

        // If valid, save it to the database
        if (validateDescriptionResult.IsValid)
        {
            _dbContext.AssessmentRequests.Update(assessmentRequest);
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            _logger.LogInformation("Attempted to update assessment request with ID {Id}, but it was not valid", id);
        }

        // Return the result
        return validateDescriptionResult;
    }
}
