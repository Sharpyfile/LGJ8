using Godot;
using System;
using System.Collections.Generic;
public partial class OptionMenuUI : Control
{
	[Export]
	private OptionButton ResolutionButton;

	[Export]
	private OptionButton ScreenModeButton;

	[Export]
	private Slider SoundVolumeSlider;

	private Vector2I[] Resolutions;
	private List<Window.ModeEnum> WindowModes = new();

    public override void _Ready()
    {
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), (float)SoundVolumeSlider.Value);
        Resolutions = new Vector2I[]
		{
			new Vector2I(1280, 720),
			new Vector2I(1366, 768),
			new Vector2I(1600, 900),
			new Vector2I(1920, 1080),
			new Vector2I(2560, 1440 ),
			new Vector2I(3840, 2160 )
        };
               
        while (ResolutionButton.ItemCount > 0)
			ResolutionButton.RemoveItem(0);

        for (int i = 0; i < Resolutions.Length; ++i)		
			ResolutionButton.AddItem($"{Resolutions[i].X}x{Resolutions[i].Y}");

        ResolutionButton.Selected = 3; // 1920x1080


        WindowModes.Add(Window.ModeEnum.Windowed);
		ScreenModeButton.AddItem("Windowed");
        WindowModes.Add(Window.ModeEnum.Fullscreen);
        ScreenModeButton.AddItem("Fullscreen");
        WindowModes.Add(Window.ModeEnum.ExclusiveFullscreen);
        ScreenModeButton.AddItem("Exclusive Fullscreen");

        ScreenModeButton.Selected = 2; // Exclusive Fullscreen

        //GetViewport().GetWindow().Mode = WindowModes[ScreenModeButton.Selected];
        //GetViewport().GetWindow().Size = Resolutions[3];

        base._Ready();
    }

    public void OnClickApplyOptions()
    {
        GetViewport().GetWindow().Mode = WindowModes[ScreenModeButton.Selected];
        GetViewport().GetWindow().Size = Resolutions[ResolutionButton.Selected];
    }

	public void OnValueChangeSoundVolume(float value)
	{
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), value);
    }
}