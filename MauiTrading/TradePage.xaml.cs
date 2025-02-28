using MauiTrading.ViewModel;

namespace MauiTrading;

public partial class TradePage : ContentPage
{
	private readonly TradeViewModel _viewModel;
	public TradePage(TradeViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}
}