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
			JokeLabel.Text = value;
		}
	}

	[Export]
	public Dictionary<Animal, int> Influence { get; set; }

	[Export]
	public Label JokeLabel { get; private set; }
	[Export]
	public Sprite2D BirdSprite { get; private set; }
	[Export]
	public Sprite2D CatSprite { get; private set; }
	[Export]
	public Sprite2D FishSprite { get; private set; }

	private readonly Color BAD_COLOR = new Color("#FF0000");
	private readonly Color GOOD_COLOR = new Color("#00FF00");
	private readonly Color NEUTRAL_COLOR = new Color("#000");

	private CardController _controller;

	public void Initialize(CardBasic card, CardController controller)
	{
		Text = card.Text;
		Influence = card.Influence;
		_controller = controller;

		Influence.TryGetValue(Animal.BIRD, out int influence);
		var birdLabel = BirdSprite.GetChild<Label>(0);
		birdLabel.Text = influence.ToString();
		birdLabel.LabelSettings.FontColor = getColor(influence);

		Influence.TryGetValue(Animal.CAT, out influence);
		var catLabel = CatSprite.GetChild<Label>(0);
		catLabel.Text = influence.ToString();
		catLabel.LabelSettings.FontColor = getColor(influence);

		Influence.TryGetValue(Animal.FISH, out influence);
		var fishLabel = FishSprite.GetChild<Label>(0);
		fishLabel.Text = influence.ToString();
		fishLabel.LabelSettings.FontColor = getColor(influence);
	}

	public void PlayCard()
	{
		_controller.DiscardCard(this);
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
