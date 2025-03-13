using MauiTrading.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiTrading.Service
{
    public class TradeHistoryService : IApiService<List<TradeData>>
    {
        private readonly HttpClient _httpClient;

        public TradeHistoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TradeData>> FetchDataAsync<Tparam>(Tparam userId)
        {
            try
            {

                var response = await _httpClient.GetAsync($"https://localhost:7247/api/trade/tradeseasonalhistory?userId={userId}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<TradeData>>() ?? throw new Exception("TradeData not found");
            }
            catch(Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
                return new List<TradeData>();
            }
        }
    }
}
