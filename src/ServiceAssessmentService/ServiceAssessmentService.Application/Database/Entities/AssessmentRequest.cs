using ServiceAssessmentService.Domain.Model;
using ServiceAssessmentService.Domain.Model.Questions;

namespace ServiceAssessmentService.Application.Database.Entities;

internal class AssessmentRequest : BaseEntity
{
    public Guid Id { get; set; }

    public virtual IEnumerable<Database.Entities.Question> Questions { get; set; } = new List<Database.Entities.Question>();


    public ServiceAssessmentService.Domain.Model.AssessmentRequest ToDomainModel()
    {
        var assessmentRequest = new ServiceAssessmentService.Domain.Model.AssessmentRequest
        {
            Id = Id,
            Questions = new List<Domain.Model.Questions.Question>(),
            CreatedAt = CreatedUtc,
            UpdatedAt = UpdatedUtc,
        };


        var domainQuestions = Questions.Select(e => e.ToDomainModel());
        assessmentRequest.Questions = domainQuestions
            .Where(e => e is not null)
            .Select(e => e!)
            .ToList();

        return assessmentRequest;
    }
}
