using Godot.Collections;
public class CardBasic : ICardBasic
{
    public string Question { get; set; }
    public string Riposte { get; set; }
    public Dictionary<Animal, int> Influence { get; set; }
}

public interface ICardBasic
{
    public string Question { get; set; }
    public string Riposte { get; set; }
    public Dictionary<Animal, int> Influence { get; set; }
}

public class Constants
{
    public static readonly int HAND_SIZE = 5;
}