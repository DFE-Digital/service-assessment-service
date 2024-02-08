using ServiceAssessmentService.Domain.Model;


namespace ServiceAssessmentService.Domain.Test;

public class AssessmentRequestPortfolioValidationTests
{
    private readonly List<Portfolio> _arbitraryTestPortfolios = new()
    {
        new Portfolio
        {
            Id = new Guid("00000000-1111-1111-1111-000000000000"),
            Name = "Test Portfolio",
            DisplayNameMidSentenceCase = "test portfolio 1",
            SortOrder = 1,
            IsInternalGroup = true,
        },
        new Portfolio
        {
            Id = new Guid("00000000-2222-2222-2222-000000000000"),
            Name = "Test Portfolio 2",
            DisplayNameMidSentenceCase = "test portfolio 2",
            SortOrder = 2,
            IsInternalGroup = true,
        },
        new Portfolio
        {
            Id = new Guid("00000000-3333-3333-3333-000000000000"),
            Name = "Test Portfolio 3",
            DisplayNameMidSentenceCase = "test portfolio 3",
            SortOrder = 3,
            IsInternalGroup = false,
        }
    };


    [Fact]
    public void When_Portfolio_IsNull_Then_Result_IsInvalid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            Portfolio = null,
        };

        // Act
        var result = assessmentRequest.ValidatePortfolio(_arbitraryTestPortfolios);

        // Assert
        Assert.False(result.IsValid);
        Assert.Empty(result.ValidationWarnings);
        Assert.Contains(result.ValidationErrors, v => v.FieldName == nameof(assessmentRequest.Portfolio));
    }

    [Fact]
    public void When_Portfolio_IsNotNull_Then_Result_IsValid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            Portfolio = _arbitraryTestPortfolios.FirstOrDefault() ?? throw new Exception(),
        };

        // Act
        var result = assessmentRequest.ValidatePortfolio(_arbitraryTestPortfolios);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.ValidationWarnings);
        Assert.Empty(result.ValidationErrors);
    }

    [Fact]
    public void When_Portfolio_IsNotInPortfolios_Then_Result_IsInvalid()
    {
        // Arrange
        var assessmentRequest = new AssessmentRequest
        {
            Portfolio = new Portfolio
            {
                Id = new Guid("00000000-9999-9999-9999-000000000000"),
                Name = "Test Portfolio 3",
                DisplayNameMidSentenceCase = "test portfolio 9",
                SortOrder = 9,
                IsInternalGroup = false,
            },
        };

        // Act
        var result = assessmentRequest.ValidatePortfolio(_arbitraryTestPortfolios);

        // Assert
        Assert.False(result.IsValid);
        Assert.Empty(result.ValidationWarnings);
        Assert.Contains(result.ValidationErrors, v => v.FieldName == nameof(assessmentRequest.Portfolio));
    }
}
