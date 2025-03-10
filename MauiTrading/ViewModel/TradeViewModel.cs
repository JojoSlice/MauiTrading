using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiTrading.Service;
using System.Runtime.CompilerServices;
using System.Net;

namespace MauiTrading.ViewModel
{
    public partial class TradeViewModel : INotifyPropertyChanged
    {

        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;

        private Models.User userData;

        private double _stopLoss;
        public double StopLoss
        {
            get => _stopLoss;
            set
            {
                if (_stopLoss != value)
                {
                    _stopLoss = value;
                    OnPropertyChanged(nameof(StopLoss));
                }
            }
        }
        private double _takeProfit;
        public double TakeProfit
        {
            get => _takeProfit;
            set
            {
                if (_takeProfit != value)
                {
                    _takeProfit = value;
                    OnPropertyChanged(nameof(TakeProfit));
                }
            }
        }
        private Models.Stock _selectedAsset;
        public Models.Stock SelectedAsset
        {
            get => _selectedAsset;
            set
            {
                if (_selectedAsset != value)
                {
                    _selectedAsset = value;
                    OnPropertyChanged(nameof(SelectedAsset));
                }
            }
        }
        private int _points;
        public int Points
        {
            get => _points;
            set
            {
                if (_points != value)
                {
                    _points = value;
                    OnPropertyChanged(nameof(Points));
                }
            }
        }
        private Models.Asset _selectedOption;
        public Models.Asset SelectedOption
        {
            get => _selectedOption;
            set
            {
                if (_selectedOption != value)
                {
                    _selectedOption = value;
                    OnPropertyChanged(nameof(SelectedOption));

                    OnSelectedOptionChanged(_selectedOption);
                }
            }
        }

        private List<Models.Asset> _tradeOptions;
        public List<Models.Asset> TradeOptions
        {
            get => _tradeOptions;
            set
            {
                if (_tradeOptions != value)
                {
                    _tradeOptions = value;
                    OnPropertyChanged(nameof(TradeOptions));
                }
            }
        }


        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged(nameof(IsLoading));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public ObservableCollection<Models.Candle> Data { get; set; } = new ObservableCollection<Models.Candle>();
        
        public TradeViewModel(HttpClient httpClient, AuthService authService)
        {
            _authService = authService;
            _httpClient = httpClient;
            Initialize();
        }

        private async void Initialize()
        {
            TradeOptions = await GetTradeOptions();
            await GetUser();

            if (TradeOptions != null && TradeOptions.Count > 0)
            {
                SelectedOption = TradeOptions[0];
            }
        }
        public async Task GetUser()
        {
            var user = _authService.CurrentUser;
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:7247/api/users/getuser");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStreamAsync();
                    userData = JsonSerializer.Deserialize<Models.User>(jsonResponse);
                    if (userData == null)
                    {
                        throw new Exception("Could not find userData");
                    }
                }
                else 
                {
                    throw new Exception("Could not connect to server");
                }
            }
            catch(Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }

        }

        private void OnSelectedOptionChanged(Models.Asset value)
        {
            if (value != null)
            {
                _ = LoadDataAsync(value);
            }
        }

        [RelayCommand]
        public async Task Buy()
        {
            var trade = BuildTrade(true);
            await SaveTrade(trade);   
        }

        [RelayCommand]
        public async Task Sell()
        {
            var trade = BuildTrade(false);
            await SaveTrade(trade);   
        }

        public Models.TradeData BuildTrade(bool isLong)
        {
            var trade = new Models.TradeData
            {
                UserId = userData.Id,
                Ticker = SelectedAsset.Ticker,
                Price = SelectedAsset.Price,
                IsLong = isLong,
                TradeDate = DateTime.UtcNow,
                PointsUsed = Points,
                StopLoss = StopLoss,
                TakeProfit = TakeProfit
            };
            return trade;
        }
        public async Task SaveTrade(Models.TradeData trade)
        {
            var json = JsonSerializer.Serialize(trade);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            try
            {
                var result = await _httpClient.PostAsync("https://localhost:7247/api/trade/newtrade", content);
                if (result == null)
                    throw new Exception("Could not reach server, try again.");

                if (result.IsSuccessStatusCode)
                {
                    await Shell.Current.DisplayAlert("Success", "Trade is made", "Ok");
                }
                else
                    throw new Exception(HttpStatusCode.Conflict.ToString());
            }
            catch(Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
        }
        
        private async Task LoadDataAsync(Models.Asset value)
        {
            await LoadData(value);
            _selectedAsset = await LoadPrice(value.Ticker);
        }
        public async Task<List<Models.Asset>> GetTradeOptions()
        {
            try
            {

                var response = await _httpClient.GetAsync($"https://localhost:7247/api/stocks/getassets");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var assets = JsonSerializer.Deserialize<List<Models.Asset>>(jsonResponse);

                    if (assets != null)
                    {
                        return assets;
                    }
                    else
                    {
                        throw new Exception("Could not load assets");
                    }
                }
                else
                {
                    throw new Exception("Could not reach server");
                }
            }
            catch(Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
                var tempAssets = new List<Models.Asset>
                {
                    new Models.Asset { Name = "Tesla", Ticker = "TSLA" },
                    new Models.Asset { Name = "Apple", Ticker = "AAPL" },
                };
                return tempAssets;
            }
        }
        public async Task<Models.Stock> LoadPrice(string ticker)
        {
            try
            {
                var url = $"https://localhost:7247/api/stocks/price?ticker={ticker}";
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var stockData = JsonSerializer.Deserialize<Models.Stock>(jsonResponse);

                    if (stockData != null)
                        return stockData;
                }
                else
                {
                    throw new Exception("Could not get stock data");
                }
            }
            catch(Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
            return new Models.Stock();
        }

        public async Task LoadData(Models.Asset stock)
        {
            try
            {
                IsLoading = true;
                var url = $"https://localhost:7247/api/stocks/stockprice?ticker={stock.Ticker}&period={stock.Period}";
                var response = await _httpClient.GetAsync(url);

                if(response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var stockCandleData = JsonSerializer.Deserialize<List<Models.Candle>>(jsonResponse);

                    if(stockCandleData.Count != 0)
                    {
                        stockCandleData = stockCandleData.OrderBy(c => c.Date).ToList();
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
    }
}
