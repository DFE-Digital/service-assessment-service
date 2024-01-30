using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceAssessmentService.Application.Database;
using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.Application.UseCases;

public class PortfolioRepository
{
    private readonly DataContext _dbContext;
    private readonly ILogger<PortfolioRepository> _logger;

    public PortfolioRepository(DataContext dbContext, ILogger<PortfolioRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }


    public async Task<Domain.Model.Portfolio?> CreatePortfolioAsync(Domain.Model.Portfolio newPortfolio)
    {
        var dbEntity = Database.Entities.Portfolio.FromDomain(newPortfolio);
        if (dbEntity is null)
        {
            _logger.LogWarning(
                "CreatePortfolioAsync: failed to convert domain model to database entity when creating new portfolio");
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
            _logger.LogWarning("SeedPortfoliosAsync: {EntityCount} entities already exist, skip seeding",
                dbPortfolios.Count);
            return;
        }

        var domainPortfolios = new List<Domain.Model.Portfolio>()
        {
            new()
            {
                Id = Guid.NewGuid(), IsInternalGroup = true, Name = "Families group",
                DisplayNameMidSentenceCase = "families group", SortOrder = 10,
            },
            new()
            {
                Id = Guid.NewGuid(), IsInternalGroup = true, Name = "Funding group",
                DisplayNameMidSentenceCase = "funding group", SortOrder = 20,
            },
            new()
            {
                Id = Guid.NewGuid(), IsInternalGroup = true, Name = "Operations and Infrastructure group",
                DisplayNameMidSentenceCase = "operations and infrastructure group", SortOrder = 30,
            },
            new()
            {
                Id = Guid.NewGuid(), IsInternalGroup = true, Name = "Regions group",
                DisplayNameMidSentenceCase = "regions group", SortOrder = 40,
            },
            new()
            {
                Id = Guid.NewGuid(), IsInternalGroup = true, Name = "Schools group",
                DisplayNameMidSentenceCase = "schools group", SortOrder = 50,
            },
            new()
            {
                Id = Guid.NewGuid(), IsInternalGroup = true, Name = "Skills group",
                DisplayNameMidSentenceCase = "skills group", SortOrder = 60,
            },
            new()
            {
                Id = Guid.NewGuid(), IsInternalGroup = false, Name = "Standards and Testing agency",
                DisplayNameMidSentenceCase = "standards and testing agency", SortOrder = 70,
            },
            new()
            {
                Id = Guid.NewGuid(), IsInternalGroup = false, Name = "Student Loans Company",
                DisplayNameMidSentenceCase = "student loans company", SortOrder = 80,
            },
            new()
            {
                Id = Guid.NewGuid(), IsInternalGroup = false, Name = "Teacher Regulation Agency",
                DisplayNameMidSentenceCase = "teacher regulation agency", SortOrder = 90,
            },
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
            _logger.LogWarning("GetPortfoliosAsync: {EntityCount} entities, {DomainModelCount} domain models",
                dbEntityModels.Count, domainModels.Count);
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
