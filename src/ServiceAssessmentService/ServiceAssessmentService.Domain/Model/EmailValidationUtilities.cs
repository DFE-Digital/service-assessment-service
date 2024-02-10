using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ServiceAssessmentService.Domain.Model;

/// <summary>
/// Email address format validation is tricky.
/// This class provides a few different methods for validating email addresses.
/// 
/// Later we will do more robust testing to determine which method is best for our use case.
/// 
/// Note that the range of "technically valid" email addresses is very wide, and this class
/// is not intended to cover all possible cases -- our expected use case is limited to
/// DfE email addresses. This means technically valid but unacceptable email addresses such as
/// IPv6 addresses and hostnames with emoji or domains without a `.` character are normally rejected.
/// </summary>
public static class EmailValidationUtilities
{
    public static readonly IReadOnlyList<string> ValidDomains = new List<string>
    {
        "@education.gov.uk",
        "@digital.education.gov.uk",
        "@example.com",
        "@example.org",
    };

    public static bool IsValidDomain(string email)
    {
        return ValidDomains.Any(email.EndsWith);
    }

    /// <summary>
    /// Email validation is tricky.
    /// This is a simple implementation that checks for the presence of an '@' character.
    /// Additionally, we check that the tail end of the string is a domain on our allow list.
    /// This is overly-permissive, and can be tightened up as needed.
    ///
    /// In the future we might allow other domains, but only where the user has previously been invited
    /// to login therefore we have a record of the individual for example.
    /// </summary>
    public static bool IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        // Extremely permissive basic check, just that it contains an '@' character
        if (!email.Contains('@'))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Validates whether the given email address is in a valid format.
    /// Uses the C# EmailAddressAttribute to implement the validation logic.
    /// </summary>
    public static bool IsValidEmailUsingAttribute(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        var emailValidation = new EmailAddressAttribute();
        return emailValidation.IsValid(email);
    }

    /// <summary>
    /// Validates whether the given email address is in a valid format.
    /// 
    /// Adapted from:
    /// https://learn.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
    /// </summary>
    public static bool IsValidEmailFormat(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        try
        {
            // Normalize the domain
            email = Regex.Replace(
                email,
                @"(@)(.+)$",
                DomainMapper,
                RegexOptions.None,
                TimeSpan.FromMilliseconds(200)
            );

            // Examines the domain part of the email and normalizes it.
            string DomainMapper(Match match)
            {
                // Use IdnMapping class to convert Unicode domain names.
                var idn = new IdnMapping();

                // Pull out and process domain name (throws ArgumentException on invalid)
                string domainName = idn.GetAscii(match.Groups[2].Value);

                return match.Groups[1].Value + domainName;
            }
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
        catch (ArgumentException)
        {
            return false;
        }

        try
        {
            return Regex.IsMatch(
                email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase,
                TimeSpan.FromMilliseconds(250)
            );
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
}
