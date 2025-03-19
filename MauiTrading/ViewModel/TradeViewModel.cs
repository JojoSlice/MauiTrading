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
using System.Collections.Specialized;
using MauiTrading.Helpers;

namespace MauiTrading.ViewModel
{
    public partial class TradeViewModel : INotifyPropertyChanged
    {
        private readonly LoadTradeHistory _loadTradeHistory;
        private readonly SeasonService _seasonService;
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
                    OnPropertyChanged(nameof(SellPrice));
                }
            }
        }
        public double SellPrice => SelectedAsset != null ? SelectedAsset.Price - (SelectedAsset.Price / 200) : 0;

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
        private double _avaiablePoints;
        public double AvaiablePoints
        {
            get => _avaiablePoints;
            set
            {
                if (_avaiablePoints != value)
                {
                    _avaiablePoints = value;
                    OnPropertyChanged(nameof(AvaiablePoints));
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

                    Task.Run(async () =>
                    {
                        await LoadPrice(_selectedOption?.Ticker);
                    });
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

        private ObservableCollection<TradeData> _tradeHistory = new ObservableCollection<TradeData>();
        public ObservableCollection<TradeData> TradeHistory
        {
            get => _tradeHistory;
            set
            {
                if (_tradeHistory != value)
                {
                    _tradeHistory = value;
                    OnPropertyChanged(nameof(TradeHistory));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public ObservableCollection<Models.Candle> Data { get; set; } = new ObservableCollection<Models.Candle>();
        
        public TradeViewModel(ApiServiceFactory apiServiceFactory, SeasonService seasonService, LoadTradeHistory loadTradeHistory)
        {
            _loadTradeHistory = loadTradeHistory;
            _apiServiceFactory = apiServiceFactory;
            _seasonService = seasonService;
            Initialize();
        }

        private async void Initialize()
        {
            TradeOptions = await GetTradeOptions();
            await UpdateUser();

            if (TradeOptions != null && TradeOptions.Count > 0)
            {
                SelectedOption = TradeOptions[0];
            }
        }
        private async Task UpdateUser()
        {
            userData = await GetUser();
            AvaiablePoints = userData.Points;
            await LoadTradeHistory();
        }
        public async Task<Models.User> GetUser()
        {
            var userService = _apiServiceFactory.CreateService<User>("user");
            return await userService.FetchDataAsync<User>();
        }

        private void OnSelectedOptionChanged(Models.Asset value)
        {
            if (value != null)
            {
                _ = LoadData(value);
            }
        }

        [RelayCommand]
        async Task Buy()
        {
            var trade = BuildTrade(true);
            await SaveTrade(trade);

            await Shell.Current.DisplayAlert("", "Trade made", "Ok");
        }

        [RelayCommand]
        async Task Sell()
        {
            var trade = BuildTrade(false);
            await SaveTrade(trade);   
            await Shell.Current.DisplayAlert("", "Trade made", "Ok");
        }

        [RelayCommand]
        async Task Back()
        {
            await Shell.Current.GoToAsync(nameof(HomePage));
        }

        async Task CloseTrade(TradeData trade)
        {
            trade.ClosingPrice = trade.PriceNow;
            if (trade != null)
            {
                var service = _apiServiceFactory.CreateService<bool>("closetrade");
                var result = await service.FetchDataAsync(trade);
                if (!result)
                {
                    await Shell.Current.DisplayAlert("Error", "Could not place trade", "Ok");
                }
                else
                {
                    await UpdateUser();
                }
            }

        }

        public Models.TradeData BuildTrade(bool isLong)
        {
            var trade = new Models.TradeData
            {
                UserId = userData.Id,
                SeasonId = _seasonService.Season.Id,
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
            else
            {
                await UpdateUser();
            }
        }
        
        public async Task<List<Models.Asset>> GetTradeOptions()
        {
            var assetService = _apiServiceFactory.CreateService<List<Asset>>("asset");

            var result = await assetService.FetchDataAsync<List<Models.Asset>>();
            return result;
        }

        public async Task LoadPrice(string ticker)
        {
            var stockService = _apiServiceFactory.CreateService<Stock>("stocks");
            var stock = await stockService.FetchDataAsync(ticker);
            SelectedAsset = stock;
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
        public async Task<Double> GetPrice(string ticker)
        {
            var stockService = _apiServiceFactory.CreateService<Stock>("stocks");
            var stock = await stockService.FetchDataAsync(ticker);

            return stock.Price;
        }
        public async Task LoadTradeHistory()
        {
            var tradeHistoryData = await _loadTradeHistory.LoadHistory();
            var tradesInSeason = tradeHistoryData.Where(td => td.SeasonId == _seasonService.Season.Id).ToList();

            if (tradesInSeason != null && tradesInSeason.Count >= 0)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    TradeHistory?.Clear();

                    foreach (var trade in tradesInSeason)
                    {
                        if (trade.IsOpen)
                        {
                            trade.PriceNow = await GetPrice(trade.Ticker);
                            TradeHistory.Add(trade);
                        }
                    }
                    foreach (var trade in TradeHistory)
                    {
                        trade.CloseTradeRequested += async (sender, args) =>
                        {
                            var tradeData = sender as TradeData;
                            if (tradeData != null)
                            {
                                await CloseTrade(tradeData);
                            }
                        };
                    }
                });
            }
        }
    }
}
