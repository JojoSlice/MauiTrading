using MauiTrading.Models;
using MauiTrading.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTrading.Helpers
{
    public class LoadTradeHistory
    {
        private readonly ApiServiceFactory _apiServiceFactory;
        private User? userData;

        public LoadTradeHistory(ApiServiceFactory apiServiceFactory)
        {
            _apiServiceFactory = apiServiceFactory;
            Initialize();
        }

        public async void Initialize()
        {
            userData = await GetUser();
        }
        public async Task<Models.User> GetUser()
        {
            var userService = _apiServiceFactory.CreateService<User>("user");
            return await userService.FetchDataAsync<string>();
        }

        public async Task<List<TradeData>> LoadHistory()
        {
            if (userData != null)
            {

                var service = _apiServiceFactory.CreateService<List<TradeData>>("tradehistory");
                var tradeHistoryData = await service.FetchDataAsync(userData.Id);

                if (tradeHistoryData != null && tradeHistoryData.Count > 0)
                {
                    tradeHistoryData = tradeHistoryData.OrderByDescending(t => t.TradeDate).ToList();
                    return tradeHistoryData;
                }
            }
            return new List<TradeData>();
        }
    }
}
