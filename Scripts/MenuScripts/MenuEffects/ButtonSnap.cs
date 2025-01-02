using Godot;
using System;

public partial class ButtonSnap : Button
{
	[Export] private bool _activeOnReady = true;
	[Export] private float _hoveredScale = 1.3f;
	[Export] private float _hoveredAngle = -20f;
	[Export] private float _snapSpeed = 0.1f;
	private bool _active;
	private float _hoverAngleInRad = 0;
	public override void _Ready()
	{
		PivotOffset = Size / 2;
		_hoverAngleInRad = Mathf.DegToRad(_hoveredAngle);
		Connect(SignalName.MouseEntered, Callable.From(PlayHoverEffect));
		Connect(SignalName.MouseExited, Callable.From(RevertHoverEffect));
		
		_active = _activeOnReady;
		// Used to tell button when button effects can be played, we do this to avoid casting by user
		// Assumes theme used for buttons and UI will never change
		if (!_activeOnReady) Connect(SignalName.ThemeChanged, Callable.From(() => 
		{
			_active = true;
			if (IsHovered()) PlayHoverEffect();
		}));
	}
	private void PlayHoverEffect()
	{
		if (!_active) return; // Don't play button effect if not active yet

		Tween scaleTween = CreateTween();
		Tween angleTween = CreateTween();
		scaleTween.TweenProperty(this, nameof(Scale).ToLower(), Vector2.One * _hoveredScale, _snapSpeed);
		angleTween.TweenProperty(this, nameof(Rotation).ToLower(), _hoverAngleInRad, _snapSpeed);
		scaleTween.Play();
		angleTween.Play();
	}
	private void RevertHoverEffect()
	{
		if (!_active) return;

		Tween scaleTween = CreateTween();
		Tween angleTween = CreateTween();
		scaleTween.TweenProperty(this, nameof(Scale).ToLower(), Vector2.One, _snapSpeed);
		angleTween.TweenProperty(this, nameof(Rotation).ToLower(), 0, _snapSpeed);
		scaleTween.Play();
		angleTween.Play();
	}
}
