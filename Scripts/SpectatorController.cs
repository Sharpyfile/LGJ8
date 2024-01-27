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

	[Export] private Vector2I _audienceOnStart = new Vector2I(4, 7);
	[Export] private double _spawnRate = .3f;

	private double _spawnTimer = 0;

	[Export] public Node2D Entrance { get; private set; }
	[Export] public Node2D Exit { get; private set; }
	[Export] private Array<SeatsRow> _rows;
	private Array<Seat> _seats = new();

	[Export] private Array<PackedScene> _pool;
	[Export] private Array<Spectator> _spectators;
	[Export] public bool _isAudienceFull { get; private set; }

	private RandomNumberGenerator _rng = new();
	private float _midX;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_midX = (Entrance.GlobalPosition.X + Exit.GlobalPosition.X) / 2.0f;
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
		// Fill audence on init 
		if (!_isAudienceFull)
		{
			_spawnTimer += delta;
			if (_spawnTimer > _spawnRate)
			{
				_isAudienceFull = !SpawnSpectator();
				_spawnTimer -= _spawnRate;
			}
		}
	}

	public void Remove(Spectator spectator)
	{
		_spectators.Remove(spectator);
		spectator.QueueFree();
	}

	private bool SpawnSpectator()
	{
		var seat = SelectSeat();
		if (seat != null)
		{
			var spectator = _pool[_rng.RandiRange(0, _pool.Count - 1)].Instantiate<Spectator>();
			_spectators.Add(spectator);
			seat.Row.AddChild(spectator);

			var entrance = seat.GlobalPosition.X < _midX ? Entrance : Exit;
			spectator.GlobalPosition = entrance.GlobalPosition;
			spectator.Initialize(this, entrance.GlobalPosition, seat);
			return true;
		}
		return false;
	}

	/**
	 * Return random seat or null, if all seats are occupied;
	 */
	private Seat SelectSeat()
	{
		int random = _rng.RandiRange(0, _seats.Count - 1);
		for (int offset = 0; offset < _seats.Count; offset++)
		{
			var seat = _seats[(random + offset) % _seats.Count];
			if (!seat.Occupied) return seat;
		}
		return null;
	}
}

