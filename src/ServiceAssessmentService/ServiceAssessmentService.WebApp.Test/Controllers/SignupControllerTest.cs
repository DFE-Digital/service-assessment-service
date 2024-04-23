using ServiceAssessmentService.WebApp.Controllers;
using ServiceAssessmentService.WebApp.Model;
using Xunit;
using Moq;
using System.Threading.Tasks;
using ServiceAssessmentService.WebApp.Interfaces;

namespace ServiceAssessmentService.WebApp.Test
{
    public class SignupControllerTest
    {
        [Fact]
        public async Task Signup_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(repo => repo.RegisterUserAsync(It.IsAny<UserModel>()))
                           .ReturnsAsync(true);
            var controller = new SignupController(userServiceMock.Object);
            var userModel = new UserModel { Email = "test@example.com", Name = "Test User" };

            // Act
            var result = await controller.Signup(userModel);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }
    }

    public class UserServiceTests
    {
        [Fact]
        public async Task RegisterUserAsync_ValidInput_ReturnsTrue()
        {
            // Arrange
            var magicLinkServiceMock = new Mock<IMagicLinkService>();
            var userService = new UserService(magicLinkServiceMock.Object);
            var userModel = new UserModel { Email = "test@example.com", Name = "Test User" };

            // Act
            var result = await userService.RegisterUserAsync(userModel);

            // Assert
            Assert.True(result);
        }
    }
}
