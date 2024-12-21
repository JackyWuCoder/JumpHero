using Godot;
using System;

namespace JumpHero
{
	public partial class Grounded : State
	{
		public override void EnterState()
		{
			player.Velocity = Vector2.Zero;
		}

		public override void ExitState()
		{
			player.StartWalk(false);
		}

		public override void Process(double delta)
		{
			
		}

		public override void PhysicsProcess(double delta)
		{
			if (!player.IsOnFloor()) 
			{
				stateManager.ChangeState(PlayerStateManager.PlayerState.AIRBORNE);
				return;
			}
			ReadInputs();
			player.MoveAndSlide();
		}

		public override void InputProcess(InputEvent inputEvent)
		{
			if (inputEvent.IsAction(ProjectInputs.DOWN))
			{
				// TODO: Decide whether this is even necessary 
			}
			if (inputEvent.IsAction(ProjectInputs.UP))
			{
				// TODO: Decide whether this is even necessary 
			}
			if (inputEvent.IsAction(ProjectInputs.JUMP))
				stateManager.ChangeState(PlayerStateManager.PlayerState.CHARGING);
		}

		private void ReadInputs()
		{
			int moveDirection = 0;
			if (Input.IsActionPressed(ProjectInputs.LEFT)) moveDirection -= 1;
			if (Input.IsActionPressed(ProjectInputs.RIGHT)) moveDirection += 1;
			player.Velocity = Vector2.Right * moveDirection * player.MoveSpeed;
			
			if (moveDirection != 0) 
			{
				player.SetDirection(moveDirection == 1);
				player.StartWalk(true);
			} 
			else player.StartWalk(false);
		}
	}
}
