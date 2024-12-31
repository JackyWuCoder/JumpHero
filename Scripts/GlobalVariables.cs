using Godot;
using System;

// Auto-loads in Godot require class to inherit from Node
public partial class GlobalVariables : Node
{
	public static GlobalVariables Instance { get; private set; }

	// Save game properties
	public int World = 1;
	
	// Default values for non-boolean options
	private static readonly Vector2 DEFAULT_RESOLUTION = new Vector2(1152, 648);
	private static readonly float DEFAULT_SFX_VOLUME = 1;
	private static readonly float DEFAULT_MUSIC_VOLUME = 1;
	private static readonly float DEFAULT_SCREEN_SHAKE_AMOUNT = 1;

	// Options menu properties (outlined below are the default properties)
	// TODO: Add more option menu customization if necessary
	public Vector2 Resolution = DEFAULT_RESOLUTION;
	public DisplayServer.WindowMode WindowMode = DisplayServer.WindowMode.Windowed; // maybe set to fullscreen later as default
	public bool Borderless = false; // TODO: Borderless window mode
	public float SFXVolume = DEFAULT_SFX_VOLUME; // Volume factor for sound effects
	public float MusicVolume = DEFAULT_MUSIC_VOLUME; // Volume factor for music
	public bool ShowJumpChargeBar = false; // Display the charge bar
	public bool AutoSave = false; // Autosave upon reaching a new world instead of manual saving
	public bool EnableCheckpoints = false; // Allows usage of a checkpoint system (i.e. teleport to most furthest checkpoint, easy mode)
	public bool AutoSkipCutscenes = false; // Skip cutscenes if false (hopefully implemented)
	public bool ShowTimer = false; // Always shows a timer at the top (for speedrunners)
	public bool SpeedrunnerMode = false; // When loading into game, will override whatever the option was set for show timer and auto skip cutscene to true
	public float ScreenShakeAmount = DEFAULT_SCREEN_SHAKE_AMOUNT; // Shake screen when landing from a long fall (TODO: Implement this on camera)

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

	// Expected to be used in options menu for button to reset to default
	public void ResetOptionsToDefault()
	{
		Resolution = DEFAULT_RESOLUTION;
		WindowMode = DisplayServer.WindowMode.Windowed;
		Borderless = false;
		SFXVolume = DEFAULT_SFX_VOLUME;
		MusicVolume = DEFAULT_MUSIC_VOLUME;
		ShowJumpChargeBar = false;
		AutoSave = false;
		EnableCheckpoints = false;
		AutoSkipCutscenes = false;
		ShowTimer = false;
		SpeedrunnerMode = false;
		ScreenShakeAmount = DEFAULT_SCREEN_SHAKE_AMOUNT;
	}
}
