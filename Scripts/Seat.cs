using Godot;
using System;

public partial class Seat : Node2D
{

	public Spectator spectator;
	public bool Occupied { get => spectator != null; }
	public SeatsRow Row; // seted by controller

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}
