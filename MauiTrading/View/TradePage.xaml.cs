using MauiTrading.Service;
using MauiTrading.ViewModel;

namespace MauiTrading;

public partial class TradePage : ContentPage
{
	private readonly Helpers.LoadTradeHistory _loadTrade;
	private readonly SeasonService _seasonService;
	private readonly ApiServiceFactory _apiServiceFactory;
	private readonly TradeViewModel _viewModel;
	public TradePage(ApiServiceFactory apiServiceFactory, SeasonService seasonService, Helpers.LoadTradeHistory loadTradeHistory)
	{
		InitializeComponent();
		_loadTrade = loadTradeHistory;
		_apiServiceFactory = apiServiceFactory;
		_seasonService = seasonService;
		_viewModel = new TradeViewModel(_apiServiceFactory, _seasonService, _loadTrade);
		BindingContext = _viewModel;
	}
}