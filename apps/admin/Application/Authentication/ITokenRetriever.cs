namespace Application.Authentication
{
    public interface ITokenRetriever
    {
        string GetAccessTokenFromCookie();
    }
}
