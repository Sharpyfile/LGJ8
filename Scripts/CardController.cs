using System;
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
	public MainUI MainUI { get; set; }

	[Export]
	public Node2D[] UICardNodes { get; set; } = new Node2D[Constants.HAND_SIZE];

	[Export]
	public Game GameController { get; private set; }

	[Export]
	private double _afterCardPlaySleep = 1.0;

	public bool IsReadyToPlay
	{
		get => _sleepTimer < 0;
	}

	private Node2D[] ScribbledCardNodes { get; set; } = new Node2D[Constants.HAND_SIZE];
	private List<CardBasic> AvailableCards = new();
	private List<CardBasic> Deck = new();
	private CardBasic[] Hand = new CardBasic[Constants.HAND_SIZE];
	private RandomNumberGenerator rng = new();
	private double counter = 0.0;

	private Card clickedCard;
	private int clickedCardIndex = -1;
	private bool isEnabled;
	private double _sleepTimer;



	#region Card animation properties

	[Export]
	public double CardSlideTime = 1.0;

	#endregion

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
		ScribbledCardNodes = MainUI.ScribbledCardNodes;
		InitializeHand();

		MainUI.ShowHand(false);
		_sleepTimer = _afterCardPlaySleep;
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_sleepTimer -= delta;

		if (clickedCard != null && clickedCard.ReadyToReinitialize && clickedCardIndex != -1)
			ReinitializeClickedCard();
	}

	private void ReinitializeClickedCard()
	{
		clickedCard = null;
		DrawCard(clickedCardIndex);
		clickedCardIndex = -1;
	}

	public void InitializeHand()
	{
		for (int i = 0; i < Constants.HAND_SIZE; ++i)
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
		if (index < Constants.HAND_SIZE)
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
			UICardNodes[index].GetChild<Card>(0).Initialize(drewCard, index, this, index * 20, HoveredCardNode.Position, CardSlideTime);
		}
	}

	public void PlayCard(int index)
	{
		GameController.PlayCard(Hand[index]);
		UpdateCardsState(index, CardState.NOT_CLICKED);

		clickedCardIndex = index;
		//TODO: hide cardn
	}

	public void UpdateCardsState(int index, CardState state)
	{
		Card updatedCard = UICardNodes[index].GetChild<Card>(0);

		switch (state)
		{
			case CardState.CLICKED:
				{
					if (clickedCard != updatedCard && clickedCard != null)
						clickedCard.RunAnimation(CardAnimationState.SLIDE_OUT);

					clickedCard = updatedCard;
					break;
				}
			case CardState.NOT_CLICKED:
				{
					clickedCard.RunAnimation(CardAnimationState.SLIDE_OUT, true);
					break;
				}
			case CardState.HOVERED:
				{
					if (clickedCard != updatedCard && clickedCard != null)
						clickedCard.RunAnimation(CardAnimationState.SLIDE_OUT);
					else if (clickedCard == updatedCard && clickedCard.SetReadyToReinitialize)
						break;

					updatedCard.RunAnimation(CardAnimationState.SlIDE_IN);
					break;
				}
			case CardState.NOT_HOVERED:
				{
					if (clickedCard != updatedCard)
					{
						updatedCard.RunAnimation(CardAnimationState.SLIDE_OUT);
						if (clickedCard != null)
							clickedCard.RunAnimation(CardAnimationState.SlIDE_IN);
					}

					break;
				}

		}
	}

	public void OnTimeout()
	{
		foreach (CardBasic card in Hand)
			AvailableCards.Add(card);

		for (int i = 0; i < Constants.HAND_SIZE; ++i)
			DrawCard(i);
	}

	public void Enable()
	{
		MainUI.ShowHand(true);
		isEnabled = true;
	}

	private void Disable()
	{
		MainUI.ShowHand(false);
		isEnabled = false;
		_sleepTimer = _afterCardPlaySleep;
	}
}
