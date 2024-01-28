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


	private bool _hideHand = false;
	private bool _showHand = false;

	[Export]
	private double _showHideCooldown = 1.5;
	private double _showHideTimer = 0;

	[Export]
	private Control _handTarget;
	private Vector2 _initialHandPosition;


	public override void _Ready()
	{
		TransitionAnimator.Play("SceneTransitionIn");
		_initialHandPosition = _hand.Position;

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
			}
            AudioPlayer = AudioManager.Instance.GetAudioPlayer("Plain Loafer", 2.0f);
        }

		if (_showHand)
			ShowHandAnimation(delta);
		else if (_hideHand)
			HideHandAnimation(delta);
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

	private void ShowHandAnimation(double delta)
	{
        if (_showHideTimer < _showHideCooldown)
        {
            _showHideTimer += delta;
            double weight = _showHideTimer / _showHideCooldown;
            _hand.Position = _hand.Position.Lerp(_initialHandPosition, (float)weight);

            if (_showHideTimer >= _showHideCooldown)
            {
                _showHand = false;
                _hand.Position = _initialHandPosition;
            }
        }
    }

	private void HideHandAnimation(double delta)
	{
		if (_showHideTimer < _showHideCooldown)
		{
			_showHideTimer += delta;
			double weight = _showHideTimer / _showHideCooldown;
			_hand.Position = _hand.Position.Lerp(_handTarget.Position, (float)weight);

			if (_showHideTimer >= _showHideCooldown)
			{
				_hideHand = false;
				_hand.Position = _handTarget.Position;
            }
		}
	}

	/**
	* Sets hand visibility and timer activity
	*/
	public void ShowHand(bool value)
	{
		// _hand.Visible = value; // TODO animation
		_showHideTimer = 0;

        if (value)
		{
			_showHand = true;
			Timer.RestartTimer(Timer.TimerMaxValue);
		}
		else
		{
			_hideHand = true;
			Timer.StopTimer();
		}

    }
}
