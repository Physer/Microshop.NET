namespace Application.Authentication;

public record struct AuthenticationData(string EmailAddress, IEnumerable<string> Roles, string AccessToken);
