using MauiTrading.ViewModel;

namespace MauiTrading
{
	public partial class HomePage : ContentPage
	{
		private readonly HomeViewModel _viewModel;
		public HomePage()
		{
			InitializeComponent();
			_viewModel = new HomeViewModel();
			BindingContext = _viewModel;
		}

		protected async void OnAppering()
		{
			base.OnAppearing();
			await _viewModel.GetMarketStatus();
		}
	}
}