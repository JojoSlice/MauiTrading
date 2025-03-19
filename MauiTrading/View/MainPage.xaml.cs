using MauiTrading.Service;
using MauiTrading.ViewModel;

namespace MauiTrading
{
    public partial class MainPage : ContentPage
    {
        private readonly AuthService _authService;
        private readonly MainViewModel _mainViewModel;

        public MainPage(AuthService authService, MainViewModel mainViewModel)
        {
            InitializeComponent();
            _authService = authService;
            _mainViewModel = mainViewModel;
            BindingContext = _mainViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await TryAutoLogin();
        }

        private async Task TryAutoLogin()
        {
            bool autoLoginSuccess = await _authService.TryAutoLoginAsync();

            if (autoLoginSuccess)
            {
                await Shell.Current.GoToAsync(nameof(HomePage));
            }
        }
    }
}
