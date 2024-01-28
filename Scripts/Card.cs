using Godot;
using Godot.Collections;



public partial class Card : Node2D, ICardBasic
{

	private string _question;
	[Export]
	public string Question
	{
		get { return _question; }
		set
		{ _question = value; QuestionLabel.Text = value; }
	}

	private string _riposte;
	[Export]
	public string Riposte
	{
		get { return _riposte; }
		set { _question = value; RiposteLabel.Text = value; }
	}

	[Export]
	public Dictionary<Animal, int> Influence { get; set; }

	[Export]
	public Label QuestionLabel { get; private set; }
	[Export]
	public Label RiposteLabel { get; private set; }

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

	public int index;

	public void Initialize(CardBasic card, int index, CardController controller)
	{
		Question = card.Question;
		Riposte = card.Riposte;
		Influence = card.Influence;
		this.index = index;
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
		_controller.PlayCard(index);
	}


	private Color getColor(int v)
	{
		if (v == 0) return NEUTRAL_COLOR;
		else if (v > 0) return GOOD_COLOR;
		else return BAD_COLOR;
	}
}
