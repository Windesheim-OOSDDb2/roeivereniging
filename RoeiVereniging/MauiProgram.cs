using RoeiVereniging.Core.Services;
using RoeiVereniging.ViewModels;
using RoeiVereniging.Views;
using Microsoft.Extensions.Logging;
using RoeiVereniging.Core.Interfaces.Services;
using RoeiVereniging.Core.Interfaces.Repositories;
using RoeiVereniging.Core.Data.Repositories;
using CommunityToolkit.Maui;

namespace RoeiVereniging
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            // Register interfaces and services
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<IClientService, ClientService>();

            // Register repositories
            builder.Services.AddSingleton<IClientRepository, ClientRepository>();
            builder.Services.AddSingleton<GlobalViewModel>();

            // Register views and viewmodels
            builder.Services.AddTransient<LoginView>().AddTransient<LoginViewModel>();

            return builder.Build();
        }
    }
}
