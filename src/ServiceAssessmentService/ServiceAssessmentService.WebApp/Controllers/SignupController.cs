using Microsoft.AspNetCore.Mvc;
using ServiceAssessmentService.WebApp.Interfaces;
using System.Threading.Tasks;

namespace ServiceAssessmentService.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SignupController : ControllerBase
    {
        private readonly IUserService _userService;

        public SignupController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Signup([FromBody] SignupRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Call the UserService to register the user
            string magicLink = await _userService.RegisterUserAsync(request.Email, request.Name);

            // Return the magic link to the client
            return Ok(new { MagicLink = magicLink });
        }
    }

    public class SignupRequest
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
