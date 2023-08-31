namespace Authentication.Models;

internal record struct AuthenticationResponse(AuthenticationStatus Status, User User);

internal record struct User(string Id, string Email, long Timejoined);
