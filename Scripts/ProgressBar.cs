using Godot;
using System;

public partial class ProgressBar : Control
{
    [Export]
    private ColorRect BarInside;

    [Export(PropertyHint.Range, "0.0f,1.0f,")]
    public float ProgressValue = 1.0f;

	[Export]
	private Gradient Gradient;

    public override void _Ready()
	{
        ProgressValue = Mathf.Clamp(ProgressValue, 0.0f, 1.0f);
        BarInside.Scale = new Vector2(ProgressValue, 1.0f);
        BarInside.Color = Gradient.Sample(ProgressValue);
    }


	public override void _Process(double delta)
	{
		ProgressValue = Mathf.Clamp(ProgressValue, 0.0f, 1.0f);
        BarInside.Scale = new Vector2(ProgressValue, 1.0f);
        BarInside.Color = Gradient.Sample(ProgressValue);
    }
}
