using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTrading.Models
{
    public class User
    {
        public string Id { get; set; }

        public string Username { get; set; }
        public string Name { get; set; } 
        public string Password { get; set; } 

        public double Points { get; set; }
    }
}
