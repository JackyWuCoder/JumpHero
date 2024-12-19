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
            if (player.IsOnFloor())
            {
                stateManager.ChangeState(PlayerStateManager.PlayerState.GROUNDED);
                return;
            }
            const float gravityBoost = 1.2f;
            player.Velocity += Vector2.Down * player.Gravity * gravityBoost;
            player.MoveAndSlide();
        }

        public override void InputProcess(InputEvent inputEvent)
        {
            
        }
    }
}
