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
        private readonly HttpClient _httpClient;

        [ObservableProperty]
        private string username;
        [ObservableProperty]
        private string password;
        // [ObservableProperty]
        //private bool isWindows;


        public MainViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            //IsWindows = DeviceInfo.Platform == DevicePlatform.WinUI;
        }

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

            try
            {
                var respons = await _httpClient.PostAsync("https://localhost:7247/api/users/login", content);

                if (respons == null)
                    throw new Exception("Could not reach server, try again later.");

                if (respons.IsSuccessStatusCode)
                {
                    var responsBody = await respons.Content.ReadAsStringAsync();
                    var responsObject = JsonSerializer.Deserialize<JsonElement>(responsBody);

                    var token = responsObject.GetProperty("token").GetString();
                    await JWT.Service.SaveTokenAsync(token);

                    await Shell.Current.GoToAsync(nameof(HomePage));
                    return true;
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Invalid username or password", "OK");
                    return false;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Network Error", "Could not reach server", "OK");
                return false;
            }
        }

        [RelayCommand]
        async Task Register()
        {
            await Shell.Current.GoToAsync(nameof(RegistrationPage));
        }

        //[RelayCommand]
        //void Quit()
        //{
        //    if (isWindows)
        //        Application.Current.Quit();
        //}
    }
}
