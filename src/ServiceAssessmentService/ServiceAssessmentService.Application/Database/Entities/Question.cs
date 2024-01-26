using ServiceAssessmentService.Domain.Model.Questions;

namespace ServiceAssessmentService.Application.Database.Entities;

internal class Question
{
    public Guid Id { get; set; }
    
    public Guid? TemplatedFromId { get; set; }
    public Question? TemplatedFrom { get; set; }
    
    public Guid? AssessmentRequestId { get; set; }
    public virtual AssessmentRequest? AssessmentRequest { get; set; } = null!;
    
    public QuestionType Type { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    public string HintText { get; set; } = string.Empty;
    
}

internal class SimpleTextQuestion : Question
{
    public SimpleTextQuestion()
    {
        Type = QuestionType.SimpleText;
    }
    
    public string? Answer { get; set; }
    
    public Domain.Model.Questions.SimpleTextQuestion ToDomainModel(AssessmentRequest assessmentRequest)
    {
        return new Domain.Model.Questions.SimpleTextQuestion()
        {
            Id = Id,
            Type = Type,
            Title = Title,
            HintText = HintText,
            Answer = Answer,
            AssessmentRequest = assessmentRequest.ToDomainModel(),
        };
    }
}

internal class LongTextQuestion : Question
{
    public LongTextQuestion()
    {
        Type = QuestionType.LongText;
    }
    
    public string? Answer { get; set; }
}

internal class DateOnlyQuestion : Question
{
    public DateOnlyQuestion()
    {
        Type = QuestionType.DateOnly;
    }
    
    public DateOnly? Answer { get; set; }
}

internal class RadioQuestion : Question
{
    public RadioQuestion()
    {
        Type = QuestionType.Radio;
    }
    
    public string? Answer { get; set; }
}

internal class RadioOption
{
    public Guid Id { get; set; }
    public Guid QuestionId { get; set; }
    public Question Question { get; set; } = null!;
    
    public string DisplayTitle { get; set; } = string.Empty;
    
    public string Value { get; set; } = string.Empty;
    
}

