using ServiceAssessmentService.Domain.Model.Questions;

namespace ServiceAssessmentService.Application.Database.Entities;

internal abstract class Question : BaseEntity
{
    public Guid Id { get; set; }

    public Guid? TemplatedFromId { get; set; }
    public Question? TemplatedFrom { get; set; }

    public Guid? AssessmentRequestId { get; set; }
    public virtual AssessmentRequest? AssessmentRequest { get; set; } = null!;

    public required QuestionType Type { get; set; }

    public string Title { get; set; } = string.Empty;

    public string HintText { get; set; } = string.Empty;

    protected Question(QuestionType type)
    {
        Type = type;
    }

    public abstract Domain.Model.Questions.Question? ToDomainModel();
}

internal class SimpleTextQuestion : Question
{
    public SimpleTextQuestion() : base(QuestionType.SimpleText)
    {
    }

    public string? SimpleTextAnswer { get; set; }

    public override Domain.Model.Questions.Question? ToDomainModel()
    {
        return new Domain.Model.Questions.SimpleTextQuestion()
        {
            Id = Id,
            Type = Type,
            Title = Title,
            HintText = HintText,
            Answer = SimpleTextAnswer,
        };
    }
}

internal class LongTextQuestion : Question
{
    public LongTextQuestion() : base(QuestionType.LongText)
    {
    }

    public string? LongTextAnswer { get; set; }

    public override Domain.Model.Questions.Question? ToDomainModel()
    {
        return new Domain.Model.Questions.LongTextQuestion()
        {
            Id = Id,
            Type = Type,
            Title = Title,
            HintText = HintText,
            Answer = LongTextAnswer,
        };
    }
}

internal class DateOnlyQuestion : Question
{
    public DateOnlyQuestion() : base(QuestionType.DateOnly)
    {
    }

    public DateOnly? DateOnlyAnswer { get; set; }

    public override Domain.Model.Questions.Question? ToDomainModel()
    {
        return new Domain.Model.Questions.DateOnlyQuestion()
        {
            Id = Id,
            Type = Type,
            Title = Title,
            HintText = HintText,
            Answer = DateOnlyAnswer,
        };
    }
}

internal class RadioQuestion : Question
{
    public RadioQuestion() : base(QuestionType.Radio)
    {
    }

    public Guid? SelectedOptionId { get; set; }
    public virtual RadioOption? SelectedOption { get; set; } = null!;

    public virtual IEnumerable<RadioOption> Options { get; set; } = new List<RadioOption>();

    public override Domain.Model.Questions.Question? ToDomainModel()
    {
        return new Domain.Model.Questions.RadioQuestion()
        {
            Id = Id,
            Type = Type,
            Title = Title,
            HintText = HintText,
            SelectedOption = SelectedOption?.ToDomainModel(),
            Options = Options.Select(e => e.ToDomainModel()).ToList(),
        };
    }

}

internal class RadioOption
{
    public Guid Id { get; set; }

    public Guid QuestionId { get; set; }
    public virtual Question Question { get; set; } = null!;

    public string DisplayTitle { get; set; } = string.Empty;

    public Guid? NestedQuestionId { get; set; }
    public virtual Question? NestedQuestion { get; set; } = null!; 

    public Domain.Model.Questions.RadioQuestion.RadioOption ToDomainModel()
    {
        return new Domain.Model.Questions.RadioQuestion.RadioOption()
        {
            Id = Id,
            DisplayTitle = DisplayTitle,
            NestedQuestion = NestedQuestion?.ToDomainModel(),
        };
    }

}

