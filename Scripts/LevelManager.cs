using Godot;
using JumpHero;
using System;

public partial class LevelManager : Node
{
    // Node names
    private static readonly string PLAYER_NODE_NAME = "Player";

    // Data members
    private Player _player;

    // Signals
    [Signal] public delegate void LevelTransitionEventHandler(Player player);
    [Signal] public delegate void WorldTransitionEventHandler(Player player);

	public override void _Ready()
	{
        _player = GetNode<Player>(PLAYER_NODE_NAME);
        _player.Connect(Player.SignalName.OnScreenExit,
            Callable.From(() => {
                CheckNewWorld();
                EmitSignal(SignalName.LevelTransition, _player);
            }
        ));
	}

    private void CheckNewWorld()
    {
        // TODO: Implement when more levels are finished, checks if 5 levels are passed
    }
}
