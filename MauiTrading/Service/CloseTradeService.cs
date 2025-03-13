using MauiTrading.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiTrading.Service
{
    class CloseTradeService : IApiService<bool>
    {
        private readonly HttpClient _httpClient;

        public CloseTradeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> FetchDataAsync<Tparam>(Tparam trade)
        {
            var json = JsonSerializer.Serialize(trade);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var result = await _httpClient.PostAsync("https://localhost:7247/api/trade/closetrade", content);
                return result.IsSuccessStatusCode;
            }
            catch
            {
                await Shell.Current.DisplayAlert("Error", "Could not close trade", "Ok");
                return false;
            }

        }
    }
}
