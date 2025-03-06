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
        public double PnLPercent { get; set; } = 0;
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
