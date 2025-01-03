using Godot;
using JumpHero;
using System;

public partial class CameraManager : Camera2D
{
    [Export] private float _transitionSpeed = 0.15f;
    
    // Viewport Dimensions (1152 x 648) --> Camera Dimensions (2304 x 1296)
    private float _cameraYOffset = 1296f; // TODO: Change so it is not hard coded
    private bool _isTransitioningLevel = false;
    private Vector2 _targetPositionOfLevel;

	public override void _Ready()
	{
        GetOwner<LevelManager>().Connect(LevelManager.SignalName.LevelTransition,
            Callable.From((Player player) => TransitionCamera(player))
        );
	}

    private void TransitionCamera(Player player)
    {
        if (_isTransitioningLevel) return;
        // Calculates new camera position via ternary operator
        Vector2 newCameraPosition = player.Position.Y < (Position.Y + _cameraYOffset / 2) ? 
            Position + Vector2.Up * _cameraYOffset : Position + Vector2.Down * _cameraYOffset;

        Tween transition = CreateTween().SetEase(Tween.EaseType.Out);
        transition.TweenProperty(this, nameof(Position).ToLower(), newCameraPosition, _transitionSpeed);
        transition.TweenCallback(Callable.From(() => _isTransitioningLevel = false));
        transition.Play();
    }
}
