using Godot;
using JumpHero;
using System;

namespace JumpHero
{
	public partial class Player : CharacterBody2D
	{
		// signals
		[Signal] public delegate void OnStateChangeEventHandler();
		[Signal] public delegate void OnDirectionChangeEventHandler();
		[Signal] public delegate void OnChargeChangeEventHandler();

		// player component node names

		// static constants
		public static readonly float DEFAULT_GRAVITY = 9.8f;
		public static readonly float FREEFALL_THRESHOLD = 1200f;
		public static readonly float SLOPE_ANGLE_THRESHOLD = Mathf.DegToRad(25);

		// data members
		public float Gravity { get; private set; } = DEFAULT_GRAVITY;
		public float MoveSpeed { get; private set; } = 150f;
		public bool IsFacingRight { get; private set; } = false;
		public float Elasticity { get; private set; } = 0.8f;

		public override void _Ready()
		{
			FloorMaxAngle = SLOPE_ANGLE_THRESHOLD;
		}

		public void NotifyStateChange(PlayerStateManager.PlayerState oldState, PlayerStateManager.PlayerState newState)
		{
			EmitSignal(SignalName.OnStateChange, (int) oldState, (int) newState);
		}

		public void SetDirection(bool isRight)
		{
			if (IsFacingRight == isRight) return;
			IsFacingRight = isRight;
			EmitSignal(SignalName.OnDirectionChange, isRight);
		}

		public void EmitChargePercentage(float percentage)
		{
			EmitSignal(SignalName.OnChargeChange, percentage);
		}
	}
}
