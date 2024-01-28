using System;
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

	public CardAnimationState CardAnimationState = CardAnimationState.IDLE;

	public int index;

	private CardController _controller;

	private int XOffset = 0;
	private int YOffset = 0;
	private double _currentAnimationTime;
	private double _slideTime;
	private float _lerpWeight = 0;
	private Vector2 _initialPosition;
	private Vector2 _targetPosition;
	public bool ReadyToReinitialize { get; private set; }
	public bool SetReadyToReinitialize { get; private set; }

	[Export] public InfluenceColor catText;
	[Export] public InfluenceColor fishText;
	[Export] public InfluenceColor parrotText;

	public void Initialize(CardBasic card, int index, CardController controller, int XOffset, Vector2 targetPosition, double slideTime)
	{
		ReadyToReinitialize = false;
		_currentAnimationTime = 0;
		Question = card.Question;
		Riposte = card.Riposte;
		Influence = card.Influence;
		this.index = index;
		_controller = controller;
		_initialPosition = Position;
		_targetPosition = targetPosition;
		_slideTime = slideTime;
		this.XOffset = XOffset;

		Influence.TryGetValue(Animal.CAT, out int catInfluence);
		catText.Initialize(catInfluence);

		Influence.TryGetValue(Animal.FISH, out int fishInfluence);
		fishText.Initialize(fishInfluence);

		Influence.TryGetValue(Animal.BIRD, out int birdInfluence);
		parrotText.Initialize(birdInfluence);
		GD.Print("Init card ", GetParent().Name, ',', catInfluence, ',', fishInfluence, ',', birdInfluence);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		if (CardAnimationState == CardAnimationState.SLIDE_IN)
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

				if (SetReadyToReinitialize)
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
		if (CardAnimationState == CardAnimationState.SLIDE_IN && animationState == CardAnimationState.SLIDE_OUT ||
			CardAnimationState == CardAnimationState.SLIDE_OUT && animationState == CardAnimationState.SLIDE_IN)
		{
			_currentAnimationTime = _slideTime - _currentAnimationTime;
		}
		else
		{
			_currentAnimationTime = 0;
		}

		SetReadyToReinitialize = reinitializeOnSlide;
		CardAnimationState = animationState;
	}
}
