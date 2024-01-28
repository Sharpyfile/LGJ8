using Godot;
using System;

public partial class InfluenceColor : Label
{
	public void Initialize(int v)
	{
		GD.Print(Name, GetColor(v));
		LabelSettings = new LabelSettings
		{
			FontColor = GetColor(v),
			FontSize = 180
		};
		Text = v.ToString();
	}

	private Color GetColor(int v)
	{
		switch (v)
		{
			case > 0: return Constants.GOOD_COLOR;
			case < 0: return Constants.BAD_COLOR;
			default: return Constants.NEUTRAL_COLOR;
		}
	}
}
