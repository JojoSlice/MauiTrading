using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiTrading.ViewModel
{
    public partial class TradeViewModel : ObservableObject
    {

        public string Username { get; set; }
        private readonly HttpClient _httpClient;

        [ObservableProperty]
        private List<Models.Asset> options = new();

        [ObservableProperty]
        private Models.Asset selectedOption;
        public ObservableCollection<Models.Candle> Data { get; set; } = new ObservableCollection<Models.Candle>();
        public TradeViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            Initialize();
        }


        private async void Initialize()
        {
            await GetUserName();
        }

        public async Task LoadData()
        {
            Data.Clear();

            
        }

        public async Task GetUserName()
        {
            Username = await JWT.Service.GetUsernameAsync();
        }
    }
}
