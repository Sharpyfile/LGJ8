using Godot;
using Godot.Collections;

public class CardBasic : ICardBasic
{
	public string Text { get; set; }
	public Dictionary<Animal, int> Influence { get; set; }
}

public interface ICardBasic
{
	public string Text { get; set; }
	public Dictionary<Animal, int> Influence { get; set; }
}

public partial class Card : Node2D, ICardBasic
{

	private string _text;
	[Export]
	public string Text
	{
		get { return _text; }
		set
		{
			_text = value;
			BGSprite.GetChild<Label>(0).Text = value;
		}
	}

	[Export]
	public Dictionary<Animal, int> Influence { get; set; }

	private Sprite2D BGSprite { get { return GetChild<Sprite2D>(0); } }
	private Sprite2D Bird { get { return BGSprite.GetChild<Sprite2D>(1); } }
	private Sprite2D Cat { get { return BGSprite.GetChild<Sprite2D>(2); } }
	private Sprite2D Fish { get { return BGSprite.GetChild<Sprite2D>(3); } }

	private readonly Color BAD_COLOR = new Color("#FF0000");
	private readonly Color GOOD_COLOR = new Color("#00FF00");
	private readonly Color NEUTRAL_COLOR = new Color("#000");

	public void Initialize(CardBasic card)
	{
		Text = card.Text;
		Influence = card.Influence;

		Influence.TryGetValue(Animal.BIRD, out int influence);
		var birdLabel = Bird.GetChild<Label>(0);
		birdLabel.Text = influence.ToString();
		birdLabel.LabelSettings.FontColor = getColor(influence);

		Influence.TryGetValue(Animal.CAT, out influence);
		var catLabel = Cat.GetChild<Label>(0);
		catLabel.Text = influence.ToString();
		catLabel.LabelSettings.FontColor = getColor(influence);

		Influence.TryGetValue(Animal.FISH, out influence);
		var fishLabel = Fish.GetChild<Label>(0);
		fishLabel.Text = influence.ToString();
		fishLabel.LabelSettings.FontColor = getColor(influence);
	}


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private Color getColor(int v)
	{
		if (v == 0) return NEUTRAL_COLOR;
		else if (v > 0) return GOOD_COLOR;
		else return BAD_COLOR;
	}
}
