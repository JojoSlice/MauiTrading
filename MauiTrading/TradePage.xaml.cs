using MauiTrading.Service;
using MauiTrading.ViewModel;

namespace MauiTrading;

public partial class TradePage : ContentPage
{
	private readonly SeasonService _seasonService;
	private readonly ApiServiceFactory _apiServiceFactory;
	private readonly TradeViewModel _viewModel;
	public TradePage(ApiServiceFactory apiServiceFactory, SeasonService seasonService)
	{
		InitializeComponent();
		_apiServiceFactory = apiServiceFactory;
		_seasonService = seasonService;
		_viewModel = new TradeViewModel(_apiServiceFactory, _seasonService);
		BindingContext = _viewModel;
	}
}