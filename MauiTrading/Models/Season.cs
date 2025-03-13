using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiTrading.Models
{
    public class Season
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("startdate")]
        public DateTime StartDate { get; set; }
        [JsonPropertyName("enddate")]
        public DateTime? EndDate { get; set; }
        [JsonPropertyName("seasonmessage")]
        public string SeasonMessage { get; set; } = string.Empty;    
    }
}
