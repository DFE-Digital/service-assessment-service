using ServiceAssessmentService.Domain.Model.Questions;

namespace ServiceAssessmentService.Domain.Model;

public class AssessmentRequest
{
    public Guid Id { get; set; }

    public RadioQuestion PhaseConcluding { get; set; }

    public RadioQuestion AssessmentType { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateOnly? PhaseStartDate { get; set; }

    public DateOnly? PhaseEndDate { get; set; }


    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }


    public IEnumerable<Question> Questions { get; set; } = Enumerable.Empty<Question>();

    public AssessmentRequest()
    {
        var phaseConcludingQuestion = new RadioQuestion()
        {
            Title = "What phase is concluding?",
            HintText = string.Empty,
            AssessmentRequest = this,
            Type = QuestionType.Radio,
            Options = ProjectPhase.Sequence
                .Select(x => new RadioQuestion.RadioOption()
                {
                    DisplayTitle = x.Name,
                    Value = x.Name.ToLowerInvariant(),
                })
                .ToList(),
        };

        var assessmentTypeQuestion = new RadioQuestion()
        {
            Title = "What type of assessment are you requesting?",
            HintText = string.Empty,
            AssessmentRequest = this,
            Type = QuestionType.Radio,
            Options = ServiceAssessmentService.Domain.Model.AssessmentType.All
                .Select(x => new RadioQuestion.RadioOption()
                {
                    DisplayTitle = x.Name,
                    Value = x.Name.ToLowerInvariant(),
                })
                .ToList(),
        };

        var nameQuestion = new SimpleTextQuestion()
        {
            Title = "What is the name of your service?",
            HintText = "This can be changed in the future.",
            AssessmentRequest = this,
            MaxLengthChars = 100,
            Type = QuestionType.SimpleText,
        };

        var descriptionQuestion = new LongTextQuestion()
        {
            Title = "What is the purpose of your discovery?",
            HintText = "Tell us the purpose of your discovery. For example, if it's part of a wider service or based on policy intent. This description will help us to arrange a review panel with the most relevant experience.",
            AssessmentRequest = this,
            MaxLengthChars = 500,
            Type = QuestionType.LongText,
        };

        var isThisReassessmentQuestion = new RadioQuestion()
        {
            Title = "Is this a reassessment?",
            HintText = "Select one option.",
            AssessmentRequest = this,
            Type = QuestionType.Radio,
            Options = new List<RadioQuestion.RadioOption>()
            {
                new RadioQuestion.RadioOption()
                {
                    DisplayTitle = "Yes",
                    Value = "yes",
                },
                new RadioQuestion.RadioOption()
                {
                    DisplayTitle = "No",
                    Value = "no",
                },
            },
        };

        var startDateQuestion = new DateOnlyQuestion()
        {
            Title = "When did this phase start?",
            HintText = "For example, 18 2 2023.",
            AssessmentRequest = this,
            Type = QuestionType.DateOnly,
        };

        var endDateQuestion = new DateOnlyQuestion()
        {
            Title = "When is this phase ending?",
            HintText = "For example, 18 2 2023.",
            AssessmentRequest = this,
            Type = QuestionType.DateOnly,
        };

        var endDateKnownQuestion = new RadioQuestion()
        {
            Title = "Do you have an end date for your discovery?",
            HintText = "Select one option.",
            AssessmentRequest = this,
            Type = QuestionType.Radio,
            Options = new List<RadioQuestion.RadioOption>()
            {
                new RadioQuestion.RadioOption()
                {
                    DisplayTitle = "Yes",
                    Value = "yes",
                    NestedQuestion = endDateQuestion,
                },
                new RadioQuestion.RadioOption()
                {
                    DisplayTitle = "No",
                    Value = "no",
                },
            },
        };
        
        PhaseConcluding = phaseConcludingQuestion;
        AssessmentType = assessmentTypeQuestion;

        Questions = new List<Question>()
        {
            phaseConcludingQuestion,
            assessmentTypeQuestion,
            isThisReassessmentQuestion,
            nameQuestion,
            descriptionQuestion,
            startDateQuestion,
            endDateKnownQuestion,
            //endDateQuestion,
        };
    }
}
