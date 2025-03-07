using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiTrading.Models
{
    public class Asset
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("ticker")]
        public string Ticker { get; set; }
        [JsonPropertyName("period")]
        public string Period { get; set; } = "1H";
    }
}
