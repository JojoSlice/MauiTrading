using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiTrading.Models
{
    public class Candle
    {
        [JsonPropertyName("open")]
        public double Open { get; set; }
        [JsonPropertyName("low")]
        public double Low { get; set; }
        [JsonPropertyName("high")]
        public double High { get; set; }
        [JsonPropertyName("close")]
        public double Close { get; set; }
        [JsonPropertyName("volume")]
        public double Volume { get; set; }
        [JsonPropertyName("time")]
        public long Time { get; set; }

        public DateTime Date => DateTimeOffset.FromUnixTimeSeconds(Time).DateTime;
    }
}
