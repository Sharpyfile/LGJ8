using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class CardController : Node
{
	[Export]
	public string FileName { get; private set; } = "res://cards.csv.txt";

	[Export]
	public PackedScene CardPrefab { get; set; }

	[Export]
	public Node2D PositionNode { get; set; }

	[Export]
	public Array<Card> Hand { get; set; }

	private List<CardBasic> CardsToDraw = new();
	private List<CardBasic> Deck = new();
	private RandomNumberGenerator rng = new();
	private double counter = 0.0;
	private readonly int HAND_SIZE = 5;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		PositionNode = GetChild<Node2D>(0);
		Deck = new();
		var file = FileAccess.Open(FileName, FileAccess.ModeFlags.Read);
		var data = file.GetAsText();
		string[] lines = data.Split('\n');
		for (int i = 1; i < lines.Length; ++i)
		{
			if (lines[i].Length > 0)
			{
				string[] columns = lines[i].Split(";");
				CardBasic card = new();
				card.Text = columns[0];
				card.Influence = new Godot.Collections.Dictionary<Animal, int>();
				card.Influence.Add(Animal.CAT, int.Parse(columns[1]));
				card.Influence.Add(Animal.FISH, int.Parse(columns[2]));
				card.Influence.Add(Animal.BIRD, int.Parse(columns[3]));
				Deck.Add(card);
			}
		}
		CardsToDraw = new List<CardBasic>(Deck);
	}

	public void DiscardCard(Card cardObject)
	{
		//TODO: Send to audience
		Hand.Remove(cardObject);
		cardObject.QueueFree();
	}

	public void DrawCard()
	{
		if (Hand.Count < HAND_SIZE)
		{
			if (CardsToDraw.Count == 0)
			{
				CardsToDraw = new List<CardBasic>(Deck);
			}
			int index = rng.RandiRange(0, CardsToDraw.Count);
			Card cardObject = CardPrefab.Instantiate<Card>();
			cardObject.Initialize(CardsToDraw[index], this);
			CardsToDraw.RemoveAt(index);
			PositionNode.AddChild(cardObject);
			Hand.Add(cardObject);
		}
	}
}
