using Godot;
using System;

public partial class ButtonSnap : Button
{
	[Export] private float _hoveredScale = 1.3f;
	[Export] private float _hoveredAngle = -20f;
	[Export] private float _snapSpeed = 0.1f;
	private float _hoverAngleInRad = 0;
	public override void _Ready()
	{
		PivotOffset = Size / 2;
		_hoverAngleInRad = Mathf.DegToRad(_hoveredAngle);
		Connect(SignalName.MouseEntered, Callable.From(PlayHoverEffect));
		Connect(SignalName.MouseExited, Callable.From(RevertHoverEffect));
	}
	private void PlayHoverEffect()
	{
		Tween scaleTween = CreateTween();
		Tween angleTween = CreateTween();
		scaleTween.TweenProperty(this, nameof(Scale).ToLower(), Vector2.One * _hoveredScale, _snapSpeed);
		angleTween.TweenProperty(this, nameof(Rotation).ToLower(), _hoverAngleInRad, _snapSpeed);
		scaleTween.Play();
		angleTween.Play();
	}
	private void RevertHoverEffect()
	{
		Tween scaleTween = CreateTween();
		Tween angleTween = CreateTween();
		scaleTween.TweenProperty(this, nameof(Scale).ToLower(), Vector2.One, _snapSpeed);
		angleTween.TweenProperty(this, nameof(Rotation).ToLower(), 0, _snapSpeed);
		scaleTween.Play();
		angleTween.Play();
	}
}
