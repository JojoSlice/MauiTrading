using MauiTrading.Service;
using MauiTrading.ViewModel;

namespace MauiTrading
{
    public partial class HomePage : ContentPage
    {
        private readonly HomeViewModel _viewModel;
        private readonly AuthService _authService;

        public HomePage(AuthService authService)
        {
            InitializeComponent();
            _authService = authService;

            _viewModel = new HomeViewModel(new HttpClient(), _authService);
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadPnLDataAsync();
        }

        private async Task LoadPnLDataAsync()
        {
            await _viewModel.LoadPnLData();
        }
    }
}