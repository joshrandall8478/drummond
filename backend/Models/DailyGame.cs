namespace backend.Models
{
    public class DailyGame
    {
        public int DailyGameId { get; set; }
        public string? Round1Category1 { get; set; }
        public string? Round1Category2 { get; set; }
        public string? Round2Category1 { get; set; }
        public string? Round2Category2 { get; set; }
        public string? Round3Category1 { get; set; }
        public string? Round3Category2 { get; set; }
        public string? Round4Category1 { get; set; }
        public string? Round4Category2 { get; set; }
        public string? Round5Category1 { get; set; }
        public string? Round5Category2 { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public GameCriteria GetCriteriaForRound(int round)
        {
            return round switch
            {
                1 => new GameCriteria { Category1 = Round1Category1, Category2 = Round1Category2 },
                2 => new GameCriteria { Category1 = Round2Category1, Category2 = Round2Category2 },
                3 => new GameCriteria { Category1 = Round3Category1, Category2 = Round3Category2 },
                4 => new GameCriteria { Category1 = Round4Category1, Category2 = Round4Category2 },
                5 => new GameCriteria { Category1 = Round5Category1, Category2 = Round5Category2 },
                _ => throw new ArgumentException($"Invalid round: {round}")
            };
        }
    }
}