using Godot;
using System;

public partial class GrowAndShrink : Control
{
	[Export] private float _scaleRate = 1;
	[Export] private float _minScale = 1;
	[Export] private float _maxScale = 1.1f;
	private float theta = 0;
	private float scaleDifference = 0;
	public override void _Ready()
	{
		PivotOffset = Size / 2;
		scaleDifference = (_maxScale - _minScale) / 2;
	}
	public override void _Process(double delta)
	{
		float scaleValue = -scaleDifference * Mathf.Cos(theta) + scaleDifference + _minScale;
		Scale = new Vector2(scaleValue, scaleValue);
		if (theta > Mathf.Pi * 2) theta = 0;
		else theta += (float) delta * _scaleRate;
	}
}
