using Godot;
using System;

public partial class Bobbing : Control
{
	[Export] private float _positionOffset = 20;
	[Export] private float _bobSpeed = 5;
	private float _baselineY = 0;
	private float theta = 0;

	public override void _Ready()
	{
		PivotOffset = Size / 2;
		_baselineY = Position.Y;
	}

	public override void _Process(double delta)
	{
		// Calculate bobbing position
		float newHeight = Mathf.Sin(theta) * _positionOffset + _baselineY;
		Position = new Vector2(Position.X, newHeight);
		theta = theta < Mathf.Pi * 2 ? theta + (float) delta * _bobSpeed : 0;
	}
}
