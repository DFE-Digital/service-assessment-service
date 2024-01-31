using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceAssessmentService.Application.Database;
using ServiceAssessmentService.Domain.Model;

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
                ErrorMessage = $"Assessment request with ID {id} not found"
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
                ErrorMessage = $"Assessment request with ID {id} not found"
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
}

