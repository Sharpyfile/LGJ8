using Godot;

public partial class Game : Node
{
	[Export]
	private MainUI _mainUI;

    private SpectatorController _spectatorController = new SpectatorController();
    private CardController _cardController = new CardController();

    private int _overallSpectatorsReaction = 0;
	private int _spectatorsReactionThreshold = 0;
	private bool _isEverythingInitiated = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_mainUI.Timer.OnTimerStop = () => RanOutOfTime();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_spectatorController.InitCompleted && !_isEverythingInitiated)
		{
			_isEverythingInitiated = true;
		}
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
		_mainUI.Timer.RestartTimer(_mainUI.Timer.TimerMaxValue);
	}

	public void PlayCard(Card cardToPlay)
	{
        _mainUI.Timer.StopTimer();

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

        _spectatorsReactionThreshold = _spectatorController.GetSpectators().Count * cardWeight / 3;

        foreach (Spectator spectator in _spectatorController.GetSpectators())
        {
            _overallSpectatorsReaction += spectator.ApplyCard(cardToPlay);
        }

        if (_overallSpectatorsReaction <= _spectatorsReactionThreshold*(-1))
		{
            //play angry crowd reaction
            AudioManager.Instance.PlaySound("crowdBoo1.wav");
        }
		else if (_overallSpectatorsReaction <= _spectatorsReactionThreshold)
		{
			//play neutral crowd reaction
			AudioManager.Instance.PlaySound("synthCricket.wav");
		}
		else
		{
			//play laughing crowd reaction
			AudioManager.Instance.PlaySound("crowdLaugh1.wav");
		}

		if (EvaluateGameEndCondition())
        {
            _mainUI.ShowGameOver();
        }
	}

	public void EnterGameEndView()
	{
	}

	public void RanOutOfTime()
	{
		//HandExitAnimation();
		foreach (Spectator spectator in _spectatorController.GetSpectators())
		{
			spectator.Annoy();
			EnterHandView();
		}
		if (EvaluateGameEndCondition())
		{
			_mainUI.ShowGameOver();
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
