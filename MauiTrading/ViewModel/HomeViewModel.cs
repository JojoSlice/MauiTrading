using CommunityToolkit.Mvvm.ComponentModel;
using MauiTrading.Chart;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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

        public ObservableCollection<MarketData> Data { get; set; } = new ObservableCollection<MarketData>();

        public HomeViewModel()
        {
            LoadMarketData();
        }

        private void LoadMarketData()
        {
            Data.Clear();

            Data.Add(new MarketData { Hour = "09:00", Price = 100 });
            Data.Add(new MarketData { Hour = "10:00", Price = 110 });
            Data.Add(new MarketData { Hour = "11:00", Price = 108 });
            Data.Add(new MarketData { Hour = "12:00", Price = 113 });
            Data.Add(new MarketData { Hour = "13:00", Price = 120 });
            Data.Add(new MarketData { Hour = "14:00", Price = 115 });
            Data.Add(new MarketData { Hour = "15:00", Price = 112 });
        }

        public class MarketData
        {
            public string Hour { get; set; }
            public double Price { get; set; }
        } 


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
                        MarketStatus = $"Market: {marketData.Exchange ?? "None"}";

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
            [JsonPropertyName("exchange")]
            public string? Exchange { get; set; }
            [JsonPropertyName("holiday")]
            public string? Holiday { get; set; }
            [JsonPropertyName("isOpen")]
            public bool IsOpen { get; set; }
            [JsonPropertyName("session")]
            public string? CurrentSession { get; set; }
        }

    }
}
