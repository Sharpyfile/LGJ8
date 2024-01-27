using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class Game : Node
{
	List<Card> allCards = new List<Card>();
	List<Card> cardsOnHand = new List<Card>();
	List<Spectator> spectators = new List<Spectator>();

	SpectatorController spectatorController = new SpectatorController();
	CardController cardController = new CardController();

	int audienceReaction = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	#region Initializing
	public void DrawFullHand()
	{
		for (int i = 0; i < 5; i++)
		{
			cardController.DrawCard();
		}
	}

	public void PopulateAudience()
	{
	}
	#endregion

	#region Card Operations
	public void DrawCard()
	{
	}

	public void RemoveCard()
	{
	}
	#endregion

	#region Audience Operations
	public void AddAudienceMember()
	{
	}

	public void RemoveAudienceMember()
	{
	}

	public void SwapAudienceMember()
	{
	}

	public void EvaluateAudienceMemberReaction(Spectator spectatorToEvaluate, Card cardToApply)
	{
		spectatorToEvaluate.ApplyCard(cardToApply);
	}

	public void EvaluateAudienceReaction(Card cardToApply)
	{
		foreach (Spectator spectator in spectators)
		{
			spectator.ApplyCard(cardToApply);
			//TODO:
			//calculate overall audience reaction
			//play sound according to overall audience reaction
		}
	}
	#endregion

	#region Main loop parts
	public void OpeningScene()
	{
		DrawFullHand();
		PopulateAudience();
		AudienceEnterAnimation();
	}

	public void EnterHandView()
	{
		HandEnterAnimation();
	}

	public void ExitHandView(Card cardToPlay)
	{
		HandExitAnimation();
		EvaluateAudienceReaction(cardToPlay);
		cardController.DiscardCard(cardToPlay);
		cardController.DrawCard();
	}

	public void EnterAudienceReactionView(Card cardToApply)
	{
		
	}

	public void ExitAudienceReactionView()
	{
		if (EvaluateGameEndCondition())
		{
			EnterGameEndView();
		}
		else
		{
			EnterHandView();
		}
	}

	public void EnterGameEndView()
	{
	}
	#endregion

	#region Animations
	public void HandEnterAnimation()
	{
	}

	public void HandExitAnimation()
	{
	}

	public void SceneOpeningAnimation()
	{
	}

	public void AudienceEnterAnimation()
	{
	}
	#endregion

	public bool EvaluateGameEndCondition()
	{
		bool gameEndCondition = false;

		return gameEndCondition;
	}
}
