using Godot;

namespace JumpHero
{
	public abstract partial class State : Node
	{
		protected Player player;
		protected PlayerStateManager stateManager;
		public override void _Ready() 
		{
			player = GetOwner() as Player;
			stateManager = GetParent() as PlayerStateManager;

			// Disable to let state manager do the processing for states
			SetProcessInput(false);
			SetProcess(false);
			SetPhysicsProcess(false);
		}
		public abstract void EnterState();
		public abstract void ExitState();
		public abstract void Process(double delta);
		public abstract void PhysicsProcess(double delta);
		public abstract void InputProcess(InputEvent inputEvent);
	}
}
