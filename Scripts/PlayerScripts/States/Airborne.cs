using Godot;
using System;

namespace JumpHero
{
	public partial class Airborne : State
	{
        public override void EnterState()
        {
            
        }

        public override void ExitState()
        {
            
        }

        public override void Process(double delta)
        {
            
        }

        public override void PhysicsProcess(double delta)
        {
            float velocityXComponent = player.Velocity.X;
            player.MoveAndSlide();
            player.Velocity += Vector2.Down * player.Gravity;
            
            const float elasticity = 0.5f;
            if (player.IsOnWallOnly())
                player.Velocity += Vector2.Left * velocityXComponent * elasticity;
                
            if (player.IsOnFloor())
                stateManager.ChangeState(PlayerStateManager.PlayerState.GROUNDED);
                
            else if (player.Velocity.Y > Player.FREEFALL_THRESHOLD)
                stateManager.ChangeState(PlayerStateManager.PlayerState.FREEFALL);
        }

        public override void InputProcess(InputEvent inputEvent)
        {
            
        }
    }
}
