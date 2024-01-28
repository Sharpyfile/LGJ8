using Godot;
using System;

public partial class Emote : Node2D
{
    [Export]
    private double OpacityCooldown;
    [Export]
    private double MaxOpacityCooldown = 1.0f;
    [Export]
    private Vector2 MaxAbsoluteOffset = new(-5, 5);

    [Export]
    private Node2D ChildOuter;

    [Export]
    private double timer = 0;

    [Export]
    private State AnimationState = State.FadingIn;

    [Export]
    public bool DestroyOnFadeOut = true;

    // Called when the node enters the scene tree for the first time.
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
        ChildOuter.SelfModulate = new Color(1, 1, 1, 1.0f - (float)(OpacityCooldown / MaxOpacityCooldown));

        if (AnimationState == State.FadingIn)
            OpacityCooldown -= delta;
        else
            OpacityCooldown += delta;

        if (OpacityCooldown < 0 && AnimationState == State.FadingIn)
        {
            OpacityCooldown = MaxOpacityCooldown;
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

    private enum State
    {
        FadingIn, Pulsing, FadingOut
    }
}
