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
		public virtual void Process(double delta) {}
		public virtual void PhysicsProcess(double delta)
		{
			// TODO: Add an environment checker that adjusts player velocity depending on environment hazards
			// Intended to be called by shared physics processes in all states
		}
		public virtual void InputProcess(InputEvent inputEvent) {}
	}
}
