using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTrading.ViewModel
{
    public class HomeDataViewModel
    {
        public List<MarketData> Data { get; set; }      

        public HomeDataViewModel()
        {
            Data = new List<MarketData>()
            {
                new MarketData { Hour = "09:00", Price = 100 },
                new MarketData { Hour = "10:00", Price = 110 },
                new MarketData { Hour = "11:00", Price = 108 },
                new MarketData { Hour = "12:00", Price = 113 },
                new MarketData { Hour = "13:00", Price = 120 },
                new MarketData { Hour = "14:00", Price = 115 },
                new MarketData { Hour = "15:00", Price = 112 },
            };
        }
    }
    public class MarketData
    {
        public string Hour { get; set; }
        public double Price { get; set; }
    }
}
