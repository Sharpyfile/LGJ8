using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class Game : Node
{
	List<Spectator> spectators = new List<Spectator>();

	SpectatorController spectatorController = new SpectatorController();
	CardController cardController = new CardController();
	AudioManager audioManager = new AudioManager();

	int overallSpectatorsReaction = 0;

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

	public void PopulateSpectators()
	{
	}
	#endregion

	#region Card Operations
	//public void DrawCard()
	//{
	//}

	//public void RemoveCard()
	//{
	//}
	#endregion

	#region Spectators Operations
	//public void AddSpectator()
	//{
	//}

	//public void RemoveSpectator()
	//{
	//}

	//public void SwapSpectator()
	//{
	//}

	public void EvaluateSpectatorReaction(Spectator spectatorToEvaluate, Card cardToApply)
	{
		spectatorToEvaluate.ApplyCard(cardToApply);
	}

	public void EvaluateSpectatorsReaction(Card cardToApply)
	{
		foreach (Spectator spectator in spectators)
		{
			spectator.ApplyCard(cardToApply);
			//TODO:
			//calculate overall spectators reaction
			
		}
	}
	#endregion

	#region Main loop parts
	public void OpeningScene()
	{
		DrawFullHand();
		audioManager.PlaySound("cardsMixing.mp3");
		PopulateSpectators();
		SpectatorsEnterAnimation();
	}

	public void EnterHandView()
	{
		HandEnterAnimation();
	}

	public void ExitHandView(Card cardToPlay)
	{
		HandExitAnimation();
		EvaluateSpectatorsReaction(cardToPlay);
		cardController.DiscardCard(cardToPlay);
		cardController.DrawCard();

		switch (overallSpectatorsReaction)
		{
			case >= 4:
				//play laughing crowd reaction
				audioManager.PlaySound("crowdLaugh1.wav");
				break;
			case < 4:
				//play neutral crowd reaction
				audioManager.PlaySound("synthCricket.wav");
				break;
			default:
				//play angry crowd reaction
				audioManager.PlaySound("crowdBoo1.wav");
				break;
		}

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

	public void SpectatorsEnterAnimation()
	{
	}
	#endregion

	public bool EvaluateGameEndCondition()
	{
		bool gameEndCondition = false;

		//TODO:
		//decide on and code when game ends

		return gameEndCondition;
	}
}
