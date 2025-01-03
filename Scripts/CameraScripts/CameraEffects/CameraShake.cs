using Godot;
using System;

public partial class CameraShake : Node
{
	[Export] private float _maxRotationDegrees = 5;
	[Export] private Vector2 _maxOffset = new(125, 25);
	[Export] private float _traumaDecay = 0.8f;
	private FastNoiseLite _noise;
	private Camera2D _camera;
	private uint _noisePosition;
	private float _trauma = 0.0f;

	public override void _Ready()
	{
		_camera = GetParent<Camera2D>();
		_noise = new() 
		{ 
			NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin,
			FractalOctaves = 2,
			FractalGain = 4
		};
		_noisePosition = GenerateNewMapPosition();
	}

	public override void _Process(double delta)
	{
		if (!Mathf.IsZeroApprox(_trauma))
		{
			_trauma = Mathf.Max(_trauma - _traumaDecay * (float) delta, 0);
			Shake();
		}
		else _noisePosition = GenerateNewMapPosition();
	}

	public void Shake(float amount = 0)
    {
        if (GlobalVariables.Instance.ScreenShakeAmount == 0) return;
		
		// Limit the maximum trauma amount to 2
		const short maxTrauma = 6;
		if (amount != 0) _trauma = amount > maxTrauma ? maxTrauma : amount;
		
		// Calculate shake offset values
		float optionsFactor = GlobalVariables.Instance.ScreenShakeAmount;
		_noisePosition += 1;
		float shakeFactor = Mathf.Pow(_trauma, 3);
		_camera.RotationDegrees = _maxRotationDegrees * optionsFactor * shakeFactor * _noise.GetNoise2D(_noise.Seed, _noisePosition);
		_camera.Offset = new Vector2(
			_maxOffset.X * shakeFactor * optionsFactor * _noise.GetNoise2D(_noise.Seed * 2, _noisePosition),
			_maxOffset.Y * shakeFactor * optionsFactor * _noise.GetNoise2D(_noise.Seed *3, _noisePosition)
		);
    }

	private static uint GenerateNewMapPosition()
	{
		return GD.Randi() % 100 + 10;
	}
}
