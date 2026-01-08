namespace backend.Models;

public class GameLineup
{
    public int? PG { get; set; }
    public int? SG { get; set; }
    public int? SF { get; set; }
    public int? PF { get; set; }
    public int? C { get; set; }
    
    public bool IsPositionFilled(string position)
    {
        return position switch
        {
            "PG" => PG.HasValue,
            "SG" => SG.HasValue,
            "SF" => SF.HasValue,
            "PF" => PF.HasValue,
            "C" => C.HasValue,
            _ => false
        };
    }
    
    public bool IsComplete()
    {
        return PG.HasValue && SG.HasValue && SF.HasValue && PF.HasValue && C.HasValue;
    }
}