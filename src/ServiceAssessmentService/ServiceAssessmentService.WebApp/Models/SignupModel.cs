using System.ComponentModel.DataAnnotations;

namespace ServiceAssessmentService.WebApp.Models
{
    public class SignUpModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
