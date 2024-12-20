using Godot;
using System;

namespace JumpHero
{
	public partial class Airborne : State
	{
        private const float MIN_BOUNCE_X_VELOCITY_THRESHOLD = 100f;
        private const float IMPULSE_STRENGTH = 100f;

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
                    Vector2 reflectedVelocity = player.Velocity.Bounce(surfaceNormal) * player.Elasticity;
                    if (surfaceAngle == 90 || surfaceAngle == 270)
                    {
                        if (Mathf.Abs(reflectedVelocity.X) < MIN_BOUNCE_X_VELOCITY_THRESHOLD)
                        {
                            reflectedVelocity.X = Mathf.Sign(reflectedVelocity.X) * MIN_BOUNCE_X_VELOCITY_THRESHOLD;
                        }
                        Vector2 impulse = surfaceNormal * IMPULSE_STRENGTH;
                        reflectedVelocity += impulse;
                    }
                    player.Velocity = reflectedVelocity;
                }
            }
        }
    }
}
