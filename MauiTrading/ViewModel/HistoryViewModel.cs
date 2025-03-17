using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiTrading.Helpers;
using MauiTrading.Models;
using MauiTrading.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTrading.ViewModel
{
    public partial class HistoryViewModel : ObservableObject
    {
        private readonly ApiServiceFactory _apiFactory;
        private readonly LoadTradeHistory _loadTradeHistory;

        [ObservableProperty]
        public User userData;

        [ObservableProperty]
        public ObservableCollection<TradeData> tradeHistory;

        public HistoryViewModel(ApiServiceFactory apiServiceFactory, LoadTradeHistory loadTradeHistory) 
        {
            _apiFactory = apiServiceFactory;
            _loadTradeHistory = loadTradeHistory;

            Initialize();
        }

        public async void Initialize()
        {
            await UpdateUser();
        }

        [RelayCommand]
        async Task Back()
        {
            await Shell.Current.GoToAsync(nameof(HomePage));
        }

        async Task CloseTrade(TradeData trade)
        {
            if (trade != null)
            {
                var service = _apiFactory.CreateService<bool>("closetrade");
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

        private async Task UpdateUser()
        {
            UserData = await GetUser();
            await LoadTradeHistory();
        }

        public async Task<User> GetUser()
        {
            var userService = _apiFactory.CreateService<User>("user");
            return await userService.FetchDataAsync<User>();
        }

        public async Task LoadTradeHistory()
        {
            var tradeHistoryData = await _loadTradeHistory.LoadHistory();

            if (tradeHistoryData != null && tradeHistoryData.Count >= 0)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    if (TradeHistory == null)
                        TradeHistory = new();
                    else
                        TradeHistory.Clear();

                    foreach (var trade in tradeHistoryData)
                    {
                        if (trade.IsOpen)
                        {
                            trade.PriceNow = await GetPrice(trade.Ticker);
                            TradeHistory.Add(trade);
                        }
                        else
                        {
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
        public async Task<Double> GetPrice(string ticker)
        {
            var stockService = _apiFactory.CreateService<Stock>("stocks");
            var stock = await stockService.FetchDataAsync(ticker);

            return stock.Price;
        }

    }
}
