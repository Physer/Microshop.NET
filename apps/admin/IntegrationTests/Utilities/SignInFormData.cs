using AngleSharp.Html.Dom;

namespace IntegrationTests.Utilities;

internal record struct SignInFormData(IHtmlFormElement FormElement, IHtmlInputElement SubmitButtonElement, IEnumerable<KeyValuePair<string, string?>> FormData);