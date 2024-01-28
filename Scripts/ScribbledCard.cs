using Godot;
using System;

public partial class ScribbledCard : Node2D
{
	public CardBasic cardData { get; private set; }

	private CardController _controller;

	public int index;
	public void Initialize(CardBasic cardData, int index, CardController cardController)
	{
		this.cardData = cardData;
		_controller = cardController;
		this.index = index;
	}

	public void OnHover()
	{
		_controller.UpdateCardsState(index, CardState.HOVERED);
	}

	public void OnUnHover()
	{
		_controller.UpdateCardsState(index, CardState.NOT_HOVERED);
	}

	public void OnClick()
	{
		_controller.UpdateCardsState(index, CardState.CLICKED);
	}
}
