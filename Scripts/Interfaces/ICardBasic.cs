using Godot;
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
    public static readonly Color BAD_COLOR = new Color("#FF0000");
    public static readonly Color GOOD_COLOR = new Color("#00AA00");
    public static readonly Color NEUTRAL_COLOR = new Color("#000");
}