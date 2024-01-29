using ServiceAssessmentService.Domain.Model.Questions;

namespace ServiceAssessmentService.Domain.Model;

public class AssessmentRequest
{
    public Guid Id { get; set; }

    public RadioQuestion PhaseConcluding { get; init; }

    public RadioQuestion AssessmentType { get; init; }

    public RadioQuestion IsThisReassessment { get; init; }

    public SimpleTextQuestion Name { get; init; }

    public LongTextQuestion Description { get; init; }

    public RadioQuestion IsProjectCodeKnown { get; init; }

    public SimpleTextQuestion ProjectCode { get; init; }

    public DateOnlyQuestion PhaseStartDate { get; init; }

    public RadioQuestion IsPhaseEndDateKnown { get; init; }

    public DateOnlyQuestion PhaseEndDate { get; init; }


    public DateTimeOffset CreatedAt { get; init; }

    public DateTimeOffset UpdatedAt { get; init; }


    public required IEnumerable<Question> Questions { get; set; }

    public AssessmentRequest()
    {
        var phaseConcludingQuestion = new RadioQuestion()
        {
            Id = new Guid("00000000-0000-1111-0001-000000000000"),
            Title = "What phase is concluding?",
            HintText = string.Empty,
            Type = QuestionType.Radio,
            // Options = ProjectPhase.Sequence
            //     .Select(x => new RadioQuestion.RadioOption()
            //     {
            //         Id = new Guid("00000000-0000-1111-0000-000000000001"),
            //         DisplayTitle = x.Name,
            //         Value = x.Name.ToLowerInvariant(),
            //     })
            //     .ToList(),
            Options = new List<RadioQuestion.RadioOption>()
            {
                new RadioQuestion.RadioOption()
                {
                    Id = new Guid("00000000-0000-1111-0001-000000000001"),
                    DisplayTitle = ProjectPhase.Sequence[0].Name,
                },
                new RadioQuestion.RadioOption()
                {
                    Id = new Guid("00000000-0000-1111-0001-000000000002"),
                    DisplayTitle = ProjectPhase.Sequence[1].Name,
                },
                new RadioQuestion.RadioOption()
                {
                    Id = new Guid("00000000-0000-1111-0001-000000000003"),
                    DisplayTitle = ProjectPhase.Sequence[2].Name,
                },
                new RadioQuestion.RadioOption()
                {
                    Id = new Guid("00000000-0000-1111-0001-000000000004"),
                    DisplayTitle = ProjectPhase.Sequence[3].Name,
                },
                new RadioQuestion.RadioOption()
                {
                    Id = new Guid("00000000-0000-1111-0001-000000000005"),
                    DisplayTitle = ProjectPhase.Sequence[4].Name,
                },
                new RadioQuestion.RadioOption()
                {
                    Id = new Guid("00000000-0000-1111-0001-000000000006"),
                    DisplayTitle = ProjectPhase.Sequence[5].Name,
                },
            },
        };

        if (ProjectPhase.Sequence.Length != phaseConcludingQuestion.Options.Count())
        {
            throw new Exception("Error setting up test data -- ProjectPhase.Sequence.Length != phaseConcludingQuestion.Options.Count()");
        }

        var assessmentTypeQuestion = new RadioQuestion()
        {
            Id = new Guid("00000000-0000-1111-0002-000000000000"),
            Title = "What type of assessment are you requesting?",
            HintText = string.Empty,
            Type = QuestionType.Radio,
            // Options = ServiceAssessmentService.Domain.Model.AssessmentType.All
            //     .Select(x => new RadioQuestion.RadioOption()
            //     {
            //         DisplayTitle = x.Name,
            //         Value = x.Name.ToLowerInvariant(),
            //     })
            //     .ToList(),
            Options = new List<RadioQuestion.RadioOption>()
            {
                new RadioQuestion.RadioOption()
                {
                    Id = new Guid("00000000-0000-1111-0002-000000000001"),
                    DisplayTitle = ServiceAssessmentService.Domain.Model.AssessmentType.All[0].Name,
                },
                new RadioQuestion.RadioOption()
                {
                    Id = new Guid("00000000-0000-1111-0002-000000000002"),
                    DisplayTitle = ServiceAssessmentService.Domain.Model.AssessmentType.All[1].Name,
                },
                new RadioQuestion.RadioOption()
                {
                    Id = new Guid("00000000-0000-1111-0002-000000000003"),
                    DisplayTitle = ServiceAssessmentService.Domain.Model.AssessmentType.All[2].Name,
                },
                new RadioQuestion.RadioOption()
                {
                    Id = new Guid("00000000-0000-1111-0002-000000000004"),
                    DisplayTitle = ServiceAssessmentService.Domain.Model.AssessmentType.All[3].Name,
                },
                new RadioQuestion.RadioOption()
                {
                    Id = new Guid("00000000-0000-1111-0002-000000000005"),
                    DisplayTitle = ServiceAssessmentService.Domain.Model.AssessmentType.All[4].Name,
                },
            },
        };

        if (ServiceAssessmentService.Domain.Model.AssessmentType.All.Length != assessmentTypeQuestion.Options.Count())
        {
            throw new Exception("Error setting up test data -- ServiceAssessmentService.Domain.Model.AssessmentType.All.Length != assessmentTypeQuestion.Options.Count()");
        }

        var nameQuestion = new SimpleTextQuestion()
        {
            Id = new Guid("00000000-0000-1111-0003-000000000000"),
            Title = "What is the name of your service?",
            HintText = "This can be changed in the future.",
            MaxLengthChars = 100,
            Type = QuestionType.SimpleText,
        };

        var descriptionQuestion = new LongTextQuestion()
        {
            Id = new Guid("00000000-0000-1111-0004-000000000000"),
            Title = "What is the purpose of your discovery?",
            HintText = "Tell us the purpose of your discovery. For example, if it's part of a wider service or based on policy intent. This description will help us to arrange a review panel with the most relevant experience.",
            MaxLengthChars = 500,
            Type = QuestionType.LongText,
        };

        var isThisReassessmentQuestion = new RadioQuestion()
        {
            Id = new Guid("00000000-0000-1111-0005-000000000000"),
            Title = "Is this a reassessment?",
            HintText = "Select one option.",
            Type = QuestionType.Radio,
            Options = new List<RadioQuestion.RadioOption>()
            {
                new RadioQuestion.RadioOption()
                {
                    Id = new Guid("00000000-0000-1111-0005-000000000001"),
                    DisplayTitle = "Yes",
                },
                new RadioQuestion.RadioOption()
                {
                    Id = new Guid("00000000-0000-1111-0005-000000000002"),
                    DisplayTitle = "No",
                },
            },
        };

        var projectCodeQuestion = new SimpleTextQuestion()
        {
            Id = new Guid("00000000-0000-1111-0006-000000000000"),
            Title = "What is your project code?",
            HintText = "This can be changed in the future.",
            MaxLengthChars = 100,
            Type = QuestionType.SimpleText,
        };

        var isProjectCodeKnownQuestion = new RadioQuestion()
        {
            Id = new Guid("00000000-0000-1111-0007-000000000000"),
            Title = "Do you have a project code?",
            // TODO: Multi-line, and include URLs -- use markdown?
            HintText = "This code is sometimes called a project ID. It starts with DDaT.\n\n" +
                       "For example, DDaT_22/23_001.\n\n" +
                       "Find the code on the DDaT portfolio tracker (opens in new tab), or speak to your business partner (opens in new tab).",
            Type = QuestionType.Radio,
            Options = new List<RadioQuestion.RadioOption>()
            {
                new RadioQuestion.RadioOption()
                {
                    Id = new Guid("00000000-0000-1111-0007-000000000001"),
                    DisplayTitle = "Yes",
                },
                new RadioQuestion.RadioOption()
                {
                    Id = new Guid("00000000-0000-1111-0007-000000000002"),
                    DisplayTitle = "No",
                },
            },
        };

        var startDateQuestion = new DateOnlyQuestion()
        {
            Id = new Guid("00000000-0000-1111-0008-000000000000"),
            Title = "When did this phase start?",
            HintText = "For example, 18 2 2023.",
            Type = QuestionType.DateOnly,
        };

        var endDateQuestion = new DateOnlyQuestion()
        {
            Id = new Guid("00000000-0000-1111-0009-000000000000"),
            Title = "When is this phase ending?",
            HintText = "For example, 18 2 2023.",
            Type = QuestionType.DateOnly,
        };

        var endDateKnownQuestion = new RadioQuestion()
        {
            Id = new Guid("00000000-0000-1111-0010-000000000000"),
            Title = "Do you have an end date for your discovery?",
            HintText = "Select one option.",
            Type = QuestionType.Radio,
            Options = new List<RadioQuestion.RadioOption>()
            {
                new RadioQuestion.RadioOption()
                {
                    Id = new Guid("00000000-0000-1111-0010-000000000001"),
                    DisplayTitle = "Yes",
                    NestedQuestion = endDateQuestion,
                },
                new RadioQuestion.RadioOption()
                {
                    Id = new Guid("00000000-0000-1111-0010-000000000001"),
                    DisplayTitle = "No",
                },
            },
        };

        // Questions common to all assessment types, with specific need to reference them individually therefore referenced via domain property
        Id = new Guid("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA");
        PhaseConcluding = phaseConcludingQuestion;
        AssessmentType = assessmentTypeQuestion;
        IsThisReassessment = isThisReassessmentQuestion;
        Name = nameQuestion;
        Description = descriptionQuestion;
        PhaseStartDate = startDateQuestion;
        IsPhaseEndDateKnown = endDateKnownQuestion;
        PhaseEndDate = endDateQuestion;

        // Common questions, plus additional questions from database
        Questions = new List<Question>()
        {
            phaseConcludingQuestion,
            assessmentTypeQuestion,
            isThisReassessmentQuestion,
            nameQuestion,
            descriptionQuestion,
            startDateQuestion,
            endDateKnownQuestion,
            //endDateQuestion, // nested question
        };
    }
}
