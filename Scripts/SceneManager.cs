using Godot;
using System;

public partial class SceneManager : Node
{
	[Export]
	private PackedScene MainMenuScene;

	[Export]
	private PackedScene MainGameplayScene;

	[Export]
	private PackedScene GameOverScene;

	[Export]
	private PackedScene GameWinScene;

	public static SceneManager Instance { get; private set; }

	public override void _Ready()
	{
		Instance = this;
	}

	public void LoadMainMenuScene()
	{
		GetTree().ChangeSceneToPacked(MainMenuScene);
	}

	public void LoadMainGameplayScene()
	{
		GetTree().ChangeSceneToPacked(MainGameplayScene);
	}

    public void LoadGameOverScene()
    {
        GetTree().ChangeSceneToPacked(GameOverScene);
    }

    public void LoadGameWinScene()
    {
        GetTree().ChangeSceneToPacked(GameWinScene);
    }

    public void ExitGame()
	{
		GetTree().Quit();
	}

}
