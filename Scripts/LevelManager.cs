using Godot;
using JumpHero;
using System;

public partial class LevelManager : Node
{
    public Camera2D Camera { get; private set; }
    public Player Player { get; private set; }

    private float _cameraYOffset = -1296f; // Viewport Dimensions (1152 x 648) --> Camera Dimensions (2304 x 1296)
    private bool _isTransitioning = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Player = GetNode<CharacterBody2D>("../Player") as Player;
        Camera = GetNode<Camera2D>("../Camera2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if ((Player.Position.Y < (Camera.Position.Y + _cameraYOffset / 2)) && !_isTransitioning) {
            StartLevelTransition();
        }
        if (_isTransitioning)
        {
            Camera.Position = new Vector2(Camera.Position.X, Camera.Position.Y + _cameraYOffset);
            _isTransitioning = false;
        }
	}

    public void StartLevelTransition()
    {
        _isTransitioning = true;
    }
}
