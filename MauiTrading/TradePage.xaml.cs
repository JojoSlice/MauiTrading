using MauiTrading.Service;
using MauiTrading.ViewModel;

namespace MauiTrading;

public partial class TradePage : ContentPage
{
    private readonly AuthService _authService;
	private readonly TradeViewModel _viewModel;
	public TradePage(AuthService authService)
	{
		InitializeComponent();
		_authService = authService;
		_viewModel = new TradeViewModel(new HttpClient(), _authService);
		BindingContext = _viewModel;
	}
}