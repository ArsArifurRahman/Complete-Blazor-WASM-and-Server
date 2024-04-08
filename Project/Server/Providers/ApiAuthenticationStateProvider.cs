using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Server.Providers;

public class ApiAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _service;
    private readonly JwtSecurityTokenHandler _handler;

    public ApiAuthenticationStateProvider(ILocalStorageService service)
    {
        _service = service;
        _handler = new JwtSecurityTokenHandler();
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        var savedToken = await _service.GetItemAsync<string>("accessToken");

        if (savedToken == null)
        {
            return new AuthenticationState(user);
        }

        var tokenContent = _handler.ReadJwtToken(savedToken);

        if (tokenContent.ValidTo < DateTime.Now)
        {
            return new AuthenticationState(user);
        }

        var claims = await GetClaims();
        user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
        return new AuthenticationState(user);
    }

    public async Task LoggedIn()
    {
        var claims = await GetClaims();
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
        var authState = Task.FromResult(new AuthenticationState(user));
        NotifyAuthenticationStateChanged(authState);
    }

    public async Task LoggedOut()
    {
        await _service.RemoveItemAsync("accessToken");
        var nobody = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(nobody));
        NotifyAuthenticationStateChanged(authState);
    }

    private async Task<List<Claim>> GetClaims()
    {
        var savedToken = await _service.GetItemAsync<string>("accessToken");
        var tokenContent = _handler.ReadJwtToken(savedToken);
        var claims = tokenContent.Claims.ToList();
        claims.Add(new Claim(ClaimTypes.Name, tokenContent.Subject));
        return claims;
    }
}
