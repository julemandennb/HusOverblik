using Microsoft.AspNetCore.Components;

namespace HusOverblik.Shared.Components
{
    public partial class AuthGuard
    {
        [Inject]
        Services.AuthenticationStateService AuthenticationState { get; set; } = default!;

        [Inject]
        Services.IAuthenticationService AuthService { get; set; } = default!;

        [Inject]
        NavigationManager Navigation { get; set; } = default!;


        [Parameter]
        public bool IsPublic { get; set; }

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        private bool isChecking = true;

        protected override void OnInitialized()
        {
            AuthenticationState.AuthenticationStateChanged += OnAuthenticationChanged;
        }

        protected override async Task OnParametersSetAsync()
        {
            if (IsPublic)
            {
                isChecking = false;
                return;
            }

            isChecking = true;

            var authenticated = await AuthService.IsAuthenticatedAsync();

            isChecking = false;

            if (!authenticated)
            {
                Navigation.NavigateTo("/login");
            }
        }

        private void OnAuthenticationChanged()
        {
            InvokeAsync(() =>
            {
                if (!IsPublic && !AuthenticationState.IsAuthenticated)
                {
                    Navigation.NavigateTo("/login");
                }

                StateHasChanged();
            });
        }

        public void Dispose()
        {
            AuthenticationState.AuthenticationStateChanged -= OnAuthenticationChanged;
        }
    }
}
