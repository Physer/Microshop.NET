namespace Application.Authentication;

public interface ITokenHandler
{
    Token ReadJwt(string jwt);
}
