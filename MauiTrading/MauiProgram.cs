﻿using MauiTrading.ViewModel;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using CommunityToolkit.Maui.Core;

namespace MauiTrading
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.ConfigureSyncfusionCore();
            builder.UseMauiApp<App>().UseMauiCommunityToolkitCore();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();

            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddSingleton<HomeViewModel>();

            builder.Services.AddTransient<RegistrationPage>();
            builder.Services.AddTransient<RegistrationViewModel>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
