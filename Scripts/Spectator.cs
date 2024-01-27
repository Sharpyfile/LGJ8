using Godot;
using System;

public partial class Spectator : Node2D
{


	[Export] public Animal Type { get; private set; }
	[Export] public SpectatorState State { get; private set; }
	[Export] public int Annoyance { get; private set; } = 2;
	[Export] public int AnnoyLimit { get; private set; } = 5;

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
				if (Annoyance > AnnoyLimit)
				{
					_seat.spectator = null;
					State = SpectatorState.EXITING;
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

	public void ApplyCard(ICardBasic card)
	{
		if (card.Influence.TryGetValue(Type, out int value))
		{
			Annoyance -= value;
		}
	}

	public void Annoy()
	{
		Annoyance++;
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

