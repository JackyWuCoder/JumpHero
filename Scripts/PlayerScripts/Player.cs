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
		[Signal] public delegate void OnWalkEventHandler();
		[Signal] public delegate void OnCollisionEventHandler();

		// player component node names
		private static readonly string STATE_MANAGER_NODE_NAME = "States";

		// static constants
		public static readonly float DEFAULT_GRAVITY = 9.8f;
		public static readonly float FREEFALL_THRESHOLD = 1200f;
		public static readonly float SLOPE_ANGLE_THRESHOLD = Mathf.DegToRad(25);

		// data members
		public float Gravity { get; private set; } = DEFAULT_GRAVITY;
		public float MoveSpeed { get; private set; } = 150f;
		public bool IsFacingRight { get; private set; } = false;
		public float Elasticity { get; private set; } = 0.8f;
		public PlayerStateManager.PlayerState State { get { return _stateManager.State; } }
		private PlayerStateManager _stateManager;

		public override void _Ready()
		{
			_stateManager = GetNode<PlayerStateManager>(STATE_MANAGER_NODE_NAME);
			FloorMaxAngle = SLOPE_ANGLE_THRESHOLD;
		}

		public void StartWalk(bool isWalking)
		{
			EmitSignal(SignalName.OnWalk, isWalking);
		}

		public void NotifyCollision(KinematicCollision2D collision)
		{
			// TODO: call this from states to notify that collision occurred and send the collider object through
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
