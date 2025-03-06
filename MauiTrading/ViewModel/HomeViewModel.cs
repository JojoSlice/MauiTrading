using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiTrading.Models;
using MauiTrading.Service;
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
        private readonly AuthService _authService;
        public ObservableCollection<Models.PnLData> Data { get; set; } = new ObservableCollection<Models.PnLData>();

        public HomeViewModel(HttpClient httpClient, AuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
            Initialize();
        }

        private async void Initialize()
        {
            await LoadPnLData();
        }
        
        [RelayCommand]
        async Task Trade()
        {
            await Shell.Current.GoToAsync(nameof(TradePage));
        }
        
        [RelayCommand]
        async Task TradeHistory()
        {

        }
        
        [RelayCommand]
        async Task LogOut()
        {
            JWT.Service.RemoveToken();
            await Shell.Current.GoToAsync(nameof(MainPage));
        }
        //Lös det hära!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public async Task LoadPnLData()
        {
            var currentUser = _authService.CurrentUser;
            if (currentUser == null)
            {
                await Shell.Current.DisplayAlert("Error", "User is not logged in.", "OK");
                return;
            }

            var json = JsonSerializer.Serialize(currentUser);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync($"http://localhost:7247/api/pnl/getseasonpnl", content);

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
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", "No PnL data found.", "OK");
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to fetch PnL data. Please try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                // Fångar eventuella undantag
                await Shell.Current.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            } 
        }
    }
}
