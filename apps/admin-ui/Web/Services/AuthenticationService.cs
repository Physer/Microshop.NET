using Application.Authentication;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;

namespace Web.Services;

public class AuthenticationService
{
    private readonly ISessionStorageService _sessionStorageService;
    private readonly IAuthenticationClient _authenticationClient;
    private readonly NavigationManager _navigationManager;

    private const string _tokenStorageKey = "msJwt";

    public AuthenticationService(ISessionStorageService sessionStorageService,
        IAuthenticationClient authenticationClient,
        NavigationManager navigationManager)
    {
        _sessionStorageService = sessionStorageService;
        _authenticationClient = authenticationClient;
        _navigationManager = navigationManager;
    }

    public async Task<string> GetTokenAsync() => await _sessionStorageService.GetItemAsStringAsync(_tokenStorageKey);

    public async Task SignInAsync(string username, string password)
    {
        var signInResult = await _authenticationClient.SignInAsync(username, password);
        if (!signInResult.Success)
            throw new UnauthorizedAccessException();

        await _sessionStorageService.SetItemAsStringAsync(_tokenStorageKey, signInResult.Token);
        _navigationManager.NavigateTo("/");
    }
}
