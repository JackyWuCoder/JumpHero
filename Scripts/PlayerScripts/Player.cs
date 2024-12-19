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

		// player component node names
		private static readonly string SLOPE_DETECTOR_NODE_NAME = "SlopeDetector";

		// static constants
		public static readonly float DEFAULT_GRAVITY = 9.8f;
		public static readonly float FREEFALL_THRESHOLD = 100f;
		private static readonly float SLOPE_ANGLE_THRESHOLD = Mathf.DegToRad(25);

		// data members
		public float Gravity { get; private set; } = DEFAULT_GRAVITY;
		public float MoveSpeed { get; private set; } = 150f;
		public bool IsFacingRight { get; private set; } = false;

		public override void _Ready()
		{
			
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

		public float GetSlopeAngle()
		{
			// TODO: Add ability to calculate slope angle player is on, such that when landing on a slope the player will slide off the slope instead of landing on it directly
			return 0;
		}
	}
}
