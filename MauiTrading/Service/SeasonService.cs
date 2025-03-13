using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiTrading.Service
{
    public class SeasonService
    {
        private static SeasonService _instance;
        private static readonly object _lock = new();
        private readonly HttpClient _httpClient;
        private Models.Season? _season;

        public Models.Season? Season => _season;

        private SeasonService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _ = LoadSeason();
        }

        public static SeasonService GetInstance(HttpClient httpClient)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new(httpClient);
                    }
                }
            }

            return _instance;
        }

        public async Task LoadSeason()
        {
            _season = await GetCurrentSeason();
        }

        public async Task<Models.Season> GetCurrentSeason()
        {
            var url = "https://localhost:7247/api/season/get";
            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Models.Season>(jsonResponse) ?? throw new Exception("Season not found");
            }
            catch(Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
                return new Models.Season();
            }
        }
    }
}
