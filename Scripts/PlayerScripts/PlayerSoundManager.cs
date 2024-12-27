using Godot;
using System;

namespace JumpHero
{
	// Added to shorten the enum reference
	using PlayerState = PlayerStateManager.PlayerState;

	public partial class PlayerSoundManager : SoundManager
	{
		// Audio clip references
		private static readonly string TRIP_SFX = "Trip";
		private static readonly string COLLIDE_SFX = "Collide";
		private static readonly string JUMP_SFX = "Jump";
		private static readonly string CHARGING_SFX = "WindUp";
		private static readonly string CHARGING_LOOP_SFX = "ChargeLoop";

		public override void _Ready()
		{
			base._Ready();

			// Connecting to player signals
			Player player = GetOwner<Player>();
			player.Connect(Player.SignalName.OnCollision, Callable.From((KinematicCollision2D collision) => OnCollide(collision)));
			player.Connect(Player.SignalName.OnChargeChange, Callable.From((float percentage) => OnChargeChange(percentage)));
			player.Connect(Player.SignalName.OnJump, Callable.From(() => PlayAudio(JUMP_SFX)));
			player.Connect(Player.SignalName.OnStateChange,
				Callable.From((PlayerState oldState, PlayerState newState) => OnStateChange(oldState, newState))
			);
		}

		private void OnCollide(KinematicCollision2D collision)
		{
			if (CalculationHelper.IsValidCollider(collision)) PlayAudio(COLLIDE_SFX);
		}

		private void OnStateChange(PlayerState oldState, PlayerState newState)
		{
			if (oldState == PlayerState.FREEFALL && newState == PlayerState.GROUNDED)
				PlayAudio(TRIP_SFX);
			else if (oldState == PlayerState.AIRBORNE && newState == PlayerState.GROUNDED)
				PlayAudio(COLLIDE_SFX);
		}

		private void OnChargeChange(float percentage)
		{
			if (Playing) return;
			if (percentage < 1) PlayAudio(CHARGING_SFX);
			else PlayAudio(CHARGING_LOOP_SFX);
		}
	}
}
