using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
        public bool IsLong { get; set; }
        public DateTime TradeDate { get; set; } = DateTime.UtcNow;
        public bool IsOpen { get; set; } = true;
        public int PointsUsed { get; set; }
        public double? StopLoss { get; set; }
        public double? TakeProfit { get; set; }
        public double? PnLPercent { get; set; }
        public double? ClosingPrice { get; set; }
    }
}
