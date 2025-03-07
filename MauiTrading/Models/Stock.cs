using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiTrading.Models
{
    public class Stock
    {
        public string Id { get; set; }
        [JsonPropertyName("ticker")]
        public string Ticker { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("price")]
        public float Price { get; set; }
        [JsonPropertyName("exchange")]
        public string Exchange { get; set; }
        [JsonPropertyName("updated")]
        public int Updated { get; set; }
        [JsonPropertyName("currency")]
        public string Currency { get; set; }
    }
}
