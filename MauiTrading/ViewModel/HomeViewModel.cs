using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiTrading.ViewModel
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient = new HttpClient();

        [ObservableProperty]
        private string marketStatus;
        [ObservableProperty]
        private string marketIsOpen;
        [ObservableProperty]
        private string marketHoliday;
        [ObservableProperty]
        private string marketSession;

        public async Task GetMarketStatus()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7247/api/market/status");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var marketData = JsonSerializer.Deserialize<Market>(jsonResponse);

                    if(marketData != null)
                    {
                        MarketStatus = $"Market: {marketData.Exchange}";

                        MarketIsOpen = marketData.IsOpen ? "Market is Open." : "Market is Closed.";
                        
                        MarketHoliday = $"Holiday: {marketData.Holiday ?? "None"}.";

                        MarketSession = $"Current session: {marketData.CurrentSession ?? "Unknown."}";
                    }
                    else
                    {
                        MarketStatus = "Could not find market data.";
                    }
                }
            }
            catch
            {
                await Shell.Current.DisplayAlert("Error", "Could not connect to market data provider", "Ok");
            }
        }
        public class Market
        {
            public string? Exchange { get; set; }
            public string? Holiday { get; set; }
            public bool IsOpen { get; set; }
            public string? CurrentSession { get; set; }
        }

    }
}
