using System.Security.Claims;

namespace Application.Authentication;

public record struct Token(IEnumerable<Claim> Claims);
