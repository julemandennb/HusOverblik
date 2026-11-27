
using Microsoft.AspNetCore.Components;

namespace HusOverblik.Shared.Layout
{
    public partial class MainLayout : IDisposable
    {
        [Inject]
        private Services.IAuthenticationService AuthService { get; set; } = default!;

        private bool isCheckingAuthentication = true;
        private bool isAuthenticated;

        protected override async Task OnInitializedAsync()
        {
            AuthService.AuthenticationStateChanged += OnAuthenticationChanged;

            isAuthenticated = await AuthService.IsAuthenticatedAsync();
            isCheckingAuthentication = false;
        }

        private void OnAuthenticationChanged()
        {
            _ = InvokeAsync(async () =>
            {
                isAuthenticated = await AuthService.IsAuthenticatedAsync();
                StateHasChanged();
            });
        }

        public void Dispose()
        {
            AuthService.AuthenticationStateChanged -= OnAuthenticationChanged;
        }
    }
}
