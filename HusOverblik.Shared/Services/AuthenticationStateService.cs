namespace HusOverblik.Shared.Services;

public class AuthenticationStateService
{
    private bool _isAuthenticated;

    public event Action? AuthenticationStateChanged;

    public bool IsAuthenticated => _isAuthenticated;

    public void SetAuthenticationState(bool isAuthenticated)
    {
        _isAuthenticated = isAuthenticated;
        AuthenticationStateChanged?.Invoke();
    }
}
