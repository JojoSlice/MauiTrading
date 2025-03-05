using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTrading.Models
{
    public class Asset
    {
        public string Name { get; set; }
        public string Ticker { get; set; }
        public string Period { get; set; } = "1H";
    }
}
