using ServiceAssessmentService.WebApp.Interfaces;
using AutoMapper;
using ServiceAssessmentService.WebApp.Models;
using System.Threading.Tasks;
using Notify.Interfaces;
using ServiceAssessmentService.Application.Database;
using ServiceAssessmentService.Domain.Model;
using ServiceAssessmentService.Application.Database.Entities;

namespace ServiceAssessmentService.WebApp.Services
{
    public class CreateUserService : ICreateUserService
    {
        private readonly IMagicLinkService _magicLinkService;
        private readonly IEmailService _emailService;
        private readonly DataContext _dbContext;
        private readonly IMapper _mapper;

        public CreateUserService(IMagicLinkService magicLinkService, IEmailService notificationService, DataContext dbContext)
        {
            _magicLinkService = magicLinkService;
            _emailService = notificationService;
            _dbContext = dbContext;
        }

        public async Task<string> RegisterUserAsync(SignUpModel person)
        {
            try
            {
                // Map UserModel to Entity
                var personModel = new PersonModel { ID = Guid.NewGuid(), Email = person.Email, PersonalName = person.FirstName, FamilyName = person.LastName };
                var personEntity = _mapper.Map<Person>(personModel);
                // Add the user to the database
                _dbContext.People.Add(personEntity);
                await _dbContext.SaveChangesAsync();

                // Generate a magic link for the user
                string magicLink = await _magicLinkService.GenerateMagicLinkAsync(personEntity.Id.ToString());

                // Return the magic link
                return magicLink;
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Error registering user: {ex.Message}");

                // Return null or throw an exception to indicate that an error occurred during user registration
                throw;
            }
        }

        public Task<bool> VerifyMagicLinkAsync(User person, string magicLink)
        {
            return _magicLinkService.VerifyMagicLinkAsync(magicLink);
        }

        public async Task<bool> SendEmailAsync(string email, string magicLink)
        {
            try
            {

                // Prepare personalisation data
                var personalisation = new Dictionary<string, dynamic>
                {
                    { "magicLink", magicLink }
                };

                // Fire and forget: Send email
                _ = _emailService.SendEmailAsync(email, "7ef7dbfe-6c94-4f13-8295-27b3e9ab4bba", personalisation);

                // Return true to indicate that the email sending process has started
                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Error sending email: {ex.Message}");

                // Return false to indicate that an error occurred during the email sending process
                return false;
            }
        }
    }
}
