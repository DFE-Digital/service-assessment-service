using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceAssessmentService.Application.Database;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.Application.UseCases;

public class PhaseRepository
{
    private readonly DataContext _dbContext;
    private readonly ILogger<PhaseRepository> _logger;

    public PhaseRepository(DataContext dbContext, ILogger<PhaseRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }


    public async Task<Domain.Model.Phase?> CreatePhaseAsync(Domain.Model.Phase newPhase)
    {
        var dbEntity = Database.Entities.Phase.FromDomain(newPhase);
        if (dbEntity is null)
        {
            _logger.LogWarning(
                "CreatePhaseAsync: failed to convert domain model to database entity when creating new phase");
            return null;
        }

        // Newly created entry does not yet have an ID -- add one.
        var newId = Guid.NewGuid();
        dbEntity.Id = newId;

        // Save it to the database
        _dbContext.Phases.Add(dbEntity);
        await _dbContext.SaveChangesAsync();

        // Return the newly created entity
        return await GetPhaseByIdAsync(newId);
    }

    public async Task SeedPhasesAsync()
    {
        var dbPhases = await _dbContext.Phases.ToListAsync();
        if (dbPhases.Count > 0)
        {
            _logger.LogWarning("SeedPhasesAsync: {EntityCount} entities already exist, skip seeding", dbPhases.Count);
            return;
        }

        var domainPhases = new List<Domain.Model.Phase>()
        {
            new() {Id = Guid.NewGuid(), Name = "Discovery", DisplayNameMidSentenceCase = "discovery", SortOrder = 10},
            new() {Id = Guid.NewGuid(), Name = "Alpha", DisplayNameMidSentenceCase = "alpha", SortOrder = 20},
            new()
            {
                Id = Guid.NewGuid(), Name = "Private Beta", DisplayNameMidSentenceCase = "private beta", SortOrder = 30
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "Public Beta", DisplayNameMidSentenceCase = "public beta", SortOrder = 40
            },
            new() {Id = Guid.NewGuid(), Name = "Live", DisplayNameMidSentenceCase = "live", SortOrder = 50},
        };

        var entityPhases = domainPhases
            .Select(Database.Entities.Phase.FromDomain)
            .ToList();

        _dbContext.Phases.AddRange(entityPhases);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Domain.Model.Phase?> GetPhaseByIdAsync(Guid id)
    {
        var dbEntity = await _dbContext.Phases
            .Where(e => e.Id == id)
            .SingleOrDefaultAsync();

        return dbEntity?.ToDomainModel();
    }

    public async Task<IEnumerable<Phase>> GetPhasesAsync()
    {
        var dbEntityModels = await _dbContext.Phases
            .ToListAsync();

        var domainModels = dbEntityModels
            .Select(e => e.ToDomainModel())
            .Where(e => e != null)
            .Select(e => e!)
            .ToList();

        if (dbEntityModels.Count != domainModels.Count)
        {
            _logger.LogWarning("GetPhasesAsync: {EntityCount} entities, {DomainModelCount} domain models",
                dbEntityModels.Count, domainModels.Count);
        }

        return domainModels;
    }

    public async Task UpdatePhaseAsync(Domain.Model.Phase phase)
    {
        var dbEntity = await _dbContext.Phases
            .Where(e => e.Id == phase.Id)
            .SingleOrDefaultAsync();

        if (dbEntity is null)
        {
            return;
        }

        dbEntity.Name = phase.Name;
        dbEntity.DisplayNameMidSentenceCase = phase.DisplayNameMidSentenceCase;
        dbEntity.SortOrder = phase.SortOrder;

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeletePhaseAsync(Guid id)
    {
        var dbEntity = await _dbContext.Phases
            .Where(e => e.Id == id)
            .SingleOrDefaultAsync();

        if (dbEntity is null)
        {
            return;
        }

        _dbContext.Phases.Remove(dbEntity);
        await _dbContext.SaveChangesAsync();
    }
}
