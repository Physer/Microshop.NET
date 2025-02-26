namespace Authentication.Models;

internal record AuthenticationResponse(string Status, User User);

internal record User(string Id, string Email, long Timejoined);
