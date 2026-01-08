using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class PlayerStats
    {
        public int PlayerId { get; set; }
        public decimal? Ppg { get; set; }
        public decimal? Rpg { get; set; }
        public decimal? Apg { get; set; }
        public decimal? Spg { get; set; }
        public int AllStars { get; set; }
        public int Mvps { get; set; }
        public int Dpoys { get; set; }
        public int SixManAwards { get; set; }
        public int Rings { get; set; }
        public bool RookieOfTheYear { get; set; }
    }
}