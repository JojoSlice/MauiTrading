using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiTrading.Service
{
    public class StocksService : IApiService<Models.Stock>
    {
        private readonly HttpClient _httpClient;

        public StocksService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Models.Stock> FetchDataAsync<Tparam>(Tparam ticker)
        {
            
            if (ticker is not string)
            {
                throw new ArgumentException("Ticker must be string");
            }

            try
            {
                var url = $"https://localhost:7247/api/stocks/price?ticker={ticker}";

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var stockData = JsonSerializer.Deserialize<Models.Stock>(jsonResponse);
                    return stockData ?? new Models.Stock();
                }
                else
                {
                    throw new Exception("Could not get stock data");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
                return new Models.Stock();
            }
        }
    }
}
