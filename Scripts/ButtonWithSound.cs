using Godot;
using System;

public partial class ButtonWithSound : Button
{
	[Export(PropertyHint.Range, "0,3,")]
	private int SoundVariant;

    public override void _Ready()
    {
        base._Ready();
        MouseEntered += () => OnHoverPlaySound();
    }

    public void OnHoverPlaySound()
	{
        GD.Print($"jazzMenu{SoundVariant}");
		AudioManager.Instance.PlaySound($"jazzMenu{SoundVariant}");
	}
}
