using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceAssessmentService.Application.Database;
using ServiceAssessmentService.Domain.Model;
using ServiceAssessmentService.Domain.Model.Validations;

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
            .Where(e => e.Id == id)
            .Select(e => e.ToDomainModel())
            .SingleOrDefaultAsync();

        return assessmentRequest;
    }

    public async Task<Domain.Model.AssessmentRequest?> CreateAsync(Domain.Model.AssessmentRequest assessmentRequest)
    {
        var entity = new Database.Entities.AssessmentRequest
        {
            Id = assessmentRequest.Id,
            Name = assessmentRequest.Name,
            PhaseConcluding = Database.Entities.Phase.FromDomain(assessmentRequest.PhaseConcluding),
            AssessmentTypeRequested = Database.Entities.AssessmentType.FromDomain(assessmentRequest.AssessmentType),
            PhaseStartDate = assessmentRequest.PhaseStartDate,
            PhaseEndDate = assessmentRequest.PhaseEndDate,
            Description = assessmentRequest.Description,
        };

        _dbContext.AssessmentRequests.Add(entity);
        await _dbContext.SaveChangesAsync();

        return assessmentRequest;
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
        if(!datePartsValidationResult.IsValid)
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


    // TODO: Figure out the nested validation result logic...
    public async Task<DateValidationResult> UpdateEndDateByPartsAsync(Guid id, bool? isEndDateKnown, string? newYear, string? newMonth, string? newDay)
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


        // Validate the outer radio first
        if (isEndDateKnown is null)
        {
            var validationResult = new DateValidationResult();
            validationResult.IsValid = false;
            validationResult.DateValidationErrors.Add(new()
            {
                FieldName = nameof(isEndDateKnown),
                ErrorMessage = $"Select whether the end date is known",
            });

            return validationResult;
        }
        else if (isEndDateKnown == false && (newYear != null || newMonth != null || newDay != null))
        {
            var validationResult = new DateValidationResult();
            validationResult.IsValid = false;
            validationResult.DateValidationErrors.Add(new()
            {
                FieldName = nameof(isEndDateKnown),
                ErrorMessage = $"End date is declared as not known, but date parts have been provided",
            });

            return validationResult;
        }
        else if (isEndDateKnown == true)
        {
            (var datePartsValidationResult, proposedDate) = ValidateDateParts(newYear, newMonth, newDay, proposedDate);
            
            if(!datePartsValidationResult.IsValid)
            {
                return datePartsValidationResult;
            }
        }


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
        string? newYear, string? newMonth, string? newDay, DateOnly? proposedDate)
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
                ErrorMessage = $"{day}/{month}/{year} is not recognised as a valid day/month/year date",
            });

            return (datePartsValidationResult, proposedDate);
        }

        return (datePartsValidationResult, proposedDate);
    }
}
