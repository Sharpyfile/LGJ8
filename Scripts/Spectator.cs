using Godot;
using System;

public partial class Spectator : Node2D
{
	[Export] private Sprite2D _spriteHappy;
	[Export] private Sprite2D _spriteNeutral;
	[Export] private Sprite2D _spriteAnnoyed;



	[Export] public Animal Type { get; private set; }
	[Export] public SpectatorState State { get; private set; }
	[Export] public int Happiness { get; private set; } = 6;
	[Export] public Mood Mood { get; private set; }

	[Export] private float _speed = 5;
	private Seat _seat;
	private Node2D _exit;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		switch (State)
		{
			case SpectatorState.ENTERING:
				if (Step(_seat))
				{
					_seat.spectator = this;
					State = SpectatorState.WATCHING;
				}
				break;
			case SpectatorState.WATCHING:
				Happiness = Math.Clamp(Happiness, (int)Mood.EXIT, (int)Mood.HAPPY);
				if (Happiness > (int)Mood.NEUTRAL && Mood != Mood.HAPPY)
				{
					ShowMood(Mood.HAPPY);
				}
				else if (Happiness > (int)Mood.ANNOYED && Mood != Mood.NEUTRAL)
				{
					ShowMood(Mood.NEUTRAL);
				}
				else if (Happiness > (int)Mood.EXIT && Mood != Mood.ANNOYED)
				{
					ShowMood(Mood.ANNOYED);
				}
				else
				{
					Mood = Mood.EXIT;
					State = SpectatorState.EXITING;
					_seat.spectator = null;
				}
				break;
			case SpectatorState.EXITING:
				if (Step(_exit))
				{
					QueueFree();
				}
				break;
		}
	}

	public void Initialize(Seat seat, Node2D exit)
	{
		_seat = seat;
		_exit = exit;
	}

	public int ApplyCard(ICardBasic card)
	{
		if (card.Influence.TryGetValue(Type, out int value))
		{
			Happiness += value;
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
	private bool Step(Node2D target)
	{
		Vector2 diff = target.Position - Position;
		Vector2 step = diff.Normalized() * _speed;

		if (diff.Length() > step.Length())
		{
			Position += step;
			return false;
		}
		else
		{
			Position = target.Position;
			return true;
		}
	}

	private void ShowMood(Mood mood)
	{
		switch (Mood = mood)
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
			case Mood.EXIT:
				_spriteAnnoyed.Visible = true;
				_spriteHappy.Visible = false;
				_spriteNeutral.Visible = false;
				break;
		}
	}
}

