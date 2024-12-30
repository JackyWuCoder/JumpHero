using Godot;
using JumpHero;
using System;

namespace JumpHero
{
	public partial class Player : CharacterBody2D
	{
		// signals
		[Signal] public delegate void OnScreenExitEventHandler();
		[Signal] public delegate void OnStateChangeEventHandler();
		[Signal] public delegate void OnDirectionChangeEventHandler();
		[Signal] public delegate void OnChargeChangeEventHandler();
		[Signal] public delegate void OnWalkEventHandler();
		[Signal] public delegate void OnJumpEventHandler();
		[Signal] public delegate void OnCollisionEventHandler();

		// player component node names
		private static readonly string STATE_MANAGER_NODE_NAME = "States";
		private static readonly string SCREEN_NOTIFIER_NODE_NAME = "ScreenNotifier";

		// static constants
		public static readonly float DEFAULT_GRAVITY = 9.8f;
		public static readonly float FREEFALL_THRESHOLD = 1100f;
		public static readonly float SLOPE_ANGLE_THRESHOLD = Mathf.DegToRad(25);
		public static readonly float MAX_JUMP_HEIGHT = 900f;
		public static readonly float MIN_JUMP_HEIGHT = 90f;
		public static readonly float MAX_JUMP_WIDTH = 350f;
		public static readonly float MIN_JUMP_WIDTH = 35f;

		// data members
		public float Gravity { get; private set; } = DEFAULT_GRAVITY;
		public float MoveSpeed { get; private set; } = 175f;
		public bool IsFacingRight { get; private set; } = false;
		public bool IsWalking { get; private set; } = false;
		public float Elasticity { get; private set; } = 0.8f;
		public PlayerStateManager.PlayerState State { get { return _stateManager.State; } }
		private PlayerStateManager _stateManager;

		public override void _Ready()
		{
			_stateManager = GetNode<PlayerStateManager>(STATE_MANAGER_NODE_NAME);
			FloorMaxAngle = SLOPE_ANGLE_THRESHOLD;
			GetNode<VisibleOnScreenNotifier2D>(SCREEN_NOTIFIER_NODE_NAME)
				.Connect(VisibleOnScreenNotifier2D.SignalName.ScreenExited,
					Callable.From(() => EmitSignal(SignalName.OnScreenExit))
			);
		}

		public void NotifyJump()
		{
			EmitSignal(SignalName.OnJump);
		}

		public void StartWalk(bool isWalking)
		{
			IsWalking = isWalking;
			EmitSignal(SignalName.OnWalk, isWalking);
		}

		public void NotifyCollision()
		{
			EmitSignal(SignalName.OnCollision, GetSlideCollision(0));
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
