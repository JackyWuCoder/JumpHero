using Godot;
using System;

namespace JumpHero
{
	public partial class Freefall : State
	{
        public override void EnterState()
        {
            // TODO: Link up sprite to make it spin in mid-air to simulate losing control in the air
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
            player.Velocity += Vector2.Down * player.Gravity * gravityBoost;
            player.MoveAndSlide();

            const float elasticity = 0.3f;
            if (player.IsOnWallOnly())
                player.Velocity += Vector2.Left * velocityXComponent * elasticity;

            float slopeAngle = player.GetSlopeAngle();
            if (slopeAngle != 0)
                player.Velocity += Vector2.Right * player.Velocity.Y * Mathf.Cos(slopeAngle);
                
            if (player.IsOnFloor() && slopeAngle == 0)
                stateManager.ChangeState(PlayerStateManager.PlayerState.GROUNDED);
        }

        public override void InputProcess(InputEvent inputEvent)
        {
            
        }
    }
}
