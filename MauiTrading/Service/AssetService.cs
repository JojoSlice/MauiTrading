using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiTrading.Service
{
    public class AssetService : IApiService<List<Models.Asset>>
    {
        private readonly HttpClient _httpClient;

    public AssetService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

        public async Task<List<Models.Asset>> FetchDataAsync<TParam>(TParam? param)
        {
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7247/api/stocks/getassets");
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<Models.Asset>>(jsonResponse) ?? new List<Models.Asset>();
                }
                else
                {
                    throw new Exception("Could not get assets");
                }
            }
            catch(Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }

            return new List<Models.Asset>
            {
                new Models.Asset { Name = "Tesla", Ticker = "TSLA" },
                new Models.Asset { Name = "Apple", Ticker = "AAPL" }
            };
        }    
    }
}
