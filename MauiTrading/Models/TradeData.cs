using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiTrading.Models
{
    public class TradeData
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string SeasonId { get; set; } = string.Empty;

        private string _ticker = string.Empty;
        public string Ticker
        { 
            get => _ticker;
            set => _ticker = value.ToUpper();
        }
        public double Price { get; set; }
        public double? PriceNow { get; set; }
        public bool IsLong { get; set; }
        public bool IsShort => !IsLong;
        public DateTime TradeDate { get; set; } = DateTime.UtcNow;
        public bool IsOpen { get; set; } = true;
        public bool IsClosed => !IsOpen;
        public int PointsUsed { get; set; }
        public double? StopLoss { get; set; }
        public double? TakeProfit { get; set; }
        public double? PnLPercent { get; set; }
        public double? ClosingPrice { get; set; }


        public event EventHandler CloseTradeRequested;

        public ICommand CloseTradeCommand { get; }

        public TradeData()
        {
            CloseTradeCommand = new RelayCommand(RequestCloseTrade);
        }

        public void RequestCloseTrade()
        {
            CloseTradeRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
