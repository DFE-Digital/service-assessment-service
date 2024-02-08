using ServiceAssessmentService.Domain.Model;

namespace ServiceAssessmentService.Domain.Test;

public class AssessmentRequestPortfolioCompletenessTests
{
    private static Portfolio ArbitraryValidPortfolio => new();

    [Fact]
    public void When_Portfolio_IsNotNull_Then_IsValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            Portfolio = ArbitraryValidPortfolio,
        };

        // Act
        var result = assessmentRequest.IsPortfolioComplete();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void When_Portfolio_IsNull_Then_IsNotValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            Portfolio = null,
        };

        // Act
        var result = assessmentRequest.IsPortfolioComplete();

        // Assert
        Assert.False(result);

    }
}
