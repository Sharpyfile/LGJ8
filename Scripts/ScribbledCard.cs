using System;
using Godot;

public partial class ScribbledCard : Node2D
{
	[Export]
	public Texture2D[] Textures = new Texture2D[Constants.HAND_SIZE];
	[Export]
	public Vector2[] BirdPositions = new Vector2[Constants.HAND_SIZE];
	[Export]
	public Vector2[] FishPositions = new Vector2[Constants.HAND_SIZE];
	[Export]
	public Vector2[] CatPositions = new Vector2[Constants.HAND_SIZE];

	public CardBasic CardData { get; private set; }

	private CardController _controller;

	public int index;
	public void Initialize(CardBasic cardData, int index, CardController cardController)
	{
		CardData = cardData;
		_controller = cardController;
		this.index = index;
		var button = GetChild<Button>(0);
		button.Icon = Textures[index];
		button.GetChild<Node2D>(0).Position = BirdPositions[index];
		button.GetChild<Node2D>(1).Position = FishPositions[index];
		button.GetChild<Node2D>(2).Position = CatPositions[index];
		var amountDict = cardData.Influence;
		amountDict.TryGetValue(Animal.BIRD, out int birdInfluence);
		amountDict.TryGetValue(Animal.FISH, out int fishInfluence);
		amountDict.TryGetValue(Animal.CAT, out int catInfluence);
		ManageDots(button.GetChild<Node2D>(0), birdInfluence);
		ManageDots(button.GetChild<Node2D>(1), fishInfluence);
		ManageDots(button.GetChild<Node2D>(2), catInfluence);
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

	private void ManageDots(Node2D node, int value)
	{
		node.Visible = true;
		node.GetChild<Sprite2D>(1).Visible = true;
		Color col = value < 0 ? Constants.BAD_COLOR : Constants.GOOD_COLOR;
		switch (Math.Abs(value))
		{
			case 2:
				node.GetChild<Sprite2D>(0).Modulate = col;
				node.GetChild<Sprite2D>(1).Modulate = col;
				break;
			case 1:
				node.GetChild<Sprite2D>(0).Modulate = col;
				node.GetChild<Sprite2D>(1).Visible = false;
				break;
			default:
				node.Visible = false;
				break;
		}
	}
}
