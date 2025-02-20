using Godot;
using System;

namespace JumpHero
{
	public partial class Freefall : State
	{
        public override void EnterState()
        {
            
        }

        public override void ExitState()
        {
            // TODO: Link up GPUParticle emitter to release particles to simulate hard landing
        }

        public override void Process(double delta)
        {
            
        }

        public override void PhysicsProcess(double delta) 
		{
			float velocityXComponent = player.Velocity.X;
			const float gravityBoost = 1.2f;
			player.MoveAndSlide();
			player.Velocity = new Vector2(player.GetRealVelocity().X, player.Velocity.Y + player.Gravity * gravityBoost);
            base.PhysicsProcess(delta);

			const float elasticity = 0.5f;
			if (player.IsOnWallOnly())
					player.Velocity += Vector2.Left * velocityXComponent * elasticity;

			if (player.IsOnFloor())
					stateManager.ChangeState(PlayerStateManager.PlayerState.GROUNDED);

            else if (player.GetSlideCollisionCount() > 0)
                player.NotifyCollision();
		}

        public override void InputProcess(InputEvent inputEvent)
        {
            
        }
    }
}
