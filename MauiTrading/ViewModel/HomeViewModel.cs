using CommunityToolkit.Mvvm.ComponentModel;
using MauiTrading.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiTrading.ViewModel
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public string Username { get; set; }
        public ObservableCollection<Models.PnLData> Data { get; set; } = new ObservableCollection<Models.PnLData>();

        public async void GetUserName()
        {
            Username = await JWT.Service.GetUsernameAsync();
        }

        public HomeViewModel()
        {
            LoadPnLData();
        }

        public async Task<List<PnLData>> LoadPnLData()
        {
            try
            {
                var response = await _httpClient.GetAsync("http//localhost:7247/api/pnl/getpnl");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var pnLData = JsonSerializer.Deserialize<List<PnLData>>(jsonResponse);
                    return pnLData ?? new List<PnLData>();
                }
            }
            catch
            {
                await Shell.Current.DisplayAlert("Error", "Could not get PnL data.", "OK");
            }

            return new List<PnLData>();
        }

    }
}
