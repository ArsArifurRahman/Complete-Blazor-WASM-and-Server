using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Server.Providers;
using Server.Services.Base;

namespace Server.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IClient _client;
    private readonly ILocalStorageService _service;
    private readonly AuthenticationStateProvider _provider;

    public AuthenticationService(IClient client, ILocalStorageService service, AuthenticationStateProvider provider)
    {
        _client = client;
        _service = service;
        _provider = provider;
    }

    public async Task<bool> AuthenticateAsync(UserLogin loginModel)
    {
        var response = await _client.LoginAsync(loginModel);
        await _service.SetItemAsync("accessToken", response.Token);
        await ((ApiAuthenticationStateProvider)_provider).LoggedIn();
        return true;
    }

    public async Task Logout()
    {
        await ((ApiAuthenticationStateProvider)_provider).LoggedOut();
    }
}
