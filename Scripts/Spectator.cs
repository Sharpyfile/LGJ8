using Godot;
using System;

public partial class Spectator : Node2D
{
	public enum State
	{
		ENTERING,
		WATCHING,
		EXITING
	}


	[Export] private Animal _type;
	[Export] private State _state;
	[Export] private float _speed = 5;
	[Export] public int Annoyence { get; private set; } = 2;
	[Export] public int AnnoyLimit { get; private set; } = 5;

	private Seat _seat;
	private Node2D _exit;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		switch (_state)
		{
			case State.ENTERING:
				if (Step(_seat))
				{
					_seat.spectator = this;
					_state = State.WATCHING;
				}
				break;
			case State.WATCHING: 
				if (Annoyence > AnnoyLimit)
				{
					_seat.spectator = null;
					_state = State.EXITING;
				}
				break;
			case State.EXITING:
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

	public void ApplyCard(ICardBasic card)
	{
		if (card.Influence.TryGetValue(_type, out int value))
		{
			Annoyence -= value;
		}
	}

	public void Annoy()
	{
        Annoyence++;
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
}

