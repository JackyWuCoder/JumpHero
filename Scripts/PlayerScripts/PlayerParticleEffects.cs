using Godot;
using JumpHero;
using System;

namespace JumpHero
{
	using PlayerState = PlayerStateManager.PlayerState;
	public partial class PlayerParticleEffects : GpuParticles2D
	{
		private enum EmissionType
		{
			EXPLOSIVE, // Played during trip animation
			LIGHT, // Played during any collision
			SUSTAINED_LIGHT // Played during jump wind-up
		}
		private Player _player;

		public override void _Ready()
		{
			// Connect player signals
			_player = GetOwner<Player>();
			_player.Connect(Player.SignalName.OnChargeChange, Callable.From((float percentage) => OnChargeChange(percentage)));
			_player.Connect(Player.SignalName.OnStateChange, 
				Callable.From((PlayerState oldState, PlayerState newState) => OnStateChange(oldState, newState))
			);
			_player.Connect(Player.SignalName.OnCollision, Callable.From((KinematicCollision2D collision) => OnCollide(collision)));
			_player.Connect(Player.SignalName.OnJump, Callable.From(() => Emitting = false));

			Modulate = Player.DEFAULT_PLAYER_COLOR;
			InitializeParticleProperties();
		}

		/*
			Properties already set in the editor, but adding this function just in case the properties need
			to be reset, since the properties will be changing depending on the emission type. This function will 
			initialize the particle properties to EXPLOSIVE type.
		*/
		private void InitializeParticleProperties()
		{
			ParticleProcessMaterial material = ProcessMaterial as ParticleProcessMaterial;

			// Non-changing properties
			material.RadialVelocityMin = Mathf.DegToRad(100);
			material.RadialVelocityMax = Mathf.DegToRad(100);
			material.Gravity = Vector3.Up * 400; // (0, 400, 0), Will need if there is wind effects in environment
			material.HueVariationMin = -0.02f;
			material.HueVariationMax = 0.02f;
			material.CollisionBounce = 1;
			material.CollisionMode = ParticleProcessMaterial.CollisionModeEnum.Rigid;
			material.EmissionShape = ParticleProcessMaterial.EmissionShapeEnum.Box;
			material.AngleMin = Mathf.DegToRad(-100);
			material.AngleMax = Mathf.DegToRad(100);
			material.Spread = 75;

			// Changing properties (direction, initial velocity, scale, one shot, explosiveness)
			SetProcessProperties(EmissionType.EXPLOSIVE);
		}

		private void SetProcessProperties(EmissionType type)
		{
			Emitting = false; // Stop previous emission if any
			ParticleProcessMaterial material = ProcessMaterial as ParticleProcessMaterial;
			Modulate = _player.CurrentColor;
			switch(type)
			{
				case EmissionType.EXPLOSIVE:
					material.Direction = Vector3.Down; // (0, -1, 0)
					material.InitialVelocityMin = 100;
					material.InitialVelocityMax = 500;
					material.ScaleMin = 0.1f;
					material.ScaleMax = 0.15f;
					material.EmissionShapeOffset = Vector3.Zero;
					OneShot = true;
					Explosiveness = 1;
					Amount = 64;
					break;

				// Will not set direction, need to manually change direction before emitting
				case EmissionType.LIGHT:
					material.InitialVelocityMin = 75;
					material.InitialVelocityMax = 200;
					material.ScaleMin = 0.05f;
					material.ScaleMax = 0.08f;
					material.EmissionShapeOffset = Vector3.Zero;
					Amount = 20;
					OneShot = true;
					Explosiveness = 1;
					break;

				case EmissionType.SUSTAINED_LIGHT:
					material.Direction = Vector3.Down; // (0, -1, 0)
					material.InitialVelocityMin = 200;
					material.InitialVelocityMax = 250;
					material.ScaleMin = 0.04f;
					material.ScaleMax = 0.06f;
					material.EmissionShapeOffset = Vector3.Up * 25; // Account for lowered sprite when charging
					Amount = 32;
					Explosiveness = 0;
					OneShot = false;
					break;
			}
		}

		private void OnChargeChange(float percentage)
		{
			if (Emitting) return;
			if (percentage != 1) return;
			SetProcessProperties(EmissionType.SUSTAINED_LIGHT);
			Emitting = true;
		}

		private void OnStateChange(PlayerState oldState, PlayerState newState)
		{
			if (oldState == PlayerState.FREEFALL && newState == PlayerState.GROUNDED)
			{
				SetProcessProperties(EmissionType.EXPLOSIVE);
				Emitting = true;
			}
			else if (oldState == PlayerState.AIRBORNE && newState == PlayerState.GROUNDED)
			{
				SetProcessProperties(EmissionType.LIGHT);
				(ProcessMaterial as ParticleProcessMaterial).Direction = Vector3.Down;
				Emitting = true;
			}
			else if (newState == PlayerState.FREEFALL)
			{
				SetProcessProperties(EmissionType.SUSTAINED_LIGHT);
				Emitting = true;
			}
		}

		private void OnCollide(KinematicCollision2D collision)
		{
			if (CalculationHelper.IsValidCollider(collision, 0))
				(ProcessMaterial as ParticleProcessMaterial).Direction = Vector3.Right;

			else if (CalculationHelper.IsValidCollider(collision, Mathf.Pi / 2))
				(ProcessMaterial as ParticleProcessMaterial).Direction = Vector3.Up; // Emitting downwards

			else if (CalculationHelper.IsValidCollider(collision, Mathf.Pi))
				(ProcessMaterial as ParticleProcessMaterial).Direction = Vector3.Left;

			else return; // Not valid collision, early exit
			SetProcessProperties(EmissionType.LIGHT);
			Emitting = true;
		}
	}
}
