namespace Application.Authentication;

public interface IAuthenticationClient
{
    Task<SignInResult> SignInAsync(string username, string password);
}
