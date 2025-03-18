using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiTrading.Models
{
    public class PnLData
    {
        [JsonPropertyName("id")]
        public string id { get; set; }
        [JsonPropertyName("userId")]
        public string UserId { get; set; }
        [JsonPropertyName("seasonId")]
        public string SeasonId { get; set; }
        [JsonPropertyName("pnL")]
        public double PnL { get; set; }
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
    }
}
