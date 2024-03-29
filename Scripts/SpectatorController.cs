using Godot;
using Godot.Collections;
using System;

public partial class SpectatorController : Node
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

	[Export] private double _spawnRate = .3f;
	[Export] private double _spawnCap = 0;

	private double _spawnTimer = 0;

	[Export] private Node2D _spectatorsParent;
	[Export] public Node2D Entrance { get; private set; }
	[Export] public Node2D Exit { get; private set; }
	[Export] private Array<SeatsRow> _rows;
	private Array<Seat> _seats = new();

	[Export] private Array<PackedScene> _pool;
	[Export] private Array<Spectator> _spectators;
	[Export] public bool IsAudienceFull { get; private set; }
	[Export] public bool InitCompleted { get; private set; }

	private RandomNumberGenerator _rng = new();
	private float _midX;

	private int _eliminator;
	private int _eliminatorIndex;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_midX = (Entrance.GlobalPosition.X + Exit.GlobalPosition.X) / 2.0f;
		foreach (var row in _rows)
		{
			foreach (var seat in row.Seats)
			{
				_spawnCap++;
				_seats.Add(seat);
				seat.Row = row;
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!IsAudienceFull)
		{
			_spawnTimer += delta;
			if (_spawnTimer > _spawnRate)
			{
				IsAudienceFull = !SpawnSpectator();
				_spawnTimer -= _spawnRate;
			}
		}
		else if (!InitCompleted)
		{
			bool completed = true;
			foreach (var spectator in _spectators)
			{
				if (spectator.State == SpectatorState.ENTERING)
				{
					completed = false;
					break;
				}
			}
			InitCompleted = completed;
		}
	}

	public void Release(Spectator spectator)
	{
		_spectators.Remove(spectator);
		if (_eliminatorIndex == 0) _eliminator = _rng.RandiRange(0, 2);
		if (_eliminatorIndex == _eliminator) _spawnCap--;
		_eliminatorIndex = ++_eliminatorIndex % 3;
		IsAudienceFull = false;
		InitCompleted = false;
	}

	private bool SpawnSpectator()
	{
		if (_spectators.Count >= _spawnCap) return false;
		var seat = SelectSeat();
		if (seat != null)
		{
			var spectator = _pool[_rng.RandiRange(0, _pool.Count - 1)].Instantiate<Spectator>();
			_spectators.Add(spectator);
			_spectatorsParent.AddChild(spectator);

			var entrance = seat.GlobalPosition.X < _midX ? Entrance : Exit;
			spectator.GlobalPosition = entrance.GlobalPosition;
			spectator.Initialize(this, entrance.GlobalPosition, seat, _rng.RandiRange(-1, 1));
			return true;
		}
		return false;
	}

	public void IncreaseCap()
	{
		if (_spawnCap < _seats.Count)
		{
			IsAudienceFull = false;
			InitCompleted = false;
			_spawnCap++;
		}
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

