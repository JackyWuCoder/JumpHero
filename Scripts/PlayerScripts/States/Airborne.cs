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
            
            else if (player.GetSlideCollisionCount() > 0)
                player.NotifyCollision();
        }

        public override void InputProcess(InputEvent inputEvent)
        {
            
        }

        private void BounceOffWall()
        {
            // no collisions during player move
            if (player.GetSlideCollisionCount() == 0) 
                return;
        
            var collision = player.GetSlideCollision(0);
            Vector2 surfaceNormal = collision.GetNormal();
            float surfaceAngle = Mathf.Abs(90 + Mathf.RadToDeg(surfaceNormal.Angle()));

            // surface is not steep enough or its not a vertical wall
            if (surfaceAngle < 45 && surfaceAngle != 90 && surfaceAngle != 270)
                return;

            // defines the reflected velocity by the surface's normal and loss of energy through player elasticity
            Vector2 reflectedVelocity = player.Velocity.Bounce(surfaceNormal) * player.Elasticity;

            // if we hit a vertical wall apply an impulse
            if (surfaceAngle == 90 || surfaceAngle == 270)
            {
                reflectedVelocity = AdjustVelocityXForVerticalWallBounce(reflectedVelocity, surfaceNormal);
            }

            player.Velocity = reflectedVelocity;
        }

        private Vector2 AdjustVelocityXForVerticalWallBounce(Vector2 reflectedVelocity, Vector2 surfaceNormal)
        {
            // if the reflected velocity in the x direction is too small set it to a minimum predefined value
            if (Mathf.Abs(reflectedVelocity.X) < MIN_BOUNCE_X_VELOCITY_THRESHOLD)
            {
                reflectedVelocity.X = Mathf.Sign(reflectedVelocity.X) * MIN_BOUNCE_X_VELOCITY_THRESHOLD;
            }
            Vector2 impulse = surfaceNormal * IMPULSE_STRENGTH;
            return reflectedVelocity + impulse;         
        }
    }
}
