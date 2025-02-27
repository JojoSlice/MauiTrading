using MauiTrading.ViewModel;

namespace MauiTrading
{
    public partial class MainPage : ContentPage
    {
        private readonly MainViewModel _viewModel;
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }
    }
}
