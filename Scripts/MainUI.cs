using Godot;
using System;

public partial class MainUI : Control
{
	[Export]
	public TimerWithSlider Timer;

	[Export]
	private AnimationPlayer TransitionAnimator;

	[Export]
	private SpectatorController _spectatorController;

	private AudioPlayer AudioPlayer;

	public override void _Ready()
	{
		TransitionAnimator.Play("SceneTransitionIn");
		base._Ready();
	}

	public void OnClickStartAnimation()
	{
		TransitionAnimator.Play("SceneTransitionOut");
		if (AudioPlayer != null)
			AudioPlayer.StopMusic(1.5f);
	}

	public void OnFinishLoadMainGameplayScene(StringName animationName)
	{
		if (animationName == "SceneTransitionOut")
			SceneManager.Instance.LoadMainMenuScene();
		else if (animationName == "SceneTransitionIn")
			AudioPlayer = AudioManager.Instance.GetAudioPlayer("Plain Loafer");
	}

	public void OnClickRestartTimer()
	{
		Timer.RestartTimer(10.0);
		Timer.OnTimerStop = () => OnTimerStopBoo();
		// TODO temporary
		foreach (var spectator in _spectatorController.GetSpectators())
		{
			spectator.Annoy();
			spectator.Annoy();
			spectator.Annoy();
			spectator.Annoy();
			spectator.Annoy();
			spectator.Annoy();
			spectator.Annoy();
			spectator.Annoy();
			spectator.Annoy();
			spectator.Annoy();
			spectator.Annoy();
			spectator.Annoy();
			spectator.Annoy();
			spectator.Annoy();
			spectator.Annoy();
			spectator.Annoy();
			spectator.Annoy();
			spectator.Annoy();
			spectator.Annoy();
			spectator.Annoy();
			spectator.Annoy();
			spectator.Annoy();
			spectator.Annoy();
			spectator.Annoy();
			spectator.Annoy();
		}
	}

	private void OnTimerStopBoo()
	{
		Timer.TimerLabel.Text = "Timer ran out via mainUI, restart";
		AudioManager.Instance.PlaySound("crowdBoo1");
	}

	public void OnClickStopTimer()
	{
		Timer.StopTimer();
	}
}
