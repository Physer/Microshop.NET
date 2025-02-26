using System.Security.Claims;

namespace Application.Authentication;

public record Token(IEnumerable<Claim> Claims);
