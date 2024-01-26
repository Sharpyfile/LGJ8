using Godot;
using System;

public partial class MainMenuUI : Node
{	
	public override void _Ready()
	{

	}


	public void OnClickExitGame()
	{
		SceneManager.Instance.ExitGame();
	}

	public void OnClickLoadMainMenu()
	{
		SceneManager.Instance.LoadMainMenuScene();
	}

	public void OnClickLoadMainGameplayScene()
	{
		SceneManager.Instance.LoadMainGameplayScene();
	}

	public void OnClickPlayTypewritter()
	{
		AudioManager.Instance.PlaySound("typewriterEndStroke");
	}
}
