using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceAssessmentService.Application.Database;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.Application.UseCases;

public class AssessmentTypeRepository
{
    private readonly DataContext _dbContext;
    private readonly ILogger<AssessmentTypeRepository> _logger;

    public AssessmentTypeRepository(DataContext dbContext, ILogger<AssessmentTypeRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }


    public async Task<Domain.Model.AssessmentType?> CreateAssessmentTypeAsync(Domain.Model.AssessmentType newAssessmentType)
    {
        var dbEntity = Database.Entities.AssessmentType.FromDomain(newAssessmentType);
        if (dbEntity is null)
        {
            _logger.LogWarning("CreateAssessmentTypeAsync: failed to convert domain model to database entity when creating new assessmentType");
            return null;
        }

        // Newly created entry does not yet have an ID -- add one.
        var newId = Guid.NewGuid();
        dbEntity.Id = newId;

        // Save it to the database
        _dbContext.AssessmentTypes.Add(dbEntity);
        await _dbContext.SaveChangesAsync();

        // Return the newly created entity
        return await GetAssessmentTypeByIdAsync(newId);
    }

    public async Task SeedAssessmentTypesAsync()
    {
        var dbAssessmentTypes = await _dbContext.AssessmentTypes.ToListAsync();
        if (dbAssessmentTypes.Count > 0)
        {
            _logger.LogWarning("SeedAssessmentTypesAsync: {EntityCount} entities already exist, skip seeding", dbAssessmentTypes.Count);
            return;
        }

        var domainAssessmentTypes = new List<Domain.Model.AssessmentType>()
        {
            new() { Id=Guid.NewGuid(), Name = "Peer review", DisplayNameMidSentenceCase = "peer review", SortOrder = 10 },
            new() { Id=Guid.NewGuid(), Name = "Mock assessment", DisplayNameMidSentenceCase = "mock assessment", SortOrder = 20 },
            new() { Id=Guid.NewGuid(), Name = "Continuous assessment", DisplayNameMidSentenceCase = "continuous assessment", SortOrder = 30 },
            new() { Id=Guid.NewGuid(), Name = "Internal (DfE) assessment", DisplayNameMidSentenceCase = "internal (DfE) assessment", SortOrder = 40 },
            new() { Id=Guid.NewGuid(), Name = "Cross government assessment", DisplayNameMidSentenceCase = "cross government assessment", SortOrder = 50 },
        };

        var entityAssessmentTypes = domainAssessmentTypes
            .Select(Database.Entities.AssessmentType.FromDomain)
            .ToList();

        _dbContext.AssessmentTypes.AddRange(entityAssessmentTypes);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Domain.Model.AssessmentType?> GetAssessmentTypeByIdAsync(Guid id)
    {
        var dbEntity = await _dbContext.AssessmentTypes
            .Where(e => e.Id == id)
            .SingleOrDefaultAsync();

        return dbEntity?.ToDomainModel();
    }

    public async Task<IEnumerable<AssessmentType>> GetAssessmentTypesAsync()
    {
        var dbEntityModels = await _dbContext.AssessmentTypes
            .ToListAsync();

        var domainModels = dbEntityModels
            .Select(e => e.ToDomainModel())
            .Where(e => e != null)
            .Select(e => e!)
            .ToList();

        if (dbEntityModels.Count != domainModels.Count)
        {
            _logger.LogWarning("GetAssessmentTypesAsync: {EntityCount} entities, {DomainModelCount} domain models", dbEntityModels.Count, domainModels.Count);
        }

        return domainModels;
    }

    public async Task UpdateAssessmentTypeAsync(Domain.Model.AssessmentType assessmentType)
    {
        var dbEntity = await _dbContext.AssessmentTypes
            .Where(e => e.Id == assessmentType.Id)
            .SingleOrDefaultAsync();

        if (dbEntity is null)
        {
            return;
        }

        dbEntity.Name = assessmentType.Name;
        dbEntity.DisplayNameMidSentenceCase = assessmentType.DisplayNameMidSentenceCase;
        dbEntity.SortOrder = assessmentType.SortOrder;

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAssessmentTypeAsync(Guid id)
    {
        var dbEntity = await _dbContext.AssessmentTypes
            .Where(e => e.Id == id)
            .SingleOrDefaultAsync();

        if (dbEntity is null)
        {
            return;
        }

        _dbContext.AssessmentTypes.Remove(dbEntity);
        await _dbContext.SaveChangesAsync();
    }


}
