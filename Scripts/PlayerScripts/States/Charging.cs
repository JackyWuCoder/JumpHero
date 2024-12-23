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
        private bool _isCharging = false;
        private ProgressBar _chargeBar;

		public override void EnterState()
		{
            player.Velocity = Vector2.Zero;
			_jumpXComponent = STARTING_X_COMPONENT;
			_jumpYComponent = STARTING_Y_COMPONENT;

            ResetChargeBarValues();
            UpdateChargeBarPosition();
        }

        public override void ExitState()
        {
			int jumpDirection = player.IsFacingRight ? 1 : -1;
			player.Velocity = Vector2.Up * _jumpYComponent + Vector2.Right * jumpDirection * _jumpXComponent;

            // Hide charge bar UI
            if (_chargeBar != null)
                _chargeBar.Visible = false;

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

            UpdateChargeBarUI();

			player.EmitChargePercentage(_jumpXComponent / MAX_X_COMPONENT);
		}

        private void ResetChargeBarValues()
        {
            _chargeBar = GetNode<ProgressBar>("/root/TestWorld/Player/ProgressBar");
            if (_chargeBar == null)
                GD.PrintErr("Charge bar not found!");
            else
            {
                _chargeBar.Visible = true;
                _chargeBar.Value = 0;
            }
        }

        private void UpdateChargeBarUI()
        {
            if (_chargeBar != null)
            {
                _chargeBar.Value = _jumpXComponent / MAX_X_COMPONENT * _maxCharge;
                _chargeBar.Modulate = GetInterpolatedColor((float)_chargeBar.Value);
            }
        }

        private void UpdateChargeBarPosition()
        {
            if (_chargeBar != null && player != null)
            {
                _chargeBar.Position = new Vector2(0, 0);
                float offsetX = 69f;
                float offsetY = 25f;
                if (player.IsFacingRight)
                    _chargeBar.Position += new Vector2(offsetX, -offsetY);
                else
                    _chargeBar.Position += new Vector2(-offsetX, -offsetY);
            }
        }

        private Color GetInterpolatedColor(float percentage)
        {
            Color startColor = new Color(0, 1, 0); // Green
            Color middleColor = new Color(1, 1, 0); // Yellow
            Color endColor = new Color(1, 0, 0); // Red

            if (percentage < 50f)
                return startColor.Lerp(middleColor, percentage / 50f);
            else 
                return middleColor.Lerp(endColor, (percentage - 50) / 50f);
        }
    }
}
