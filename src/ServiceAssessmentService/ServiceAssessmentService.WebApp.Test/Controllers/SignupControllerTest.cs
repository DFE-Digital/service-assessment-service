using Microsoft.AspNetCore.Mvc;
using Moq;
using ServiceAssessmentService.WebApp.Controllers;
using ServiceAssessmentService.WebApp.Interfaces;
using ServiceAssessmentService.WebApp.Models;
using System.Threading.Tasks;
using Xunit;

namespace ServiceAssessmentService.WebApp.Test.Controllers
{
    public class SignupControllerTest
    {
        [Fact]
        public async Task Signup_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(repo => repo.RegisterUserAsync(It.IsAny<UserModel>()))
                           .ReturnsAsync("testMagicLink");
            var controller = new SignupController(userServiceMock.Object);
            var signupRequest = new SignupRequest { Email = "test@example.com", Name = "Test User" };

            // Act
            var result = await controller.Signup(signupRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var okResultValue = okResult.Value;
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("testMagicLink", okResultValue.GetType().GetProperty("MagicLink").GetValue(okResultValue));
        }

        // Add more test methods for other scenarios like invalid input, error handling, etc.
    }
}
