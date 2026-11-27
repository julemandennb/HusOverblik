
using Microsoft.AspNetCore.Components;

namespace HusOverblik.Shared.Layout
{
    public partial class NavMenu : IDisposable
    {
        [Inject]
        private Services.IAuthenticationService AuthService { get; set; } = default!;

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        private bool isAuthenticated;

        protected override async Task OnInitializedAsync()
        {
            AuthService.AuthenticationStateChanged += OnAuthenticationChanged;

            isAuthenticated = await AuthService.IsAuthenticatedAsync();
        }

        private void OnAuthenticationChanged()
        {
            _ = InvokeAsync(async () =>
            {
                isAuthenticated = await AuthService.IsAuthenticatedAsync();
                StateHasChanged();
            });
        }

        private async Task HandleLogout()
        {
            await AuthService.LogoutAsync();
            Navigation.NavigateTo("/login", forceLoad: true);
        }

        public void Dispose()
        {
            AuthService.AuthenticationStateChanged -= OnAuthenticationChanged;
        }
    }
}
