using Godot;
using System;

public partial class Spectator : Node2D
{
	[Export] private Sprite2D _spriteHappy;
	[Export] private Sprite2D _spriteNeutral;
	[Export] private Sprite2D _spriteAnnoyed;
	[Export] private AngryCloud _angryCloud;
	[Export] private Emote _happyEmote;
	[Export] private Emote _neutralEmote;
	[Export] private Emote _angryEmote;



	[Export] public Animal Type { get; private set; }
	[Export] public SpectatorState State { get; private set; }
	[Export] public int Happiness { get; private set; } = 6;
	[Export] public Mood Mood { get; private set; }

	[Export] private float _speed = 5;
	private Seat _seat;
	private SpectatorController _controller;
	private Vector2 _entrance;
	private Vector2 _midpoint;
	private bool _midpointReached = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetMood(Mood.NEUTRAL);
		setAngryCloud(false);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		switch (State)
		{
			case SpectatorState.ENTERING:
				if (!_midpointReached)
				{
					_midpointReached = Step(_midpoint);
				}
				else
				{
					if (Step(_seat.GlobalPosition))
					{
						State = SpectatorState.WATCHING;
						_midpointReached = false;
					}
				}
				break;
			case SpectatorState.WATCHING:
				Happiness = Math.Clamp(Happiness, (int)Mood.EXIT, (int)Mood.HAPPY);
				setAngryCloud(Happiness == 1);

				if (Happiness > (int)Mood.NEUTRAL)
				{
					if (Mood != Mood.HAPPY) SetMood(Mood.HAPPY);
				}
				else if (Happiness > (int)Mood.ANNOYED)
				{
					if (Mood != Mood.NEUTRAL) SetMood(Mood.NEUTRAL);
				}
				else if (Happiness > (int)Mood.EXIT)
				{
					if (Mood != Mood.ANNOYED) SetMood(Mood.ANNOYED);
				}
				else
				{
					SetMood(Mood.EXIT);
				}
				break;
			case SpectatorState.EXITING:
				if (!_midpointReached)
				{
					_midpointReached = Step(_midpoint);
				}
				else
				{
					if (Step(_entrance)) _controller.Remove(this);
				}
				break;
		}
	}

	public void Initialize(SpectatorController controller, Vector2 entrance, Seat seat)
	{
		_controller = controller;
		_seat = seat;
		_seat.spectator = this;

		_entrance = entrance;
		_midpoint = new Vector2(entrance.X, _seat.GlobalPosition.Y);
	}

	public int ApplyCard(ICardBasic card)
	{
		if (card.Influence.TryGetValue(Type, out int value))
		{
			Happiness += value;
			GD.Print($"{Type}: {value}, {Happiness}");
			setEmote(value);
			return value;
		}
		return 0;
	}

	public void Annoy()
	{
		Happiness--;
	}

	/**
	 * returns true if target was reached
	 */
	private bool Step(Vector2 target)
	{
		Vector2 diff = target - GlobalPosition;
		Vector2 step = diff.Normalized() * _speed;

		var overshoot = _speed > diff.Length();
		GlobalPosition = overshoot ? target : GlobalPosition + step;

		return overshoot;
	}

	private void SetMood(Mood mood)
	{
		Mood = mood;
		switch (mood)
		{
			case Mood.HAPPY:
				_spriteHappy.Visible = true;
				_spriteNeutral.Visible = false;
				_spriteAnnoyed.Visible = false;
				break;
			case Mood.NEUTRAL:
				_spriteNeutral.Visible = true;
				_spriteHappy.Visible = false;
				_spriteAnnoyed.Visible = false;
				break;
			case Mood.ANNOYED:
				_spriteAnnoyed.Visible = true;
				_spriteHappy.Visible = false;
				_spriteNeutral.Visible = false;
				break;
			case Mood.EXIT:
				State = SpectatorState.EXITING;
				_seat.spectator = null;
				goto case Mood.ANNOYED;
		}
	}

	private void setAngryCloud(bool value)
	{
		_angryCloud.Visible = value;
	}

	private void setEmote(int value)
	{
		switch(value) 
		{
			case > 0:
				_happyEmote.Visible = true;
				break;
			case 0:
				_neutralEmote.Visible = true;
				break;
			case < 0:
				_angryEmote.Visible = true;
				break;
			default:
				_neutralEmote.Visible = true;
				break;
		}
	}
}

