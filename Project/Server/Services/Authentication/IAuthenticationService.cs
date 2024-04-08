using Server.Services.Base;

namespace Server.Services.Authentication;

public interface IAuthenticationService
{
    Task<bool> AuthenticateAsync(UserLogin loginModel);
    public Task Logout();
}
