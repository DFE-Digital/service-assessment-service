using System;
namespace ServiceAssessmentService.WebApp.Models
{
    public class User
    {
        public string Email { get; set; }
        public string PersonalName { get; set; }

        public string FamilyName { get; set; }

        public Guid ID { get; set; }
        // Add other properties as needed
    }
}
