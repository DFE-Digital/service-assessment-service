using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceAssessmentService.Application.Database;
using ServiceAssessmentService.Domain.Model.Questions;

namespace ServiceAssessmentService.Application.UseCases;

public class QuestionRepository
{
    
    private readonly DataContext _dbContext;
    private readonly ILogger<QuestionRepository> _logger;

    public QuestionRepository(DataContext dbContext, ILogger<QuestionRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<IEnumerable<Domain.Model.Questions.Question>> GetAllQuestions()
    {
        var allQuestions = await _dbContext.Questions
            .Select(e => e.ToDomainModel())
            .ToListAsync();

        return allQuestions;
    }
    
    public async Task<Domain.Model.Questions.Question?> GetByIdAsync(Guid id)
    {
        var question1 = await _dbContext.Questions
            .Where(e => e.Id == id)
            .SingleOrDefaultAsync();

        var question = question1?.ToDomainModel();

        return question;
    }
    
    public async Task<Domain.Model.Questions.Question?> CreateAsync(Domain.Model.Questions.SimpleTextQuestion question)
    {
        var entity = new Database.Entities.SimpleTextQuestion()
        {
            Id = question.Id,
            Type = QuestionType.SimpleText,
        };

        _dbContext.Questions.Add(entity);
        await _dbContext.SaveChangesAsync();

        return question;
    }
}
