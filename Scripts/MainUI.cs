using Godot;
using System;

public partial class MainUI : Control
{
	[Export]
	public TimerWithSlider Timer;

	public void OnClickLoadMainMenu()
    {
		SceneManager.Instance.LoadMainMenuScene();
	}

	public void OnClickRestartTimer()
	{
		Timer.RestartTimer(10.0);
        Timer.OnTimerStop = () => Timer.TimerLabel.Text = "Timer ran out via mainUI, restart";
	}

	public void OnClickStopTimer()
	{
		Timer.StopTimer();
    }
}