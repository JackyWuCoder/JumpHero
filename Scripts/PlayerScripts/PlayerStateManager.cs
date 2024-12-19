using Godot;
using System;

namespace JumpHero
{
	public partial class PlayerStateManager : Node
	{
		[Signal] public delegate void OnPlayerStateChangeEventHandler();
		public enum PlayerState 
		{
			AIRBORNE,
			FREEFALL,
			GROUNDED
		}
		public PlayerState State { get; private set; } = PlayerState.GROUNDED;
		private State _currentState = null;
		
		public override void _Ready()
		{
			_currentState = GetState(PlayerState.GROUNDED);
		}

        public override void _Process(double delta)
        {
            _currentState.Process(delta);
        }

        public override void _PhysicsProcess(double delta)
        {
            _currentState.PhysicsProcess(delta);
        }

        public override void _Input(InputEvent @event)
        {
            _currentState.InputProcess(@event);
        }

        public void ChangeState(PlayerState state)
		{
			if (this.State == state) return;
			_currentState.ExitState();
			State newState = GetState(state);
			newState.EnterState();
			this.State = state;
			_currentState = newState;
			EmitSignal(SignalName.OnPlayerStateChange);
		}

		private State GetState(PlayerState state)
		{
			string targetState = state.ToString().Capitalize();
			foreach(Node child in GetChildren())
			{
				if (child.Name == targetState) return child as State;
			}
			GD.PushError($"ERROR: passed in argument {state} does not have a corresponding IState object");
			return null;
		}
	}
}
