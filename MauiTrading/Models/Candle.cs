using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace MauiTrading.Models
{
    public class Candle
    {
        public double Open { get; set; }
        public double Low { get; set; }
        public double High { get; set; }
        public double Close { get; set; }
        public double Volume { get; set; }
        public DateTime Date { get; set; }
    }
}
