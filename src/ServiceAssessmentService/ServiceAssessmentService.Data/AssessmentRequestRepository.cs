using Microsoft.EntityFrameworkCore;

namespace ServiceAssessmentService.Data;

public class AssessmentRequestRepository
{
    private readonly DataContext _dbContext;

    public AssessmentRequestRepository(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Domain.Model.AssessmentRequest>> GetAllAssessmentRequests()
    {
        var allAssessmentRequests = await _dbContext.AssessmentRequests
            .Select(e => e.ToDomainModel())
            .ToListAsync();

        return allAssessmentRequests;
    }

    public async Task<Domain.Model.AssessmentRequest?> GetByIdAsync(Guid id)
    {
        var assessmentRequest = await _dbContext.AssessmentRequests
            .Where(e => e.Id == id)
            .Select(e => e.ToDomainModel())
            .SingleOrDefaultAsync();

        return assessmentRequest;
    }


    public async Task<Domain.Model.AssessmentRequest?> CreateAsync(Domain.Model.AssessmentRequest assessmentRequest)
    {
        var entity = new Entities.AssessmentRequest
        {
            Id = assessmentRequest.Id,
            Name = assessmentRequest.Name,
            PhaseConcluding = assessmentRequest.PhaseConcluding,
            AssessmentType = assessmentRequest.AssessmentType,
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
}
