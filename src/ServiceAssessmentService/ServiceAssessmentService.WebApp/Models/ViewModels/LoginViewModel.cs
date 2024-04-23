using System.ComponentModel.DataAnnotations;

namespace ServiceAssessmentService.WebApp.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "ID is required")]
        public string ID { get; set; }
    }
}
