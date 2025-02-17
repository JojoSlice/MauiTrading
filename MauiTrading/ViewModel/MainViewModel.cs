using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiTrading;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MauiTrading.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient = new HttpClient();

        [ObservableProperty]
        private string username;
        [ObservableProperty]
        private string password;

        [RelayCommand]
        public async Task<bool> Login()
        {
            var loginData = new
            {
                Username = username,
                Password = password
            };

            var json = JsonSerializer.Serialize(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var respons = await _httpClient.PostAsync("https://localhost:7247/api/login", content);

            if (respons.IsSuccessStatusCode)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Login successful", "OK");
                return true;
            }

            await Application.Current.MainPage.DisplayAlert("Error", "Invalid username or password", "OK");
            return false;
        }

        [RelayCommand]
        async Task Register()
        {
            await Shell.Current.GoToAsync(nameof(RegistrationPage));
        }
    }
}
