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
            // Services
            builder.Services.AddSingleton<IBoatService, BoatService>();
            builder.Services.AddSingleton<IReservationService, ReservationService>();

            // Repositories
            builder.Services.AddSingleton<IBoatRepository, BoatRepository>();
            builder.Services.AddSingleton<IReservationRepository, ReservationRepository>();

            // Views and ViewModels
            builder.Services.AddTransient<StartView>().AddTransient<StartViewModel>();
            builder.Services.AddTransient<ReserveBoatView>().AddTransient<ReserveBoatViewModel>();
            builder.Services.AddTransient<AddBoatView>().AddTransient<AddBoatViewModel>();
            return builder.Build();
        }
    }
}
