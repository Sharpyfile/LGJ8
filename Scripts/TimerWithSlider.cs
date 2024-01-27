using Godot;
using System;

public partial class TimerWithSlider : Control
{
	[Export]
	private ColorRect SliderInside;

	[Export]
	public Label TimerLabel;

	[Export]
	public double TimerMaxValue = 30.0;

	[Export]
	private Color BadColor;

	[Export]
	private Color GoodColor;

	private double timerCurrentValue;
	private bool playedCough = false;

	public Action OnTimerStop;
	
	public override void _Ready()
	{
		timerCurrentValue = -1;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (timerCurrentValue > 0.0)
		{
			// subtract delta time
			timerCurrentValue -= delta;

			// change slider inside and lerp color
			SliderInside.Scale = new Vector2((float)(timerCurrentValue / TimerMaxValue), 1.0f);
			SliderInside.Color = BadColor.Lerp(GoodColor, SliderInside.Scale.X);

            // set timer label for time
            TimerLabel.Text = string.Format("{0:0.00}", timerCurrentValue) + " s";

			if (!playedCough && SliderInside.Scale.X < 0.25f)
			{
				playedCough = true;
				AudioManager.Instance.PlaySound("cough");
			}

            // If timer is below 0, zero it and set label again to ""
            if (timerCurrentValue <= 0.0 && OnTimerStop != null)
			{
				timerCurrentValue = 0.0;
				TimerLabel.Text = "";
                OnTimerStop();
			}            
        }
		else
		{
            SliderInside.Scale = new Vector2(0, 1);
        }
    }	

	public void StopTimer()
	{
		timerCurrentValue = 0;
        TimerLabel.Text = "";
    }

	public void RestartTimer(double newMaxValue)
	{
		TimerMaxValue = newMaxValue;
		timerCurrentValue = TimerMaxValue;
		playedCough = false;
        SliderInside.Scale = new Vector2(1, 1);
    }
}
