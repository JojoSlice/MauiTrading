namespace MauiTrading
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzcyOTk2MEAzMjM4MmUzMDJlMzBmblU5ZW5sZ2ZydjQ3ZENaVE1FUFJCbWQwMWdVNXFSWFdSRDZBaEkvcHhZPQ==");
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
        //protected override async void OnStart()
        //{
        //    if (await JWT.Service.IsTokenValidAsync())
        //    {
        //        await Shell.Current.GoToAsync(nameof(HomePage));
        //    }
        //    else
        //    {
        //        await Shell.Current.GoToAsync(nameof(MainPage));
        //    }
        //}
    }
}