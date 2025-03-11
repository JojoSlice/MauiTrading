using MauiTrading.Service;
using MauiTrading.ViewModel;

namespace MauiTrading;

public partial class TradePage : ContentPage
{
	private readonly ApiServiceFactory _apiServiceFactory;
	private readonly TradeViewModel _viewModel;
	public TradePage(ApiServiceFactory apiServiceFactory)
	{
		InitializeComponent();
		_apiServiceFactory = apiServiceFactory;
		_viewModel = new TradeViewModel(_apiServiceFactory);
		BindingContext = _viewModel;
	}
}