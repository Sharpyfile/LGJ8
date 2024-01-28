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

	public CardAnimationState CardAnimationState = CardAnimationState.IDLE;

	public int index;

	private readonly Color BAD_COLOR = new Color("#FF0000");
	private readonly Color GOOD_COLOR = new Color("#00FF00");
	private readonly Color NEUTRAL_COLOR = new Color("#000");

	private CardController _controller;

	private int XOffset = 0;
	private int YOffset = 0;
	private double _currentAnimationTime;
	private double _slideTime;
	private float _lerpWeight = 0;
	private Vector2 _initialPosition;
	private Vector2 _targetPosition;
	public bool ReadyToReinitialize { get; private set; }
	public bool SetReadyToReinialize { get; private set; }

	public void Initialize(CardBasic card, int index, CardController controller, int XOffset, Vector2 targetPosition, double slideTime)
	{
		ReadyToReinitialize = false;
        _currentAnimationTime = 0;
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

		_initialPosition = Position;
		_targetPosition = targetPosition;
		_slideTime = slideTime;
        this.XOffset = XOffset;
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		if (CardAnimationState == CardAnimationState.SlIDE_IN)
		{
			_currentAnimationTime += delta;
			_lerpWeight = (float)(_currentAnimationTime / _slideTime);
            Position = Position.Lerp(_targetPosition, _lerpWeight);

			if (_currentAnimationTime > _slideTime)
			{
				Position = _targetPosition;
                CardAnimationState = CardAnimationState.TOP;
            }
		}
		else if (CardAnimationState == CardAnimationState.SLIDE_OUT)
        {
            _currentAnimationTime += delta;
            _lerpWeight = (float)(_currentAnimationTime / _slideTime);

            Position = Position.Lerp(_initialPosition, _lerpWeight);
            if (_currentAnimationTime > _slideTime)
            {
                Position = _initialPosition;
                CardAnimationState = CardAnimationState.IDLE;

				if (SetReadyToReinialize)				
                    ReadyToReinitialize = true;
                
            }
        }
    }

	public void PlayCard()
	{
		_controller.PlayCard(index);
	}

	public void RunAnimation(CardAnimationState animationState, bool reinitializeOnSlide = false)
	{
		// Get value between position and cast it as _currentAnimationTime
		if (CardAnimationState == CardAnimationState.SlIDE_IN && animationState == CardAnimationState.SLIDE_OUT ||
            CardAnimationState == CardAnimationState.SLIDE_OUT && animationState == CardAnimationState.SlIDE_IN)
		{
			_currentAnimationTime = _slideTime - _currentAnimationTime;
        }
		else
		{
			_currentAnimationTime = 0;
		}

		SetReadyToReinialize = reinitializeOnSlide;
        CardAnimationState = animationState;
	}


	private Color GetColor(int v)
	{
		if (v == 0) return NEUTRAL_COLOR;
		else if (v > 0) return GOOD_COLOR;
		else return BAD_COLOR;
	}
}
