using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiTrading.Models
{
    public class User
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        [JsonPropertyName ("username")]
        public string Username { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonPropertyName("points")]
        public double Points { get; set; } = 1000;
    }
}
