using Godot;
using System;

public partial class AudioPlayer : AudioStreamPlayer
{

	private bool stoppingMusic = false;
	private bool waitingForMusic = false;
	private double StoppingCooldownCountdown = 0;
	private double MaxStoppingCooldownCountdown = 0;

    private double WaitingCooldownCountdown = 0;

    public void PlaySound(AudioStream audioStream, float delay = 0)
	{
		Stream = audioStream;
		if (delay <= 0.0f)
			Play();
		else
		{
			WaitingCooldownCountdown = delay;
			waitingForMusic = true;
        }
    }

	public void StopMusic(float cooldown)
	{
		MaxStoppingCooldownCountdown = cooldown;
		StoppingCooldownCountdown = MaxStoppingCooldownCountdown;
		stoppingMusic = true;

    }

    public override void _Process(double delta)
    {
        base._Process(delta);
		if (stoppingMusic)
		{
			StoppingCooldownCountdown -= delta;

			VolumeDb = Mathf.Lerp(-20, 0, (float)(StoppingCooldownCountdown / MaxStoppingCooldownCountdown));

			if (StoppingCooldownCountdown <= 0)
			{
				stoppingMusic = false;
                QueueFree();
            }
        }
		else if (waitingForMusic)
		{
			WaitingCooldownCountdown -= delta;

			if (WaitingCooldownCountdown < 0)
			{
				waitingForMusic = false;
				Play();
			}
        }
	}


    private void OnFinishedDestroy()
	{
		QueueFree();
	}
}
