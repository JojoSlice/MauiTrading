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
        public RegistrationPage(RegistrationViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }
    }
}
