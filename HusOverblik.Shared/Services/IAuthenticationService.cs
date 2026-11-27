using HusOverblik.Shared.DTOs;

namespace HusOverblik.Shared.Services;

public interface IAuthenticationService
{
    Task<bool> LoginAsync(LoginRequest request);

    Task LogoutAsync();

    Task<bool> IsAuthenticatedAsync();

    event Action? AuthenticationStateChanged;
}
