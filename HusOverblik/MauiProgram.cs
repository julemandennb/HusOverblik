using HusOverblik.Services;
using HusOverblik.Shared.Services;
using Microsoft.Extensions.Logging;

namespace HusOverblik
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            // Add device-specific services used by the HusOverblik.Shared project
            builder.Services.AddSingleton<IFormFactor, FormFactor>();

            builder.Services.AddMauiBlazorWebView();

            SetBaseAddress(builder);

            return builder.Build();
        }
        
        private static void SetBaseAddress(MauiAppBuilder builder)
        {
#if ANDROID
            var apiBaseAddress = "https://10.0.2.2:7031/";
#else
            var apiBaseAddress = "https://localhost:7031/";
#endif

            builder.Services.AddSingleton(new HttpClient
            {
                BaseAddress = new Uri(apiBaseAddress)
            });

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif


        }
    }
}
