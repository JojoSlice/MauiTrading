using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json;

namespace MauiTrading.ViewModel
{
    public partial class TradeViewModel : ObservableObject
    {

        private readonly HttpClient _httpClient;
        public string Username { get; set; }

        [ObservableProperty]
        private List<Models.Asset> tradeOptions;

        [ObservableProperty]
        private Models.Asset selectedOption;

        [ObservableProperty]
        private bool isLoading;


        
        public ObservableCollection<Models.Candle> Data { get; set; } = new ObservableCollection<Models.Candle>();
        
        public TradeViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            Initialize();
        }


        private async void Initialize()
        {
            await GetUserName();
            TradeOptions = await GetTradeOptions();

            if (TradeOptions != null && TradeOptions.Count > 0)
            {
                SelectedOption = TradeOptions[0];
            }
        }

        partial void OnSelectedOptionChanged(Models.Asset value)
        {
            if (value != null)
            {
                _ = LoadDataAsync(value);
            }
        }

        private async Task LoadDataAsync(Models.Asset value)
        {
            await LoadData(value);
        }
        public async Task<List<Models.Asset>> GetTradeOptions()
        {
            var response = await _httpClient.GetAsync($"http://localhost:7247/api/stocks/getassets");
            if(response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var assets = JsonSerializer.Deserialize<List<Models.Asset>>(jsonResponse);

                if (assets != null)
                {
                    return assets;
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Could not load assets", "Ok");
                    return null;
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Could not reach server", "Ok");
                return null;
            }
        }

        public async Task LoadData(Models.Asset stock)
        {
            try
            {
                IsLoading = true;

                var response = await _httpClient.GetAsync($"http://localhost:7247/api/stocks/stockprice?stock={stock}");

                if(response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var stockCandleData = JsonSerializer.Deserialize<List<Models.Candle>>(jsonResponse);

                    if(stockCandleData != null)
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            Data.Clear();
                            foreach (var data in stockCandleData)
                            {
                                Data.Add(data);

                            }
                        });
                    }
                }
            }
            catch
            {
                await Shell.Current.DisplayAlert("Error", "Could not get stock data", "Ok");
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task GetUserName()
        {
            Username = await JWT.Service.GetUsernameAsync();
        }
    }
}
