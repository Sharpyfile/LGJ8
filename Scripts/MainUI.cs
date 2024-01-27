using Godot;
using System;

public partial class MainUI : Control
{
	[Export]
	public TimerWithSlider Timer;

    [Export]
    private AnimationPlayer TransitionAnimator;

    public override void _Ready()
    {
        TransitionAnimator.Play("SceneTransitionIn");
        base._Ready();
    }

    public void OnClickStartAnimation()
    {
        TransitionAnimator.Play("SceneTransitionOut");
    }

    public void OnFinishLoadMainGameplayScene(StringName animationName)
    {
        if (animationName == "SceneTransitionOut")
            SceneManager.Instance.LoadMainMenuScene();
    }

    public void OnClickRestartTimer()
	{
		Timer.RestartTimer(10.0);
		Timer.OnTimerStop = () => OnTimerStopBoo();

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