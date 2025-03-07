using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTrading.Models
{
    public class PnLData
    {
        public string UserId { get; set; } = string.Empty;
        public string SeasonId { get; set; } = string.Empty;
        public double PnL { get; set; } = 0;
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
