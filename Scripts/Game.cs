using Godot;
using System;
using System.Collections.Generic;

public partial class Game : Node
{
	List<T> allCards = new List<T>();
	List<T> cardsOnHand = new List<T>();
	List<T> audience = new List<T>();

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
		}
	}
    #endregion

    #region View change
    public void EnterHandView()
	{
	}

	public void EnterAudienceReactionView()
	{
	}

	public void EnterGameEndView()
	{
	}
    #endregion

    public void EvaluateGameEndCondition()
	{
		bool gameEndCondition = false;

		if (gameEndCondition)
		{
			EnterGameEndView();
		}
		else
		{
			EnterHandView();
		}
	}
}
