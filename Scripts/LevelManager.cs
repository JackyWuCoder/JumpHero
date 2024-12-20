using Godot;
using JumpHero;
using System;

public partial class LevelManager : Node
{
    public Camera2D Camera { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Camera = GetNode<Camera2D>("../Player/Camera2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
