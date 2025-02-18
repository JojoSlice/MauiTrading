using MauiTrading.ViewModel;

namespace MauiTrading;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
		BindingContext = new HomeViewModel();
	}
}