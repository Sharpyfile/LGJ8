using Godot;
using System;
using System.Collections.Generic;

public partial class AudioManager : Node
{
	public static AudioManager Instance { get; private set; }

	[Export]
	private AudioStream[] Clips;
	[Export] 
	private PackedScene AudioPlayerScene;

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
			AudioPlayer sound = AudioPlayerScene.Instantiate() as AudioPlayer;
			AddChild(sound);
			sound.PlaySound(ClipsAsDictionary[name]);
		}
	}
}