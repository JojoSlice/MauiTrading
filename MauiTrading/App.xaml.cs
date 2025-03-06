using MauiTrading.Service;
using MauiTrading.ViewModel;
using System.Threading.Tasks;

namespace MauiTrading
{
    public partial class App : Application
    {
        private readonly AuthService _authService;
        private readonly HomeViewModel _homeViewModel;
        private readonly MainViewModel _mainViewModel;
        public App(AuthService authService, HomeViewModel homeViewModel, MainViewModel mainViewModel)
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzcyOTk2MEAzMjM4MmUzMDJlMzBmblU5ZW5sZ2ZydjQ3ZENaVE1FUFJCbWQwMWdVNXFSWFdSRDZBaEkvcHhZPQ==");

            _authService = authService;
            _homeViewModel = homeViewModel;
            _mainViewModel = mainViewModel;

        }
        protected override async void OnStart()
        {
            await SetMainPage();
        }
        private async Task SetMainPage()
        {
            bool autoLoginSuccess = await _authService.TryAutoLoginAsync();

            Window mainWindow = Application.Current.Windows.FirstOrDefault() ?? new Window();

            if (autoLoginSuccess)
            {
                mainWindow.Page = new HomePage(_homeViewModel, _authService); 
            }
            else
            {
                mainWindow.Page = new MainPage(_mainViewModel);
            }
        }
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}