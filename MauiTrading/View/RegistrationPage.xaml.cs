using MauiTrading.Service;
using MauiTrading.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTrading
{
    public partial class RegistrationPage : ContentPage
    {
        private readonly RegistrationViewModel _viewModel;
        private readonly ApiServiceFactory _apiServiceFactory;
        public RegistrationPage(ApiServiceFactory apiServiceFactory)
        {
            _apiServiceFactory = apiServiceFactory;
            _viewModel = new RegistrationViewModel(_apiServiceFactory);
            BindingContext = _viewModel;
            InitializeComponent();
        }
    }
}
