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
            player.Velocity = new Vector2(player.GetRealVelocity().X, player.Velocity.Y + player.Gravity);

            // const float elasticity = 0.8f;
            if (player.IsOnWallOnly())
            {
                BounceOffWall();
                // player.Velocity += Vector2.Left * velocityXComponent * elasticity;
            }
                
            if (player.IsOnFloor())
                stateManager.ChangeState(PlayerStateManager.PlayerState.GROUNDED);
                
            else if (player.Velocity.Y > Player.FREEFALL_THRESHOLD)
                stateManager.ChangeState(PlayerStateManager.PlayerState.FREEFALL);
        }

        public override void InputProcess(InputEvent inputEvent)
        {
            
        }

        private void BounceOffWall()
        {
            if(player.GetSlideCollisionCount() > 0) {
                var collision = player.GetSlideCollision(0);
                Vector2 surfaceNormal = collision.GetNormal();
                float surfaceAngle = Mathf.Abs(90 + Mathf.RadToDeg(surfaceNormal.Angle()));
                if (surfaceAngle >= 45 || surfaceAngle == 90 || surfaceAngle == 270)
                {
                    player.Velocity = player.Velocity.Bounce(surfaceNormal) * player.Elasticity;
                }
            }
        }
    }
}
