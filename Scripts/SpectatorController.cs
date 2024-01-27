using Godot;
using Godot.Collections;
using System;

public partial class SpectatorController : Node2D
{
	public Array<Spectator> GetSpectators() => _spectators;
	public Array<Spectator> GetSpectators(SpectatorState state)
	{
		var ret = new Array<Spectator>();
		foreach (var x in _spectators)
		{
			if (x.State == state) ret.Add(x);
		}
		return ret;
	}

	[Export] private double _speed = 5.0f;
	[Export] private double _spawnRate = 5.0f;

	private double _spawnTimer = 0;

	[Export] private Node2D _entrance;
	[Export] private Node2D _exit;
	[Export] private Array<SeatsRow> _rows;
	private Array<Seat> _seats = new();

	[Export] private Array<PackedScene> _pool;
	[Export] private Array<Spectator> _spectators;

	private RandomNumberGenerator _rng = new();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach (var row in _rows)
		{
			foreach (var seat in row.Seats)
			{
				_seats.Add(seat);
				seat.Row = row;
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_spawnTimer += delta;
		if (_spawnTimer > _spawnRate)
		{
			var seat = SelectSeat();
			if (seat != null)
			{
				var spectator = _pool[_rng.RandiRange(0, _pool.Count - 1)].Instantiate<Spectator>();
				seat.Row.AddChild(spectator);

				spectator.GlobalPosition = _entrance.GlobalPosition;
				spectator.Initialize(seat, _exit);
			}
			_spawnTimer -= _spawnRate;
		}
	}

	/**
	 * Return random seat or null, if all seats are occupied;
	 */
	private Seat SelectSeat()
	{
		int random = _rng.RandiRange(0, _seats.Count - 1);
		int index = random;


		while (_seats[index].Occupied)
		{
			if (index == random) return null; // loop completed
			index++;
			if (index >= _seats.Count) index = 0;
		}

		return _seats[index];
	}
}

