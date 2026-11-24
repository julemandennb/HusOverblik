using HusOverblik.Shared.Services;
using HusOverblik.Web.Components;
using HusOverblik.Web.Database;
using HusOverblik.Web.Model;
using HusOverblik.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HusOverblik
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Database
            var connectionString =
                builder.Configuration.GetConnectionString("HusOverblikDb")
                ?? throw new InvalidOperationException(
                    "HusOverblikDb connection string is missing.");

            builder.Services.AddDbContext<ApplicationDbContext>(
                options => options.UseNpgsql(connectionString));

            // ASP.NET Core Identity
            builder.Services
                .AddIdentityApiEndpoints<ApplicationUser>(options =>
                {
                    options.User.RequireUniqueEmail = true;

                    options.Password.RequiredLength = 8;
                    options.Password.RequireDigit = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddAuthorization();

            // Blazor
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

            builder.Services.AddSingleton<IFormFactor, FormFactor>();

            var app = builder.Build();

            // HTTP pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute(
                "/not-found",
                createScopeForStatusCodePages: true);

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseAntiforgery();

            // Identity API:
            // /api/auth/register
            // /api/auth/login
            // /api/auth/refresh
            // /api/auth/logout
            app.MapGroup("/api/auth")
            .MapIdentityApi<ApplicationUser>();

            // Get currently authenticated user
            app.MapGet("/api/users/me", (ClaimsPrincipal user) =>
            {
                return Results.Ok(new
                {
                    Id = user.FindFirstValue(
                        ClaimTypes.NameIdentifier),

                    Email = user.FindFirstValue(
                        ClaimTypes.Email),

                    UserName = user.Identity?.Name
                });
            })
            .RequireAuthorization();

            // Blazor endpoints
            app.MapStaticAssets();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(
                    typeof(HusOverblik.Shared._Imports).Assembly,
                    typeof(HusOverblik.Web.Client._Imports).Assembly);

            await SeedRolesAsync(app);

            var adminEmail = builder.Configuration["AdminProfil:Email"]
                ?? throw new InvalidOperationException("Admin email is missing.");

            var adminPassword = builder.Configuration["AdminProfil:Password"]
             ?? throw new InvalidOperationException("Admin password is missing.");
            await SeedAdminAsync(app, adminEmail, adminPassword);

            await app.RunAsync();
        }

        private static async Task SeedRolesAsync(WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var roleManager = scope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = ["Admin"];

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(
                        new IdentityRole(role));

                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException(
                            $"Failed to create role '{role}': " +
                            string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
            }
        }


        private static async Task SeedAdminAsync(
            WebApplication app,
            string adminEmail,
            string adminPassword)
        {
            using var scope = app.Services.CreateScope();

            var userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    FullName = "Admin",
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(
                    adminUser, adminPassword);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        "Failed to create administrator: " +
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                var roleResult = await userManager.AddToRoleAsync(
                    adminUser, "Admin");

                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        "Failed to assign Admin role: " +
                        string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}