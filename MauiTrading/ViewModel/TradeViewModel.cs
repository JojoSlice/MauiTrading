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
using MauiTrading.Models;
using System.Reflection;

namespace MauiTrading.ViewModel
{
    public partial class TradeViewModel : INotifyPropertyChanged
    {

        private readonly ApiServiceFactory _apiServiceFactory;

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
        
        public TradeViewModel(ApiServiceFactory apiServiceFactory)
        {
            _apiServiceFactory = apiServiceFactory;
            Initialize();
        }

        private async void Initialize()
        {
            TradeOptions = await GetTradeOptions();
            var user = await GetUser();

            if (TradeOptions != null && TradeOptions.Count > 0)
            {
                SelectedOption = TradeOptions[0];
            }
        }
        public async Task<Models.User> GetUser()
        {
            var userService = _apiServiceFactory.CreateService<User>("user");
            return await userService.FetchDataAsync<string>();
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
            var tradeService = _apiServiceFactory.CreateService<bool>("trade");

            var result = await tradeService.FetchDataAsync(trade);
            if (!result)
            {
                await Shell.Current.DisplayAlert("Error", "Could not place trade", "Ok");
            }
        }
        
        private async Task LoadDataAsync(Models.Asset value)
        {
            await LoadData(value);
            _selectedAsset = await LoadPrice(value.Ticker);
        }
        public async Task<List<Models.Asset>> GetTradeOptions()
        {
            var assetService = _apiServiceFactory.CreateService<List<Asset>>("asset");

            var result = await assetService.FetchDataAsync<List<Models.Asset>>();
            return result;
        }
        public async Task<Models.Stock> LoadPrice(string ticker)
        {
            var stockService = _apiServiceFactory.CreateService<Stock>("stocks");
            return await stockService.FetchDataAsync(ticker);
        }

        public async Task LoadData(Models.Asset stock)
        {
            IsLoading = true;

            var candleService = _apiServiceFactory.CreateService<List<Candle>>("candle");
            var stockCandleData = await candleService.FetchDataAsync(stock);

            if (stockCandleData.Count != 0)
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
            IsLoading = false;
        }
    }
}
