using Microsoft.EntityFrameworkCore;
using ServiceAssessmentService.Application.Database;
using AssessmentRequest = ServiceAssessmentService.Application.Database.Entities.AssessmentRequest;

namespace ServiceAssessmentService.Application.UseCases;

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
        var assessmentRequest1 = await _dbContext.AssessmentRequests
            .Where(e => e.Id == id)
            .SingleOrDefaultAsync();

        var assessmentRequest = assessmentRequest1?.ToDomainModel();

        return assessmentRequest;
    }

    public async Task<Domain.Model.AssessmentRequest?> CreateAsync(Domain.Model.AssessmentRequest assessmentRequest)
    {
        var entity = new AssessmentRequest
        {
            Id = assessmentRequest.Id,
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

        var entity = await _dbContext.AssessmentRequests
            .SingleOrDefaultAsync(e => e.Id == assessmentRequest.Id);

        if (entity is null)
        {
            return null;
        }

        _dbContext.AssessmentRequests.Update(entity);
        await _dbContext.SaveChangesAsync();

        return assessmentRequest;
    }

}
