using Godot;
using System;

namespace JumpHero
{
	public partial class PlayerSpriteEffect : Sprite2D
	{
		[Export] private Vector2 _spriteSize = new(50, 50);
		private static readonly float MAX_SQUASH_AMOUNT_Y = 0.4f;
		private static readonly float MAX_SQUASH_AMOUNT_X = 1.3f;

		public override void _Ready()
		{
			Player player = GetOwner<Player>();
			player.Connect(Player.SignalName.OnChargeChange, Callable.From((float chargePercent) => OnChargeChange(chargePercent)));
			player.Connect(Player.SignalName.OnStateChange, 
				Callable.From(
					(PlayerStateManager.PlayerState oldState, PlayerStateManager.PlayerState newState) => OnStateChange(oldState, newState)
				)
			);
		}

		private void OnStateChange(PlayerStateManager.PlayerState oldState, PlayerStateManager.PlayerState newState)
		{
			// TODO: Implement this part
		}

		private void OnChargeChange(float chargePercent)
		{
			// Play stretch animation
			if (chargePercent == 0) 
			{
				Tween tween = CreateTween();
				tween.TweenProperty(this, nameof(Scale).ToLower(), new Vector2(0.8f, 1.2f), 0.1f);
				tween.TweenProperty(this, nameof(Scale).ToLower(), Vector2.One, 0.1f);
				tween.TweenProperty(this, nameof(Position).ToLower(), Vector2.Zero, 0.05f);
				tween.Play();
				return;
			}

			// Play shaking animation
			if (chargePercent == 1) 
			{
				float randomX = (float) GD.RandRange(-0.5, 0.5), randomY = (float) GD.RandRange(-0.5, 0.5);
				const float offsetLimit = 0.6f;
				float squashOffset = _spriteSize.Y / 3;
				Position = new Vector2
				(
					Mathf.Clamp(Position.X + randomX, -offsetLimit, offsetLimit),
					Mathf.Clamp(Position.Y + randomY, -offsetLimit + squashOffset, offsetLimit + squashOffset)
				);
				return;
			}

			float scaleY = 1 - (1 - MAX_SQUASH_AMOUNT_Y) * chargePercent;
			float scaleX = 1 + (MAX_SQUASH_AMOUNT_X - 1) * chargePercent;

			// TODO: Figure out how to generalize it
			float positionY = _spriteSize.Y / 3 * chargePercent;
			Scale = new Vector2(scaleX, scaleY);
			Position = new Vector2(Position.X, positionY);
		}
	}
}
