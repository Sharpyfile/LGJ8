using Godot;
using Godot.Collections;
using System;

public partial class SeatsRow : Sprite2D
{
	[Export] public Array<Seat> Seats { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
