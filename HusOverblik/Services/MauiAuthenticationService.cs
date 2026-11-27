
using System.Net.Http.Json;
using HusOverblik.Shared.DTOs;
using HusOverblik.Shared.Services;

namespace HusOverblik.Services;

public class MauiAuthenticationService : IAuthenticationService
{
    public event Action? AuthenticationStateChanged;
    private const string AccessTokenKey = "access_token";
    private const string RefreshTokenKey = "refresh_token";
    private const string ExpiryKey = "token_expiry";

    private readonly AuthenticationStateService _authenticationState;

    private readonly HttpClient _httpClient;

    public MauiAuthenticationService(HttpClient httpClient, AuthenticationStateService authenticationState)
    {
        _httpClient = httpClient;
        _authenticationState = authenticationState;

    }

    public async Task<bool> LoginAsync(LoginRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "/api/auth/login?useCookies=false",
            request);

        if (!response.IsSuccessStatusCode)
            return false;

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

        if (result == null || string.IsNullOrWhiteSpace(result.AccessToken))
            return false;

        await SecureStorage.Default.SetAsync(
            AccessTokenKey, result.AccessToken);

        await SecureStorage.Default.SetAsync(
            RefreshTokenKey, result.RefreshToken);

        var expiry = DateTimeOffset.UtcNow.AddSeconds(result.ExpiresIn);

        await SecureStorage.Default.SetAsync(
            ExpiryKey, expiry.ToString("O"));

        _authenticationState.SetAuthenticationState(true);
        AuthenticationStateChanged?.Invoke();

        return true;
    }

    public async Task LogoutAsync()
    {
        SecureStorage.Default.Remove(AccessTokenKey);
        SecureStorage.Default.Remove(RefreshTokenKey);
        SecureStorage.Default.Remove(ExpiryKey);

        _authenticationState.SetAuthenticationState(false);
    }


    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await SecureStorage.Default.GetAsync(AccessTokenKey);
        var expiryString = await SecureStorage.Default.GetAsync(ExpiryKey);

        bool isAuthenticated =
            !string.IsNullOrWhiteSpace(token) &&
            DateTimeOffset.TryParse(expiryString, out var expiry) &&
            expiry > DateTimeOffset.UtcNow;

        if (_authenticationState.IsAuthenticated != isAuthenticated)
        {
            _authenticationState.SetAuthenticationState(isAuthenticated);
            AuthenticationStateChanged?.Invoke();
        }

        return isAuthenticated;
    }

}
