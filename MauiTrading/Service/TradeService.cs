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
    public class TradeService : IApiService<bool>
    {
        private readonly HttpClient _httpClient;

        public TradeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> FetchDataAsync<TParam>(TParam trade)
        {
            var json = JsonSerializer.Serialize(trade);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var result = await _httpClient.PostAsync("https://localhost:7247/api/trade/newtrade", content);
                return result.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
