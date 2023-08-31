namespace Authentication.Models;

internal record struct AuthenticationResponse(string Status, User User);

internal record struct User(string Id, string Email, long Timejoined);
