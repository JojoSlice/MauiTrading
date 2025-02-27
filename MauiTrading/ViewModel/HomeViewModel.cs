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
        private readonly HttpClient _httpClient;
        public string Username { get; set; }
        public ObservableCollection<Models.PnLData> Data { get; set; } = new ObservableCollection<Models.PnLData>();

        public HomeViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            Initialize();
        }

        private async void Initialize()
        {
            await GetUserName();
            await LoadPnLData();
        }
        public async Task GetUserName()
        {
            Username = await JWT.Service.GetUsernameAsync();
        }

        public async Task LoadPnLData()
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://localhost:7247/api/pnl/getpnl?username={Username}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var pnLData = JsonSerializer.Deserialize<List<PnLData>>(jsonResponse);

                    if (pnLData != null)
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            Data.Clear();
                            foreach (var item in pnLData)
                            {
                                Data.Add(item);
                            }
                        });
                    }
                }
            }
            catch
            {
                await Shell.Current.DisplayAlert("Error", "Could not get PnL data.", "OK");
            }
        }
    }
}
