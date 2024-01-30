using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceAssessmentService.Application.Database;
using ServiceAssessmentService.Domain.Model;
using AssessmentRequest = ServiceAssessmentService.Application.Database.Entities.AssessmentRequest;

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
        var entity = new AssessmentRequest
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

    
    

    public async Task<Domain.Model.Phase?> CreatePhaseAsync(Domain.Model.Phase newPhase)
    {
        var dbEntity = Database.Entities.Phase.FromDomain(newPhase);
        if(dbEntity is null)
        {
            _logger.LogWarning( "CreatePhaseAsync: failed to convert domain model to database entity when creating new phase");
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
            new() { Id=Guid.NewGuid(), Name = "Discovery", DisplayNameMidSentenceCase = "discovery", SortOrder = 10 },
            new() { Id=Guid.NewGuid(), Name = "Alpha", DisplayNameMidSentenceCase = "alpha", SortOrder = 20 },
            new() { Id=Guid.NewGuid(), Name = "Private Beta", DisplayNameMidSentenceCase = "private beta", SortOrder = 30 },
            new() { Id=Guid.NewGuid(), Name = "Public Beta", DisplayNameMidSentenceCase = "public beta", SortOrder = 40 },
            new() { Id=Guid.NewGuid(), Name = "Live", DisplayNameMidSentenceCase = "live", SortOrder = 50 },
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
            _logger.LogWarning("GetPhasesAsync: {EntityCount} entities, {DomainModelCount} domain models", dbEntityModels.Count, domainModels.Count);
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

    

    public async Task<Domain.Model.AssessmentType?> CreateAssessmentTypeAsync(Domain.Model.AssessmentType newAssessmentType)
    {
        var dbEntity = Database.Entities.AssessmentType.FromDomain(newAssessmentType);
        if(dbEntity is null)
        {
            _logger.LogWarning( "CreateAssessmentTypeAsync: failed to convert domain model to database entity when creating new assessmentType");
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
    
    
    
    
    

    public async Task<Domain.Model.Portfolio?> CreatePortfolioAsync(Domain.Model.Portfolio newPortfolio)
    {
        var dbEntity = Database.Entities.Portfolio.FromDomain(newPortfolio);
        if(dbEntity is null)
        {
            _logger.LogWarning( "CreatePortfolioAsync: failed to convert domain model to database entity when creating new portfolio");
            return null;
        }
        
        // Newly created entry does not yet have an ID -- add one.
        var newId = Guid.NewGuid();
        dbEntity.Id = newId;
        
        // Save it to the database
        _dbContext.Portfolios.Add(dbEntity);
        await _dbContext.SaveChangesAsync();
        
        // Return the newly created entity
        return await GetPortfolioByIdAsync(newId);
    }

    public async Task SeedPortfoliosAsync()
    {
        var dbPortfolios = await _dbContext.Portfolios.ToListAsync();
        if (dbPortfolios.Count > 0)
        {
            _logger.LogWarning("SeedPortfoliosAsync: {EntityCount} entities already exist, skip seeding", dbPortfolios.Count);
            return;
        }
        
        var domainPortfolios = new List<Domain.Model.Portfolio>()
        {
            new() { Id=Guid.NewGuid(), IsInternalGroup = true, Name = "Families group", DisplayNameMidSentenceCase = "families group", SortOrder = 10, },
            new() { Id=Guid.NewGuid(), IsInternalGroup = true, Name = "Funding group", DisplayNameMidSentenceCase = "funding group", SortOrder = 20, },
            new() { Id=Guid.NewGuid(), IsInternalGroup = true, Name = "Operations and Infrastructure group", DisplayNameMidSentenceCase = "operations and infrastructure group", SortOrder = 30, },
            new() { Id=Guid.NewGuid(), IsInternalGroup = true, Name = "Regions group", DisplayNameMidSentenceCase = "regions group", SortOrder = 40, },
            new() { Id=Guid.NewGuid(), IsInternalGroup = true, Name = "Schools group", DisplayNameMidSentenceCase = "schools group", SortOrder = 50, },
            new() { Id=Guid.NewGuid(), IsInternalGroup = true, Name = "Skills group", DisplayNameMidSentenceCase = "skills group", SortOrder = 60, },
            new() { Id=Guid.NewGuid(), IsInternalGroup = false, Name = "Standards and Testing agency", DisplayNameMidSentenceCase = "standards and testing agency", SortOrder = 70, },
            new() { Id=Guid.NewGuid(), IsInternalGroup = false, Name = "Student Loans Company", DisplayNameMidSentenceCase = "student loans company", SortOrder = 80, },
            new() { Id=Guid.NewGuid(), IsInternalGroup = false, Name = "Teacher Regulation Agency", DisplayNameMidSentenceCase = "teacher regulation agency", SortOrder = 90, },
        };
        
        var entityPortfolios = domainPortfolios
            .Select(Database.Entities.Portfolio.FromDomain)
            .ToList();
        
        _dbContext.Portfolios.AddRange(entityPortfolios);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<Domain.Model.Portfolio?> GetPortfolioByIdAsync(Guid id)
    {
        var dbEntity = await _dbContext.Portfolios
            .Where(e => e.Id == id)
            .SingleOrDefaultAsync();

        return dbEntity?.ToDomainModel();
    }

    public async Task<IEnumerable<Portfolio>> GetPortfoliosAsync()
    {
        var dbEntityModels = await _dbContext.Portfolios
            .ToListAsync();

        var domainModels = dbEntityModels
            .Select(e => e.ToDomainModel())
            .Where(e => e != null)
            .Select(e => e!)
            .ToList();

        if (dbEntityModels.Count != domainModels.Count)
        {
            _logger.LogWarning("GetPortfoliosAsync: {EntityCount} entities, {DomainModelCount} domain models", dbEntityModels.Count, domainModels.Count);
        }
        
        return domainModels;
    }

    public async Task UpdatePortfolioAsync(Domain.Model.Portfolio portfolio)
    {
        var dbEntity = await _dbContext.Portfolios
            .Where(e => e.Id == portfolio.Id)
            .SingleOrDefaultAsync();

        if (dbEntity is null)
        {
            return;
        }

        dbEntity.Name = portfolio.Name;
        dbEntity.DisplayNameMidSentenceCase = portfolio.DisplayNameMidSentenceCase;
        dbEntity.SortOrder = portfolio.SortOrder;
        dbEntity.IsInternalGroup = portfolio.IsInternalGroup;

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeletePortfolioAsync(Guid id)
    {
        var dbEntity = await _dbContext.Portfolios
            .Where(e => e.Id == id)
            .SingleOrDefaultAsync();

        if (dbEntity is null)
        {
            return;
        }

        _dbContext.Portfolios.Remove(dbEntity);
        await _dbContext.SaveChangesAsync();
    }
}
