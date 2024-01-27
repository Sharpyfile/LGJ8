using Godot;
using System;

public partial class MainUI : Control
{
	
	public void OnClickLoadMainMenu()
    {
		SceneManager.Instance.LoadMainMenuScene();
	}
}
