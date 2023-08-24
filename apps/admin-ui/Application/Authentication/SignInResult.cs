namespace Application.Authentication;

public record struct SignInResult(bool Success, string? Token);
