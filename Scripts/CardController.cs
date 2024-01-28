using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class CardController : Node
{
	[Export]
	public string FileName { get; private set; } = "res://cards.csv.txt";

	[Export]
	public PackedScene ScribbledCardPrefab { get; set; }

	[Export]
	public PackedScene CardPrefab { get; set; }

	[Export]
	public Node2D ClickedCardNode { get; set; }

	[Export]
	public Node2D HoveredCardNode { get; set; }

	[Export]
	public Node2D[] ScribbledCardNodes { get; set; } = new Node2D[HAND_SIZE];

	[Export]
	public Node2D[] UICardNodes { get; set; } = new Node2D[HAND_SIZE];

	[Export]
	public Game GameController { get; private set; }

	private List<CardBasic> AvailableCards = new();
	private List<CardBasic> Deck = new();
	private CardBasic[] Hand = new CardBasic[HAND_SIZE];
	private RandomNumberGenerator rng = new();
	private double counter = 0.0;
	private static readonly int HAND_SIZE = 5;

	private Card clickedCard;
	private Card hoveredCard;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
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
				card.Question = columns[0];
				card.Riposte = columns[1];
				card.Influence = new Godot.Collections.Dictionary<Animal, int>();
				card.Influence.Add(Animal.CAT, int.Parse(columns[2]));
				card.Influence.Add(Animal.FISH, int.Parse(columns[3]));
				card.Influence.Add(Animal.BIRD, int.Parse(columns[4]));
				Deck.Add(card);
			}
		}
		AvailableCards = new List<CardBasic>(Deck);
		InitializeHand();
	}

	public void InitializeHand()
	{
		for (int i = 0; i < HAND_SIZE; ++i)
		{
			ScribbledCard scribbledCard = ScribbledCardPrefab.Instantiate<ScribbledCard>();
			ScribbledCardNodes[i].AddChild(scribbledCard);
			Card card = CardPrefab.Instantiate<Card>();
			UICardNodes[i].AddChild(card);
			DrawCard(i);
		}
	}

	public void DrawCard(int index)
	{
		if (index < HAND_SIZE)
		{
			if (AvailableCards.Count == 0)
			{
				AvailableCards = new List<CardBasic>(Deck);
			}
			int rngIndex = rng.RandiRange(0, AvailableCards.Count - 1);
			CardBasic drewCard = AvailableCards[rngIndex];
			Hand[index] = drewCard;
			AvailableCards.RemoveAt(rngIndex);
			ScribbledCardNodes[index].GetChild<ScribbledCard>(0).Initialize(drewCard, index, this);
			UICardNodes[index].GetChild<Card>(0).Initialize(drewCard, index, this);
		}
	}

	public void PlayCard(int index)
	{

		GameController.PlayCard(Hand[index]);
		DrawCard(index);
		//TODO: hide card
	}

	public void UpdateCardsState(int index, CardState state)
	{

		switch (state)
		{
			case CardState.CLICKED:
				{
					if (ClickedCardNode.GetChildCount() != 0)
					{
						Card previousCard = ClickedCardNode.GetChild<Card>(0);
						if (UICardNodes[previousCard.index].GetChildCount() != 0)
						{
							throw new System.Exception("There is card in node " + previousCard.index.ToString());
						}
						ClickedCardNode.RemoveChild(previousCard);
						//TODO: animation
						UICardNodes[previousCard.index].AddChild(previousCard);
					}

					if (HoveredCardNode.GetChildCount() == 0)
					{
						throw new System.Exception("There should be a card in hovered node");
					}
					Card clickedCard = HoveredCardNode.GetChild<Card>(0);
					HoveredCardNode.RemoveChild(clickedCard);
					//TODO: animation
					ClickedCardNode.AddChild(clickedCard);
					break;
				}

			case CardState.HOVERED:
				{
					if (HoveredCardNode.GetChildCount() != 0)
					{
						throw new System.Exception("There should be no card in hovered node");
					}
					if (UICardNodes[index].GetChildCount() == 0)
					{
						throw new System.Exception("There is no card in node " + index.ToString());
					}
					Card hoveredCard = UICardNodes[index].GetChild<Card>(0);
					UICardNodes[index].RemoveChild(hoveredCard);
					//TODO: animation
					HoveredCardNode.AddChild(hoveredCard);
					break;
				}
			case CardState.NOT_HOVERED:
				{
					if (HoveredCardNode.GetChildCount() == 0)
					{
						throw new System.Exception("There should be a card in hovered node");
					}
					Card previousCard = HoveredCardNode.GetChild<Card>(0);
					if (UICardNodes[previousCard.index].GetChildCount() != 0)
					{
						throw new System.Exception("There is card in node " + previousCard.index.ToString());
					}
					HoveredCardNode.RemoveChild(previousCard);
					//TODO: animation
					UICardNodes[previousCard.index].AddChild(previousCard);
					break;
				}
		}
	}

	public void OnTimeout()
	{
		foreach (CardBasic card in Hand)
			AvailableCards.Add(card);

		for (int i = 0; i < HAND_SIZE; ++i)
			DrawCard(i);
	}
}
