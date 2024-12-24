using Godot;
using JumpHero;
using System;

public partial class LevelManager : Node
{
    public Player Player { get; private set; }
    public Camera2D Camera { get; private set; }

    [Signal] public delegate void OnLevelTransitionedEventHandler(Vector2 newCameraPosition);

    private float _cameraYOffset = -1296f; // Viewport Dimensions (1152 x 648) --> Camera Dimensions (2304 x 1296)

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Player = GetNode<CharacterBody2D>("../Player") as Player;
        Camera = GetNode<Camera2D>("../Camera2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        PlayerLevelTransition();
	}

    private void PlayerLevelTransition()
    {
        if (Player.Position.Y < (Camera.Position.Y + _cameraYOffset / 2))
        {
            RequestLevelTransition(Camera.Position + new Vector2(0, _cameraYOffset));
        }
        else if (Player.Position.Y > (Camera.Position.Y - _cameraYOffset / 2))
        {
            RequestLevelTransition(Camera.Position - new Vector2(0, _cameraYOffset));
        }
    }

    private void RequestLevelTransition(Vector2 newCameraPosition)
    {
        EmitSignal(SignalName.OnLevelTransitioned, newCameraPosition);
    }
}
