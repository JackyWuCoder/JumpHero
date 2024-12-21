using Godot;
using JumpHero;
using System;

public partial class LevelManager : Node
{
    public Camera2D Camera { get; private set; }

    private float _cameraYOffset = -1296f; // Viewport Dimensions (1152 x 648) --> Camera Dimensions (2304 x 1296)

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Camera = GetNode<Camera2D>("../Camera2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
       // Camera.Position = new Vector2(Camera.Position.X, )
	}
}
