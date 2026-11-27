
using HusOverblik.Shared.DTOs;
using HusOverblik.Shared.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace HusOverblik.Web.Services;

public class WebAuthenticationService : IAuthenticationService
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly IJSRuntime _jsRuntime;

    public event Action? AuthenticationStateChanged;

    public WebAuthenticationService(
        AuthenticationStateProvider authenticationStateProvider,
        IJSRuntime jsRuntime)
    {
        _authenticationStateProvider = authenticationStateProvider;
        _jsRuntime = jsRuntime;
    }

    public async Task<bool> LoginAsync(LoginRequest request)
    {
        var success = await _jsRuntime.InvokeAsync<bool>(
            "husOverblikAuth.login",
            request.Email,
            request.Password);

        if (success)
        {
            AuthenticationStateChanged?.Invoke();
        }

        return success;
    }

    public async Task LogoutAsync()
    {
        var success = await _jsRuntime.InvokeAsync<bool>(
            "husOverblikAuth.logout");

        if (!success)
        {
            throw new InvalidOperationException("Website logout failed.");
        }

        AuthenticationStateChanged?.Invoke();
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var state = await _authenticationStateProvider
            .GetAuthenticationStateAsync();

        return state.User.Identity?.IsAuthenticated == true;
    }
}
