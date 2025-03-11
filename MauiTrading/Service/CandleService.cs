using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiTrading.Service
{
    public class CandleService : IApiService<List<Models.Candle>>
    {
        private readonly HttpClient _httpClient;

        public CandleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Models.Candle>> FetchDataAsync<Tparam>(Tparam tparam)
        {
            if (tparam is Models.Asset stock)
            {

                try
                {
                    var url = $"https://localhost:7247/api/stocks/stockprice?ticker={stock.Ticker}&period={stock.Period}";
                    var response = await _httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var stockCandleData = JsonSerializer.Deserialize<List<Models.Candle>>(jsonResponse);
                        if (stockCandleData.Count >= 0)
                            return stockCandleData;
                        else
                            throw new Exception("Could not find data");
                    }
                    else
                        throw new Exception("Could not read server");
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
                    return new List<Models.Candle>();
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Something went wrong", "Ok");
                return new List<Models.Candle>();
            }
        }
    }
}
