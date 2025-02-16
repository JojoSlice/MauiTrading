using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiTrading;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTrading.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private string username;
        [ObservableProperty]
        private string password;

        [RelayCommand]
        void Login()
        {
            Debug.WriteLine($"Användare {Username} loggade in med lösen {Password}");
        }

        [RelayCommand]
        async Task Register()
        {
            await Shell.Current.GoToAsync(nameof(RegistrationPage));
        }
    }
}
