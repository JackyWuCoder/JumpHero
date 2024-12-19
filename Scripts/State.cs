using Godot;

namespace JumpHero
{
	public abstract partial class State : Node
	{
		protected Player player;
		public override void _Ready() 
		{
			player = GetOwner() as Player;
		}
		public abstract void EnterState();
		public abstract void ExitState();
		public abstract void Process(double delta);
		public abstract void PhysicsProcess(double delta);
		public abstract void InputProcess(InputEvent inputEvent);
	}
}
