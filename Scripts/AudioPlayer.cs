using Godot;
using System;

public partial class AudioPlayer : AudioStreamPlayer
{

	private bool stoppingMusic = false;
	private double CooldownCountdown = 0;
	private double MaxCooldownCountdown = 0;

	public void PlaySound(AudioStream audioStream)
	{
		Stream = audioStream;
		Play();
	}

	public void StopMusic(float cooldown)
	{
		MaxCooldownCountdown = cooldown;
		CooldownCountdown = MaxCooldownCountdown;
		stoppingMusic = true;

    }

    public override void _Process(double delta)
    {
        base._Process(delta);
		if (stoppingMusic)
		{
			CooldownCountdown -= delta;

			VolumeDb = Mathf.Lerp(-20, 0, (float)(CooldownCountdown / MaxCooldownCountdown));

			if (CooldownCountdown <= 0)
			{
				stoppingMusic = false;
                QueueFree();
            }
        }		
	}


    private void OnFinishedDestroy()
	{
		QueueFree();
	}
}
