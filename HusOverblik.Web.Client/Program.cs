using HusOverblik.Shared.Services;
using HusOverblik.Web.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace HusOverblik.Web.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            // Add device-specific services used by the HusOverblik.Shared project
            builder.Services.AddSingleton<IFormFactor, FormFactor>();

            builder.Services.AddScoped(sp =>
               new HttpClient
               {
                   BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
               });


            AddScoped(builder);

            await builder.Build().RunAsync();
        }

        private static void AddScoped(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddScoped<ApiService>();
        }
    }
}
