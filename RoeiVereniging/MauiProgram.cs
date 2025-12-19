using RoeiVereniging.Core.Services;
using RoeiVereniging.ViewModels;
using RoeiVereniging.Views;
using Microsoft.Extensions.Logging;
using RoeiVereniging.Core.Interfaces.Services;
using RoeiVereniging.Core.Interfaces.Repositories;
using RoeiVereniging.Core.Data.Repositories;
using CommunityToolkit.Maui;
using RoeiVereniging.Core.Repositories;

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
            // Services
            builder.Services.AddSingleton<IBoatService, BoatService>();
            builder.Services.AddSingleton<IReservationService, ReservationService>();
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<IEmailService, EmailService>();
            builder.Services.AddSingleton<IAuthService, AuthService>();

            // Repositories
            builder.Services.AddSingleton<IBoatRepository, BoatRepository>();
            builder.Services.AddSingleton<IReservationRepository, ReservationRepository>();
            builder.Services.AddSingleton<IUserRepository, UserRepository>();

            // Views and ViewModels
            builder.Services.AddTransient<LoginView>().AddTransient<LoginViewModel>();
            builder.Services.AddTransient<StartView>().AddTransient<StartViewModel>();
            builder.Services.AddTransient<ReserveBoatView>().AddTransient<ReserveBoatViewModel>();
            builder.Services.AddTransient<ReservationView>().AddTransient<ReservationViewModel>();
            builder.Services.AddTransient<AddBoatView>().AddTransient<AddBoatViewModel>();
            builder.Services.AddTransient<ReservationView>().AddTransient<ReservationViewModel>();
            builder.Services.AddTransient<WeatherView>().AddTransient<WeatherViewModel>();
            builder.Services.AddTransient<EditBoatView>().AddTransient<EditBoatViewModel>();
            
            builder.Services.AddSingleton<GlobalViewModel>();

            // Background Services
            builder.Services.AddSingleton<BadweatherCheckerBackgroundServices>();

            // Build the app
            var app = builder.Build();

            // After building the app, start the backgrouservice
            app.Services.GetService<BadweatherCheckerBackgroundServices>()?.Start();

            return app;
        }
    }
}
