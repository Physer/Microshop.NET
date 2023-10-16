namespace Application.Authentication;

public interface ITokenParser
{
    IEnumerable<string> GetRoles(string accessToken);
}
