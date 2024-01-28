using Godot;
using System;

public partial class AngryCloud : Node2D
{
	[Export]
	private double OpacityCooldown;
	[Export]
	private double MaxOpacityCooldown = 1.0f;
	[Export]
	private double PulsingCooldown;
	[Export]
	private double MaxPulsingCooldown = 1.0f;
	[Export]
	private Vector2 MaxAbsoluteOffset = new(-5, 5);

	[Export]
	private Node2D ChildInner;

	[Export]
	private Node2D ChildOuter;

	[Export]
	private float FrequencyModifier = 3.0f;

	[Export]

	private double timer = 0;

	[Export]
	private State AnimationState = State.FadingIn;


	[Export]
	public bool DestroyOnFadeOut = false;

	public override void _Ready()
	{
		OpacityCooldown = MaxOpacityCooldown;
		AnimationState = State.FadingIn;

		GD.Randomize();
		float positionX = (float)GD.RandRange(MaxAbsoluteOffset.X, MaxAbsoluteOffset.Y);
		float positionY = (float)GD.RandRange(MaxAbsoluteOffset.X, MaxAbsoluteOffset.Y);
		ChildOuter.Position = new Vector2(positionX, positionY);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (AnimationState == State.FadingIn || AnimationState == State.FadingOut)
		{
			ChildOuter.SelfModulate = new Color(1, 1, 1, 1.0f - (float)(OpacityCooldown / MaxOpacityCooldown));

			if (AnimationState == State.FadingIn)
				OpacityCooldown -= delta;
			else
				OpacityCooldown += delta;

			if (OpacityCooldown < 0 && AnimationState == State.FadingIn)
			{
				OpacityCooldown = MaxOpacityCooldown;
				PulsingCooldown = MaxPulsingCooldown;
				AnimationState = State.Pulsing;

			}
			else if (OpacityCooldown > MaxOpacityCooldown && AnimationState == State.FadingOut)
			{
				if (DestroyOnFadeOut)
				{
					QueueFree();
					return;
				}

				OpacityCooldown = MaxOpacityCooldown;
				AnimationState = State.FadingIn;

				float positionX = (float)GD.RandRange(MaxAbsoluteOffset.X, MaxAbsoluteOffset.Y);
				float positionY = (float)GD.RandRange(MaxAbsoluteOffset.X, MaxAbsoluteOffset.Y);

				ChildOuter.Position = new Vector2(positionX, positionY);
			}
		}
		else if (AnimationState == State.Pulsing)
		{
			ChildOuter.SelfModulate = new Color(1, 1, 1, 1);

			PulsingCooldown -= delta;

			if (PulsingCooldown < 0)
			{
				AnimationState = State.FadingOut;
				OpacityCooldown = 0;
			}
		}

		float parentScaleComponent = 1.0f + (Mathf.Sin((float)timer * FrequencyModifier) * 0.1f);
		float childScaleComponent = 1.0f + (Mathf.Cos((float)timer * FrequencyModifier) * 0.1f);

		ChildOuter.Scale = new Vector2(parentScaleComponent, parentScaleComponent);
		ChildInner.Scale = new Vector2(childScaleComponent, childScaleComponent);

		timer += delta;
	}

	private enum State
	{
		FadingIn, Pulsing, FadingOut
	}
}
