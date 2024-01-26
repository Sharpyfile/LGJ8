using Godot;
using System;
using System.Collections.Generic;

public partial class AudioManager : AudioStreamPlayer
{
	public static AudioManager Instance { get; private set; }

	[Export]
	private AudioStream[] Clips;

	private Dictionary<string, AudioStream> ClipsAsDictionary = new Dictionary<string, AudioStream>();

    public override void _Ready()
	{
		Instance = this;

		for (int i = 0; i < Clips.Length; ++i)        
			ClipsAsDictionary.Add(System.IO.Path.GetFileNameWithoutExtension(Clips[i].ResourcePath), Clips[i]);        
	}

	public void PlaySound(string name)
	{ 
		if (ClipsAsDictionary.ContainsKey(name))
		{
			Stream = ClipsAsDictionary[name];
			Play();
		}
	}
}