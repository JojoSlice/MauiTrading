using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTrading.Models
{
    public class PnLData
    {
        public string UserId { get; set; }
        public double PnL { get; set; }
        public DateTime Date { get; set; }
    } 
}
