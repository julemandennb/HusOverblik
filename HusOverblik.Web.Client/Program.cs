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

            await builder.Build().RunAsync();
        }
    }
}
