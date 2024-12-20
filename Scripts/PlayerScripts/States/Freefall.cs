using Godot;
using System;

namespace JumpHero
{
	public partial class Freefall : State
	{
        public override void EnterState()
        {
            // TODO: Link up sprite to make it spin in mid-air to simulate losing control in the air
        }

        public override void ExitState()
        {
            // TODO: Link up GPUParticle emitter to release particles to simulate hard landing
        }

        public override void Process(double delta)
        {
            
        }

        public override void InputProcess(InputEvent inputEvent)
        {
            
        }
    }
}
