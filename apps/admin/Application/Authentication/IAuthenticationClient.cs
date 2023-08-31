namespace Application.Authentication;

public interface IAuthenticationClient
{
    Task<AuthenticationData> SignInAsync(string username, string password);
}
