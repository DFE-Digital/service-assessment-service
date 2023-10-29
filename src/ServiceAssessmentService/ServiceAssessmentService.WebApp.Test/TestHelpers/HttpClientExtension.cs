using AngleSharp.Html.Dom;
using Xunit;

namespace ServiceAssessmentService.WebApp.Test.TestHelpers;


// Adapted from (MIT license):
// https://github.com/dotnet/aspnetcore/
//
public static class HttpClientExtensions
{
    public static Task<HttpResponseMessage> SendAsync(
        this HttpClient client,
        IHtmlFormElement form,
        IHtmlElement submitButton)
    {
        return client.SendAsync(form, submitButton, new Dictionary<string, string>());
    }

    public static Task<HttpResponseMessage> SendAsync(
        this HttpClient client,
        IHtmlFormElement form,
        IEnumerable<KeyValuePair<string, string>> formValues)
    {
        var submitElement = Assert.Single(form.QuerySelectorAll("[type=submit]"));
        var submitButton = Assert.IsAssignableFrom<IHtmlElement>(submitElement);

        return client.SendAsync(form, submitButton, formValues);
    }

    public static Task<HttpResponseMessage> SendAsync(
        this HttpClient client,
        IHtmlFormElement form,
        IHtmlElement submitButton,
        IEnumerable<KeyValuePair<string, string>> formValues)
    {
        foreach (var kvp in formValues)
        {
            var element = Assert.IsAssignableFrom<IHtmlInputElement>(form[kvp.Key]);
            element.Value = kvp.Value;
        }

        var submit = form.GetSubmission(submitButton);
        var target = (Uri)submit.Target;
        if (submitButton.HasAttribute("formaction"))
        {
            var formAction = submitButton.GetAttribute("formaction");
            target = new Uri(formAction, UriKind.Relative);
        }

        // {
        //     // for debugging -- output copy of post body...
        //     using Stream s1 = new MemoryStream();
        //     submit.Body.CopyTo(s1);
        //     
        //     // read all bytes from stream
        //     byte[] bytes = new byte[s1.Length];
        //     s1.Read(bytes, 0, (int)s1.Length);
        //     // convert byte array to string
        //     string body = Encoding.ASCII.GetString(bytes);
        //     // do something with body
        //     Console.WriteLine(body);
        //     body.Should().Contain("service-name=abc");
        // }

        var submission = new HttpRequestMessage(new HttpMethod(submit.Method.ToString()), target)
        {
            Content = new StreamContent(submit.Body),
        };

        foreach (var header in submit.Headers)
        {
            submission.Headers.TryAddWithoutValidation(header.Key, header.Value);
            submission.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        return client.SendAsync(submission);
    }
}
