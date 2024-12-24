using Godot;
using JumpHero;
using System;

public partial class CameraManager : Camera2D
{
    private bool _isTransitioningLevel = false;
    private Vector2 _targetPositionOfLevel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        var levelManager = GetParent() as LevelManager;
        levelManager.Connect(LevelManager.SignalName.OnLevelTransitioned, Callable.From((Vector2 newCameraPosition) => OnLevelTransitioned(newCameraPosition)) );
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    private void OnLevelTransitioned(Vector2 newCameraPosition)
    {
        if (_isTransitioningLevel) return;
        _targetPositionOfLevel = newCameraPosition;
        _isTransitioningLevel = true;
    }

}
