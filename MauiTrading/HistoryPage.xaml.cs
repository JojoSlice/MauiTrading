using MauiTrading.Helpers;
using MauiTrading.Service;
using MauiTrading.ViewModel;

namespace MauiTrading;

public partial class HistoryPage : ContentPage
{
	private readonly LoadTradeHistory _loadTradeHistory;
	private readonly ApiServiceFactory _apiFactory;
	private readonly HistoryViewModel _historyViewModel;
	public HistoryPage(HistoryViewModel historyViewModel, ApiServiceFactory apiServiceFactory, LoadTradeHistory loadTradeHistory)
	{
		_loadTradeHistory = loadTradeHistory;
		_apiFactory = apiServiceFactory;
		_historyViewModel = historyViewModel;
		_historyViewModel = new HistoryViewModel(_apiFactory, _loadTradeHistory);
		BindingContext = _historyViewModel;
		InitializeComponent();
	}
}