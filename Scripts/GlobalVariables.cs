using Godot;
using System;

// Auto-loads in Godot require class to inherit from Node
public partial class GlobalVariables : Node
{
	public static GlobalVariables Instance { get; private set; }
	public int World { get; private set; } = 1;

    public override void _Ready()
    {
        Instance = this;
    }

    public void LoadSaved()
	{
		// TODO: Implement loading of any saves here, and change any global variables such as settings here
	}

	public void ClearSaveFile()
	{
		// TODO: Reset all variables and re-write save file with blank slate
		World = 1;
	}
}
