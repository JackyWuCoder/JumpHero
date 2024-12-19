using Godot;
using System;

namespace JumpHero
{
	public partial class PlayerStateManager : Node
	{
		public enum PlayerState 
		{
			AIRBORNE,
			FREEFALL,
			GROUNDED,
			CHARGING
		}
		public PlayerState State { get; private set; } = PlayerState.GROUNDED;
		private State _currentState = null;
		private Player _player;
		
		public override void _Ready()
		{
			_player = GetOwner() as Player;
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

        public void ChangeState(PlayerState newState)
		{
			if (State == newState) return;

			_currentState.ExitState();
			State stateObject = GetState(newState);
			stateObject.EnterState();
			_currentState = stateObject;
			
			PlayerState oldState = State;
			State = newState;
			_player.NotifyStateChange(oldState, newState);
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
