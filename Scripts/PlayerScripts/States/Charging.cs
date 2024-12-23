using Godot;
using System;

namespace JumpHero
{
    public partial class Charging : State
    {
        private static readonly float STARTING_X_COMPONENT = 75f;
        private static readonly float STARTING_Y_COMPONENT = 75f;
        private static readonly float MAX_Y_COMPONENT = 600f;
        private static readonly float MAX_X_COMPONENT = 450f;
        private float _jumpXComponent = STARTING_X_COMPONENT;
        private float _jumpYComponent = STARTING_Y_COMPONENT;

        // Charge bar UI
        private float _maxCharge = 100f;
        private float _chargeSpeed = 50f;
        private float _currentCharge = 0f;
        private bool _isCharging = false;
        private ProgressBar _chargeBar;

		public override void EnterState()
		{
            player.Velocity = Vector2.Zero;
			_jumpXComponent = STARTING_X_COMPONENT;
			_jumpYComponent = STARTING_Y_COMPONENT;

            // Get a reference to the charge bar
            _chargeBar = GetNode<ProgressBar>("/root/TestWorld/CanvasLayer/ProgressBar");
            if (_chargeBar == null)
                GD.PrintErr("Charge bar not found!");
            else 
                _chargeBar.Value = _currentCharge;
        }

        public override void ExitState()
        {
			int jumpDirection = player.IsFacingRight ? 1 : -1;
			player.Velocity = Vector2.Up * _jumpYComponent + Vector2.Right * jumpDirection * _jumpXComponent;
			player.NotifyJump();
			player.EmitChargePercentage(0);
        }

        public override void InputProcess(InputEvent inputEvent)
        {
			if (inputEvent.IsActionPressed(ProjectInputs.LEFT)) player.SetDirection(false);
			else if (inputEvent.IsActionPressed(ProjectInputs.RIGHT)) player.SetDirection(true);
        }

        public override void PhysicsProcess(double delta)
        {
			base.PhysicsProcess(delta);
			player.MoveAndSlide();
			
            if (!player.IsOnFloor())
			{
				stateManager.ChangeState(PlayerStateManager.PlayerState.AIRBORNE);
				return;
			}
			if (Input.IsActionJustReleased(ProjectInputs.JUMP)) 
				stateManager.ChangeState(PlayerStateManager.PlayerState.AIRBORNE);
			ChargeJump((float) delta);
        }

        public override void Process(double delta)
        {
            
        }

		private void ChargeJump(float delta)
		{
			const float chargeRate = 1f;
			_jumpXComponent += 	chargeRate * MAX_X_COMPONENT * delta;
			_jumpYComponent += chargeRate * MAX_Y_COMPONENT * delta;
			if (_jumpXComponent > MAX_X_COMPONENT) _jumpXComponent = MAX_X_COMPONENT;
			if (_jumpYComponent > MAX_Y_COMPONENT) _jumpYComponent = MAX_Y_COMPONENT;

            // Update the charge bar UI
            if (_chargeBar != null)
                _chargeBar.Value = _jumpXComponent / MAX_X_COMPONENT * _maxCharge;

			player.EmitChargePercentage(_jumpXComponent / MAX_X_COMPONENT);
		}
    }
}
