using Godot;
using JumpHero;
using System;

namespace JumpHero
{
	using PlayerState = PlayerStateManager.PlayerState;
	public partial class PlayerParticleEffects : GpuParticles2D
	{
		public override void _Ready()
		{
			// Connect player signals
			Player player = GetOwner<Player>();
			player.Connect(Player.SignalName.OnChargeChange, Callable.From((float percentage) => OnChargeChange(percentage)));
			player.Connect(Player.SignalName.OnStateChange, 
				Callable.From((PlayerState oldState, PlayerState newState) => OnStateChange(oldState, newState))
			);
			player.Connect(Player.SignalName.OnCollision, Callable.From((KinematicCollision2D collision) => OnCollide(collision)));
			player.Connect(Player.SignalName.OnDirectionChange, Callable.From((bool isRight) => OnDirectionChange(isRight)));

			Modulate = PlayerSpriteEffect.DEFAULT_PLAYER_COLOR;
		}
		
		private void OnChargeChange(float percentage)
		{

		}

		private void OnStateChange(PlayerState oldState, PlayerState newState)
		{

		}

		private void OnCollide(KinematicCollision2D collision)
		{

		}

		private void OnDirectionChange(bool isRight)
		{
			
		}
	}

}
