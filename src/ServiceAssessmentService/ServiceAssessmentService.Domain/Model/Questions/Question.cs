namespace ServiceAssessmentService.Domain.Model.Questions;

public abstract class Question
{
    public Guid Id { get; set; }
    public Guid TemplatedFromId { get; set; }
    public required AssessmentRequest AssessmentRequest { get; set; }
    
    public required QuestionType Type { get; set; }

    public string Title { get; set; } = string.Empty;
    public string HintText { get; set; } = string.Empty;

    public abstract string? AnswerDisplayText { get; }
}

public class SimpleTextQuestion : Question
{
    public SimpleTextQuestion()
    {
        Type = QuestionType.SimpleText;
    }
    
    public int MaxLengthChars { get; set; } = 250;
    public string? Answer { get; set; }
    public override string? AnswerDisplayText => Answer;
}


public class LongTextQuestion : Question
{
    public LongTextQuestion()
    {
        Type = QuestionType.LongText;
    }
    
    public int MaxLengthChars { get; set; } = 250;
    public string? Answer { get; set; }
    public override string? AnswerDisplayText => Answer;
}

public class DateOnlyQuestion : Question
{
    public DateOnlyQuestion()
    {
        Type = QuestionType.DateOnly;
    }
    
    public DateOnly? Answer { get; set; }
    public override string? AnswerDisplayText => Answer.ToString();
}


public class RadioQuestion : Question
{
    public RadioQuestion()
    {
        Type = QuestionType.Radio;
    }
    
    public IEnumerable<RadioOption> Options { get; set; } = Enumerable.Empty<RadioOption>();
    public RadioOption? Answer { get; set; }
    public override string? AnswerDisplayText => Answer?.DisplayTitle;

    public class RadioOption
    {
        public string DisplayTitle { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        
        public Question? NestedQuestion { get; set; } = null!;
    }
}
