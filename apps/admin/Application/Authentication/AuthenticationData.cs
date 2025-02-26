namespace Application.Authentication;

public record AuthenticationData(string EmailAddress, IEnumerable<string> Roles, string AccessToken);
