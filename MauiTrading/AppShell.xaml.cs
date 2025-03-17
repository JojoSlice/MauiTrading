namespace MauiTrading
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(TradePage), typeof(TradePage));
            Routing.RegisterRoute(nameof(HistoryPage), typeof(HistoryPage));
        }
    }
}
