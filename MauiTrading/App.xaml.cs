﻿using MauiTrading.Service;
using MauiTrading.ViewModel;
using System.Threading.Tasks;

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
    }
}