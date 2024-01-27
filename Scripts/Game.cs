using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class Game : Node
{
	List<T> allCards = new List<T>();
	List<T> cardsOnHand = new List<T>();
	List<T> audience = new List<T>();

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

	public void EvaluateAudienceMemberReaction()
	{
	}

	public void EvaluateAudienceReaction()
	{
		foreach (var audienceMember in audience)
		{
			EvaluateAudienceMemberReaction();
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

	public void ExitHandView()
	{
		HandExitAnimation();
		RemoveCard();
		DrawCard();
		EnterAudienceReactionView();
	}

	public void EnterAudienceReactionView()
	{
        EvaluateAudienceReaction();
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
