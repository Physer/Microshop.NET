namespace Application.Authentication;

public interface IAuthenticationClient
{
    Task<AuthenticationData> SigIn(string username, string password);
}
