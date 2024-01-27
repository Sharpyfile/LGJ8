using Godot;
using Godot.Collections;
using System;

public partial class SpectatorController : Node2D
{

	[Export] private double _speed = 5.0f;
	[Export] private double _spawnRate = 5.0f;

	private double _timer = 0;

	[Export] private Node2D _entrace;
	[Export] private Node2D _exit;
	[Export] private Node _seatsParent;
	private Array<Node2D> _seats = new();

	[Export] private Node _spectatorsParent;
	[Export] private PackedScene _spectatorScene;
	private Array<Node2D> _spectators = new();

	private int _tmp_counter = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach (var child in _seatsParent.GetChildren())
		{
			if (child is Node2D) _seats.Add(child as Node2D);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_timer += delta;

		if (_timer > _spawnRate)
		{
			Spectator newSpectator = _spectatorScene.Instantiate<Spectator>();

			_spectators.Add(newSpectator);
			_spectatorsParent.AddChild(newSpectator);

			newSpectator.Position = _entrace.Position;
			newSpectator.Initialize(_seats[_tmp_counter++], _exit);

			if (_tmp_counter >= _seats.Count) _tmp_counter = 0;
			_timer -= _spawnRate;
		}
	}
}
