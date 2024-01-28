using Godot;

public partial class Game : Node
{
	SpectatorController spectatorController = new SpectatorController();
	CardController cardController = new CardController();
	AudioManager audioManager = new AudioManager();
	TimerWithSlider timer = new TimerWithSlider();
	MainUI mainUI = new MainUI();

	int overallSpectatorsReaction = 0;
	int spectatorsReactionThreshold = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timer.OnTimerStop = () => RanOutOfTime();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	#region Initializing
	//public void DrawFullHand()
	//{
	//	for (int i = 0; i < 5; i++)
	//	{
	//		cardController.DrawCard();
	//	}
	//}

	//public void PopulateSpectators()
	//{
	//}
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

	//public void EvaluateSpectatorsReaction(Card cardToApply)
	//{
	//	foreach (Spectator spectator in spectatorController.GetSpectators())
	//	{
	//		overallSpectatorsReaction += spectator.ApplyCard(cardToApply);
	//	}
	//}
	#endregion

	#region Main loop parts
	//public void OpeningScene()
	//{
	//	audioManager.PlaySound("cardsMixing.mp3");
	//	//SpectatorsEnterAnimation();
	//}

	public void EnterHandView()
	{
		//HandEnterAnimation();
		timer.RestartTimer(timer.TimerMaxValue);
	}

	public void PlayCard(Card cardToPlay)
	{
		timer.StopTimer();

		int cardWeight = 0;

		if (cardToPlay.Influence[Animal.CAT] > 0)
		{
			cardWeight = cardToPlay.Influence[Animal.CAT];
		}
		else if (cardToPlay.Influence[Animal.BIRD] > 0)
		{
			cardWeight = cardToPlay.Influence[Animal.BIRD];
		}
		else
		{
			cardWeight = cardToPlay.Influence[Animal.FISH];
		}

		spectatorsReactionThreshold = spectatorController.GetSpectators().Count * cardWeight / 3;

		foreach (Spectator spectator in spectatorController.GetSpectators())
		{
			overallSpectatorsReaction += spectator.ApplyCard(cardToPlay);
		}

		if (overallSpectatorsReaction <= spectatorsReactionThreshold * (-1))
		{
			//play angry crowd reaction
			audioManager.PlaySound("crowdBoo1.wav");
		}
		else if (overallSpectatorsReaction <= spectatorsReactionThreshold)
		{
			//play neutral crowd reaction
			audioManager.PlaySound("synthCricket.wav");
		}
		else
		{
			//play laughing crowd reaction
			audioManager.PlaySound("crowdLaugh1.wav");
		}

		if (EvaluateGameEndCondition())
		{
			mainUI.ShowGameOver();
		}
	}

	public void EnterGameEndView()
	{
	}

	public void RanOutOfTime()
	{
		//HandExitAnimation();
		foreach (Spectator spectator in spectatorController.GetSpectators())
		{
			spectator.Annoy();
			EnterHandView();
		}
		if (EvaluateGameEndCondition())
		{
			mainUI.ShowGameOver();
		}
	}
	#endregion

	#region Animations
	//public void HandEnterAnimation()
	//{

	//}

	//public void HandExitAnimation()
	//{
	//}

	//public void CardHighlightAnimation (Card card)
	//{

	//}

	//public void SceneOpeningAnimation()
	//{
	//}

	//public void SpectatorsEnterAnimation()
	//{
	//}
	#endregion

	public bool EvaluateGameEndCondition()
	{
		bool gameEndCondition = false;

		//TODO:
		//decide on when game ends and code it

		return gameEndCondition;
	}
}
