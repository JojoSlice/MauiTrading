using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiTrading.Service;
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
        private readonly ApiServiceFactory _apiServiceFactory;


        [ObservableProperty]
        private string username;
        [ObservableProperty]
        private string password;
        [ObservableProperty]
        private string repeatPassword;
        [ObservableProperty]
        private string name;

        public RegistrationViewModel(ApiServiceFactory apiServiceFactory)
        {
            _apiServiceFactory = apiServiceFactory;
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
            var registration = _apiServiceFactory.CreateService<bool>("register");
            var result = await registration.FetchDataAsync(newUser);
            if (result)
            {
                await Shell.Current.GoToAsync("..");
            }
        }

        [RelayCommand]
        async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }

    }
}
