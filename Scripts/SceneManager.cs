using Godot;
using System;

public partial class SceneManager : Node
{
	[Export]
	private PackedScene MainMenuScene;

	[Export]
	private PackedScene MainGameplayScene;

	public static SceneManager Instance { get; private set; }

	public override void _Ready()
	{
	}

	public void LoadMainMenuScene()
	{
		GetTree().ChangeSceneToPacked(MainMenuScene);
	}

	public void LoadMainGameplayScene()
	{
		GetTree().ChangeSceneToPacked(MainGameplayScene);
	}

	public void ExitGame()
	{
		GetTree().Quit();
	}

}
