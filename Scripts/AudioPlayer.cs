using Godot;
using System;

public partial class AudioPlayer : AudioStreamPlayer
{
	public void PlaySound(AudioStream audioStream)
	{
		Stream = audioStream;
		Play();
	}

	private void OnFinishedDestroy()
	{
		QueueFree();
	}
}
