using Godot;
using System;

namespace JumpHero
{
	public partial class PlayerSpriteEffect : Sprite2D
	{
		// static constants
		public static readonly Color DEFAULT_PLAYER_COLOR = new("e64c0e"); // Orange

		// animation names
		private static readonly string SQUASH_BOTTOM = "SquashBottom"; // played when soft landing (airborne -> grounded)
		private static readonly string SQUASH_LEFT = "SquashLeft"; // played when hitting left wall
		private static readonly string SQUASH_RIGHT = "SquashRight"; // played when hitting right wall
		private static readonly string SQUASH_TOP = "SquashTop"; // played when hitting ceiling
		private static readonly string STAND = "Stand"; // played when input made after hard landing
		private static readonly string TRIP = "Trip"; // played when hard landing (freefall -> grounded)
		private static readonly string WALK = "Walk"; // played when moving left and right on the ground
		private static readonly string FALLING = "Falling"; // played when in freefall state
		private static readonly string RESET = "RESET"; // played when sprite properties need to be reset

		// static constants
		private static readonly string ANIMATION_NODE_NAME = "Animator";
		private static readonly float MAX_SQUASH_AMOUNT_Y = 0.4f;
		private static readonly float MAX_SQUASH_AMOUNT_X = 1.3f;

		// data members
		[Export] private Vector2 _spriteSize = new(50, 50);
		private AnimationPlayer _animation;

		public override void _Ready()
		{
			_animation = GetNode<AnimationPlayer>(ANIMATION_NODE_NAME);
			
			// Connect player signals
			Player player = GetOwner<Player>();
			player.Connect(Player.SignalName.OnChargeChange, Callable.From((float chargePercent) => OnChargeChange(chargePercent)));
			player.Connect(Player.SignalName.OnStateChange, 
				Callable.From(
					(PlayerStateManager.PlayerState oldState, PlayerStateManager.PlayerState newState) => OnStateChange(oldState, newState)
				)
			);
			player.Connect(Player.SignalName.OnWalk, Callable.From((bool isWalking) => OnWalkChange(isWalking)));
			player.Connect(Player.SignalName.OnCollision, Callable.From((KinematicCollision2D collision) => OnCollision(collision)));

			// Initialize player visuals
			Modulate = DEFAULT_PLAYER_COLOR;
		}

		// 5:31am loud bang sound
		public void SetColor(Color color)
		{
			Modulate = color;
		}

        private void OnStateChange(PlayerStateManager.PlayerState oldState, PlayerStateManager.PlayerState newState)
		{
			if (newState == PlayerStateManager.PlayerState.GROUNDED)
			{
				if (oldState == PlayerStateManager.PlayerState.FREEFALL)
				{
					_animation.Play(RESET);
					_animation.Queue(TRIP);
				}
				else _animation.Play(SQUASH_BOTTOM); // previous state is airborne
			}
			else if (newState == PlayerStateManager.PlayerState.FREEFALL) _animation.Play(FALLING);
		}

		private void OnCollision(KinematicCollision2D collision)
		{
			// skip squash animations if player is in freefall state
			if (_animation.CurrentAnimation == FALLING) return;

			// Check if collision is a wall and not a slope
			if (CalculationHelper.IsValidCollider(collision, 0)) _animation.Play(SQUASH_LEFT);
			else if (CalculationHelper.IsValidCollider(collision, Mathf.Pi / 2)) _animation.Play(SQUASH_TOP);
			else if (CalculationHelper.IsValidCollider(collision, Mathf.Pi)) _animation.Play(SQUASH_RIGHT);
		}

		private void OnWalkChange(bool isWalking)
		{
			if (_animation.AssignedAnimation == TRIP && isWalking)
			{
				_animation.Play(STAND);
				_animation.Queue(WALK);
			}
			else if (isWalking) _animation.Queue(WALK);
			else _animation.Play(RESET);
		}

		private void OnChargeChange(float chargePercent)
		{
			if (chargePercent == 0) PlayStretchAnimation();
			else if (chargePercent == 1) VibrateSprite();
			else SquashSprite(chargePercent);
		}

		private void PlayStretchAnimation()
		{
			Tween tween = CreateTween();
			tween.TweenProperty(this, nameof(Scale).ToLower(), new Vector2(0.8f, 1.2f), 0.1f);
			tween.TweenProperty(this, nameof(Scale).ToLower(), Vector2.One, 0.1f);
			tween.TweenProperty(this, nameof(Position).ToLower(), Vector2.Zero, 0.05f);
			tween.Play();
		}

		private void VibrateSprite()
		{
			const float range = 0.8f;
			float randomX = (float) GD.RandRange(-range, range), randomY = (float) GD.RandRange(-range, range);
			const float offsetLimit = 1.2f;
			float squashOffset = _spriteSize.Y / 3;
			Position = new Vector2
			(
				Mathf.Clamp(Position.X + randomX, -offsetLimit, offsetLimit),
				Mathf.Clamp(Position.Y + randomY, -offsetLimit + squashOffset, offsetLimit + squashOffset)
			);	
		}

		private void SquashSprite(float squashPercent)
		{
			float scaleY = 1 - (1 - MAX_SQUASH_AMOUNT_Y) * squashPercent;
			float scaleX = 1 + (MAX_SQUASH_AMOUNT_X - 1) * squashPercent;

			// TODO: Figure out how to generalize it
			float positionY = _spriteSize.Y / 3 * squashPercent;
			Scale = new Vector2(scaleX, scaleY);
			Position = new Vector2(Position.X, positionY);
		}
	}
}
