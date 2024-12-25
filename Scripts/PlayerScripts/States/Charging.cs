using Godot;
using System;

namespace JumpHero
{
    public partial class Charging : State
    {
        private float _jumpXComponent = Player.MIN_JUMP_WIDTH;
        private float _jumpYComponent = Player.MIN_JUMP_HEIGHT;

        // Charge bar UI
        private float _maxCharge = 100f;
        private bool _isCharging = false;
        private ProgressBar _chargeBar;

		public override void EnterState()
		{
            player.Velocity = Vector2.Zero;
			_jumpXComponent = Player.MIN_JUMP_WIDTH;
			_jumpYComponent = Player.MIN_JUMP_HEIGHT;

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
			{
				stateManager.ChangeState(PlayerStateManager.PlayerState.AIRBORNE);
				return;
			}
			ChargeJump((float) delta);
        }

        public override void Process(double delta)
        {
            
        }

		private void ChargeJump(float delta)
		{
			const float chargeRate = 1f;
			_jumpXComponent += 	chargeRate * Player.MAX_JUMP_WIDTH * delta;
			_jumpYComponent += chargeRate * Player.MAX_JUMP_HEIGHT * delta;
			if (_jumpXComponent > Player.MAX_JUMP_WIDTH) _jumpXComponent = Player.MAX_JUMP_WIDTH;
			if (_jumpYComponent > Player.MAX_JUMP_HEIGHT) _jumpYComponent = Player.MAX_JUMP_HEIGHT;

            UpdateChargeBarUI();

			player.EmitChargePercentage(_jumpXComponent / Player.MAX_JUMP_WIDTH);
		}

        private void ResetChargeBarValues()
        {
            _chargeBar = GetNode<ProgressBar>("../../ProgressBar");
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
                _chargeBar.Value = _jumpXComponent / Player.MAX_JUMP_WIDTH * _maxCharge;
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
