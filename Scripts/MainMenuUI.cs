using Godot;
using System;
using System.Diagnostics;

public partial class MainMenuUI : Node
{
	[Export]
	private Control OptionMenu;
	[Export]
	private Control Credits;

	private AudioPlayer AmbientPlayer;

	[Export]
	private AnimationPlayer TransitionAnimator;


    public override void _Ready()
    {
        base._Ready();
		OptionMenu.Visible = false;
		Credits.Visible = false;

		if (AudioManager.Instance != null)
		{
			AmbientPlayer = AudioManager.Instance.GetAudioPlayer("cafeAmbience");

        }
    }

    public void OnClickExitGame()
	{
		SceneManager.Instance.ExitGame();
	}


	public void OnClickStartAnimation()
	{
		TransitionAnimator.Play("SceneTransitionOut");
        if (AmbientPlayer != null)		
			AmbientPlayer.StopMusic(2.5f);
    }

	public void OnFinishLoadMainGameplayScene(StringName animationName)
	{
		if (animationName == "SceneTransitionOut")
			SceneManager.Instance.LoadMainGameplayScene();
    }

	public void OnClickOpenHideOptions()
	{
		OptionMenu.Visible = !OptionMenu.Visible;
		if (OptionMenu.Visible)
			Credits.Visible = false;
    }

    public void OnClickOpenHideCredits()
    {
        Credits.Visible = !Credits.Visible;

        if (Credits.Visible)
            OptionMenu.Visible = false;
    }

    public void OnClickPlayTypewritter()
	{
		AudioManager.Instance.PlaySound("synthCricket");
	}	

}
