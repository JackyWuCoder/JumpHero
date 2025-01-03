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
            foreach(Node effect in GetViewport().GetCamera2D().GetChildren())
            {
                if (effect is CameraShake shakeEffect)
                {
                    float fallSpeed = player.GetRealVelocity().Y;
                    shakeEffect.Shake(fallSpeed / Player.FREEFALL_THRESHOLD);
                    break;
                }
            }
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
