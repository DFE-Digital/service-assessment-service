using Notify.Client;
using Notify.Models.Responses;
using Microsoft.Extensions.Configuration;
using System;
using ServiceAssessmentService.WebApp.Interfaces;

public class EmailService : IEmailService
{
    private readonly NotificationClient _client;

    public EmailService(IConfiguration configuration)
    {
        string apiKey = configuration["GovUKNotify:ApiKey"];
        _client = new NotificationClient(apiKey);
    }

    public async Task SendEmailAsync(string emailAddress, string templateId, Dictionary<string, dynamic> personalisation, string reference = null, string emailReplyToId = null)
    {
        // Send the email notification
        EmailNotificationResponse response = _client.SendEmail(emailAddress, templateId, personalisation, reference, emailReplyToId);

        // You can handle the response as needed
        if (response != null)
        {
            // Email sent successfully
            Console.WriteLine("Email sent successfully.");
        }
        else
        {
            // Handle error or failure
            Console.WriteLine("Failed to send email.");
        }
    }
}
