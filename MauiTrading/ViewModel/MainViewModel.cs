using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiTrading;
using MauiTrading.Service;
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
        private readonly AuthService _authService;


        [ObservableProperty]
        private string username;
        [ObservableProperty]
        private string password;


        public MainViewModel(AuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        public async Task<bool> Login()
        {
            Service.AuthService.LoginDto loginDto = new Service.AuthService.LoginDto { Password = password, Username = username };

            return await _authService.LoginAsync(loginDto);
        }

        [RelayCommand]
        async Task Register()
        {
            await Shell.Current.GoToAsync(nameof(RegistrationPage));
        }
    }
}
