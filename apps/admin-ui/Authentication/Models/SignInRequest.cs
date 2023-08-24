namespace Authentication.Models;

internal class SignInRequest
{
    public SignInRequest(string username, string password)
    {
        FormFields = new List<FormField>
        {
            new FormField
            {
                Id = "email",
                Value = username,
            },
            new FormField
            {
                Id = "password",
                Value = password
            }
        };
    }

    public IEnumerable<FormField> FormFields { get; init; }

    internal class FormField
    {
        public required string Id { get; init; }
        public required string Value { get; init; }
    }
}