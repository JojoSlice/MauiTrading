using MauiTrading.ViewModel;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using CommunityToolkit.Maui.Core;
using MauiTrading.Models;

namespace MauiTrading
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkitCore()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });



            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddSingleton(sp => Service.SeasonService.GetInstance(sp.GetRequiredService<HttpClient>()));
            builder.Services.AddSingleton<Service.AuthService>();
            builder.Services.AddSingleton<Service.ApiServiceFactory>();
            builder.Services.AddSingleton<Helpers.LoadTradeHistory>();

            builder.Services.AddKeyedScoped<Service.IApiService<List<Models.Candle>>, Service.CandleService>("candle");
            builder.Services.AddKeyedScoped<Service.IApiService<Models.Stock>, Service.StocksService>("stocks");
            builder.Services.AddKeyedScoped<Service.IApiService<Models.User>, Service.UserService>("user");
            builder.Services.AddKeyedScoped<Service.IApiService<bool>, Service.TradeService>("trade");
            builder.Services.AddKeyedScoped<Service.IApiService<List<Models.Asset>>, Service.AssetService>("asset");
            builder.Services.AddKeyedScoped<Service.IApiService<bool>, Service.RegistrationService>("register");
            builder.Services.AddKeyedScoped<Service.IApiService<List<TradeData>>, Service.TradeHistoryService>("tradehistory");
            builder.Services.AddKeyedScoped<Service.IApiService<bool>, Service.CloseTradeService>("closetrade");

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainViewModel>();

            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<HomeViewModel>();

            builder.Services.AddTransient<RegistrationPage>();
            builder.Services.AddTransient<RegistrationViewModel>();

            builder.Services.AddTransient<TradePage>();
            builder.Services.AddTransient<TradeViewModel>();

            builder.Services.AddTransient<HistoryPage>();
            builder.Services.AddTransient<HistoryViewModel>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
