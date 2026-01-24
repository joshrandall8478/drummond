using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class PlayerStats
    {
        // adding these for check, might delete not sure.
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }

        public int PlayerId { get; set; }
        public decimal? Ppg { get; set; }
        public decimal? Rpg { get; set; }
        public decimal? Apg { get; set; }
        public decimal? Spg { get; set; }

        public decimal? Bpg {get ; set;}

        public string? College {get ; set;}
        public int? DraftYear {get ; set;}

        public int? IsLottery {get ; set;}

        public int YearsInLeague {get; set;}
        public int AllStars { get; set; }
        public int Mvps { get; set; }
        public int Dpoys { get; set; }
        public int SixManAwards { get; set; }
        public int Rings { get; set; }
        public bool RookieOfTheYear { get; set; }
        public List<string> Teams { get; set; } = new();
    }
}