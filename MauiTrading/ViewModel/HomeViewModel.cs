using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiTrading.ViewModel
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public string Username { get; set; }
        public ObservableCollection<Models.PnLData> Data { get; set; } = new ObservableCollection<Models.PnLData>();

        public async void GetUserName()
        {
            Username = await JWT.Service.GetUsernameAsync();
        }

        public HomeViewModel()
        {
            LoadPnLData();
        }

        private void LoadPnLData()
        {

        }

    }
}
