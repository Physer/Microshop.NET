﻿using AngleSharp.Html.Dom;
using Xunit;

namespace IntegrationTests.Utilities;

/// <summary>
/// HttpClient Extensions to leverage AngleSharp for Razor Pages usage.
/// See: https://github.com/dotnet/AspNetCore.Docs.Samples/blob/main/test/integration-tests/IntegrationTestsSample/tests/RazorPagesProject.Tests/Helpers/HttpClientExtensions.cs
/// </summary>
internal static class HttpClientExtensions
{
    public static Task<HttpResponseMessage> SendAsync(
        this HttpClient client,
        IHtmlFormElement? form,
        IHtmlElement? submitButton)
    {
        if (form is null) throw new ArgumentNullException(nameof(form));
        if (submitButton is null) throw new ArgumentNullException(nameof(submitButton));

        return client.SendAsync(form, submitButton, new Dictionary<string, string>());
    }

    public static Task<HttpResponseMessage> SendAsync(
        this HttpClient client,
        IHtmlFormElement? form,
        IEnumerable<KeyValuePair<string, string>> formValues)
    {
        if (form is null) throw new ArgumentNullException(nameof(form));

        var submitElement = Assert.Single(form.QuerySelectorAll("[type=submit]"));
        var submitButton = Assert.IsAssignableFrom<IHtmlElement>(submitElement);

        return client.SendAsync(form, submitButton, formValues);
    }

    public static Task<HttpResponseMessage> SendAsync(
        this HttpClient client,
        IHtmlFormElement? form,
        IHtmlElement? submitButton,
        IEnumerable<KeyValuePair<string, string>> formValues)
    {
        if (form is null) throw new ArgumentNullException(nameof(form));
        if (submitButton is null) throw new ArgumentNullException(nameof(submitButton));

        foreach (var kvp in formValues)
        {
            var element = Assert.IsAssignableFrom<IHtmlInputElement>(form[kvp.Key]);
            element.Value = kvp.Value;
        }

        var submit = (form?.GetSubmission()) ?? throw new Exception("Unable to retrieve the form's submission element");
        var target = (Uri)submit.Target;
        if (submitButton.HasAttribute("formaction"))
        {
            var formaction = submitButton.GetAttribute("formaction") ?? throw new Exception("Unable to retrieve the formaction");
            target = new Uri(formaction, UriKind.Relative);
        }
        var submission = new HttpRequestMessage(new HttpMethod(submit.Method.ToString()), target)
        {
            Content = new StreamContent(submit.Body)
        };

        foreach (var header in submit.Headers)
        {
            submission.Headers.TryAddWithoutValidation(header.Key, header.Value);
            submission.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        return client.SendAsync(submission);
    }
}