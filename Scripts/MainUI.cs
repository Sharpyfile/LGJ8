using Godot;
using System;
using System.Reflection.Metadata;

public partial class MainUI : Control
{
	[Export]
	public TimerWithSlider Timer;

	[Export]
	public Node2D[] ScribbledCardNodes { get; set; } = new Node2D[Constants.HAND_SIZE];

	[Export]
	private AnimationPlayer TransitionAnimator;

	[Export]
	private SpectatorController _spectatorController;

	[Export]
	private Control _hand;

	private AudioPlayer AudioPlayer;

	private bool _isFullInitial = false;

	private bool GameOver = false;


	public override void _Ready()
	{
		TransitionAnimator.Play("SceneTransitionIn");
		base._Ready();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		if (_spectatorController.InitCompleted && !_isFullInitial)
		{
			_isFullInitial = true;
			if (AudioManager.Instance.AmbientPlayer != null)
			{
				AudioManager.Instance.AmbientPlayer.StopMusic(1.5f);
				AudioPlayer = AudioManager.Instance.GetAudioPlayer("Plain Loafer", 2.0f);
			}
		}
	}

	public void OnClickStartAnimation()
	{
		TransitionAnimator.Play("SceneTransitionOut");
		if (AudioPlayer != null)
			AudioPlayer.StopMusic(1.5f);
	}

	public void ShowGameOver()
	{
		GameOver = true;
		OnClickStartAnimation();
	}

	public void OnFinishLoadMainGameplayScene(StringName animationName)
	{
		if (animationName == "SceneTransitionOut")
		{
			if (GameOver)
				SceneManager.Instance.LoadGameOverScene();
			else
				SceneManager.Instance.LoadMainMenuScene();
		}
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
		Timer.TimerLabel.Text = "Time's up!";
		AudioManager.Instance.PlaySound("crowdBoo1");
	}

	public void OnClickStopTimer()
	{
		Timer.StopTimer();
	}

	/**
	* Sets hand visibility and timer activity
	*/
	public void ShowHand(bool value)
	{
		_hand.Visible = value; // TODO animation
		if (value)
		{
			Timer.RestartTimer(Timer.TimerMaxValue);
		}
		else
		{
			Timer.StopTimer();
		}
	}
}
