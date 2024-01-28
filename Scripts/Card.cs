using System;
using System.Xml.Linq;
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

	[Export]
	public int MaxYOffset = 0;

	public CardAnimationState animationState = CardAnimationState.IDLE;

	public int index;

	private CardController _controller;

	private int XOffset = 0;

	public void Initialize(CardBasic card, int index, CardController controller, int XOffset)
	{
		Question = card.Question;
		Riposte = card.Riposte;
		Influence = card.Influence;
		this.index = index;
		_controller = controller;

		Influence.TryGetValue(Animal.BIRD, out int influence);
		var birdLabel = BirdSprite.GetChild<Label>(0);
		birdLabel.Text = influence.ToString();
		birdLabel.LabelSettings.FontColor = GetColor(influence);

		Influence.TryGetValue(Animal.CAT, out influence);
		var catLabel = CatSprite.GetChild<Label>(0);
		catLabel.Text = influence.ToString();
		catLabel.LabelSettings.FontColor = GetColor(influence);

		Influence.TryGetValue(Animal.FISH, out influence);
		var fishLabel = FishSprite.GetChild<Label>(0);
		fishLabel.Text = influence.ToString();
		fishLabel.LabelSettings.FontColor = GetColor(influence);

		Position = new Vector2(XOffset, 0);
		this.XOffset = XOffset;
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

	}

	public void PlayCard()
	{
		_controller.PlayCard(index);
	}

	public void RunAnimation()
	{
		animationState = CardAnimationState.SLIDE;
	}


	private Color GetColor(int v)
	{
		if (v == 0) return Constants.NEUTRAL_COLOR;
		else if (v > 0) return Constants.GOOD_COLOR;
		else return Constants.BAD_COLOR;
	}
}
