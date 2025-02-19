using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiTrading.ViewModel
{
    public partial class RegistrationViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient = new HttpClient();


        [ObservableProperty]
        private string username;
        [ObservableProperty]
        private string password;
        [ObservableProperty]
        private string repeatPassword;
        [ObservableProperty]
        private string name;

        public RegistrationViewModel()
        {
            RefresCanExecute();
        }

        private bool CanConfirm()
        {
            return!string.IsNullOrEmpty(Username) &&
                !string.IsNullOrEmpty(Name) &&
                !string.IsNullOrEmpty(Password) &&
                !string.IsNullOrEmpty(RepeatPassword) &&
                Regex.IsMatch(Username, @"^[A-Za-z0-9_]{5,20}$") &&
                Regex.IsMatch(Name, @"\b[A-Z][a-z]*\s[A-Z][a-z]*\b") &&
                Regex.IsMatch(Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*]).{8,}$") &&
                Password == RepeatPassword;
        }

        partial void OnUsernameChanged(string value) => RefresCanExecute();
        partial void OnNameChanged(string value) => RefresCanExecute();
        partial void OnPasswordChanged(string value) => RefresCanExecute();
        partial void OnRepeatPasswordChanged(string value) => RefresCanExecute();

        private void RefresCanExecute()
        {
            ConfirmCommand.NotifyCanExecuteChanged();
        }
        
        [RelayCommand(CanExecute = nameof(CanConfirm))]
        private async Task Confirm()
        {
            var newUser = new
            {
                username = Username,
                name = Name,
                password = Password
            };

            var json = JsonSerializer.Serialize(newUser);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var respons = await _httpClient.PostAsync("https://localhost:7247/api/users/register", content);
                
                if (respons == null)
                    throw new Exception("Could not reach server, try again later.");

                if (respons.IsSuccessStatusCode)
                {
                    await Shell.Current.DisplayAlert("Success", "User registered", "OK");

                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    if (respons.StatusCode == System.Net.HttpStatusCode.Conflict)
                        await Shell.Current.DisplayAlert("Error", "Username already taken.", "OK");
                    else
                        await Shell.Current.DisplayAlert("Error", "Failed to register", "OK");
                }
            }
            catch(Exception ex)
            {
                await Shell.Current.DisplayAlert("Network Error", "Could not reach server", "OK");
            }
        }

        [RelayCommand]
        async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }

    }
}
