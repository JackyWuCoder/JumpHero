using Godot;
using JumpHero;
using System;

namespace JumpHero
{
	public partial class Player : CharacterBody2D
	{
		// static constants
		public static readonly float DEFAULT_GRAVITY = 9.8f;
		public static readonly float FREEFALL_THRESHOLD = 100f;

		// data members
		public float Gravity { get; private set; } = DEFAULT_GRAVITY;
		public float MoveSpeed { get; private set; } = 150f;
		public bool IsFacingRight { get; private set; } = false;

		// signals
		[Signal] public delegate void OnStateChangeEventHandler();
		[Signal] public delegate void OnDirectionChangeEventHandler();

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
	}
}
