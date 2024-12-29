using Godot;
using System;

public partial class SceneLoadManager : Node
{
	// Public enum for interfacing with this class
	public enum Scene
	{
		INGAME,
		MENUS
	}
	// Singleton
	public static SceneLoadManager Instance { get; private set; }

	// Scene file paths
	private static readonly string GAME_SCENE_PATH = "Scenes/Levels/GameWorld.tscn";
	private static readonly string MENUS_SCENE_PATH = "Scenes/Menus/AllMenus.tscn";
	private static readonly string LOAD_MENU_PATH = "Scenes/Menus/LoadMenu.tscn";

	// Data members
	private LoadMenu _loadingScreen;
	private Node _currentScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
		_currentScene = GetTree().Root.GetChild<Node>(-1); // Current scene always at last of root

		// Get load menu and instantiate it for displaying usage
		PackedScene loadScene = ResourceLoader.Load<PackedScene>(LOAD_MENU_PATH);
		_loadingScreen = loadScene.Instantiate<LoadMenu>();
		GetTree().Root.CallDeferred(MethodName.AddChild, _loadingScreen);
		_loadingScreen.CallDeferred(MethodName.Connect, LoadMenu.SignalName.OnFinishedLoading, 
			Callable.From((string path) => InstantiateScene(path)));
	}

	// This function expects one of the static variables outlined above
	public void TransitionScene(Scene scene)
	{
		// Get scene path from scene enum
		string scenePath = "";
		switch(scene)
		{
			case Scene.INGAME:
				scenePath = GAME_SCENE_PATH;
				break;
			case Scene.MENUS:
				scenePath = MENUS_SCENE_PATH;
				break;
			default:
				GD.PushError($"{scene} has not been added to switch-case");
				break;
		}
		// Need to defer because code might still be running, need to wait for next frame
		CallDeferred(MethodName.StartTransition, scenePath);
	}

	private void StartTransition(string scenePath)
	{
		_currentScene.Free();
		ResourceLoader.LoadThreadedRequest(scenePath);
		_loadingScreen.StartLoadScreen(scenePath);
	}

	private void InstantiateScene(string scenePath)
	{
		// Assumes resource loader will always be a packed scene
		PackedScene nextScene = (PackedScene) ResourceLoader.LoadThreadedGet(scenePath);
		_currentScene = nextScene.Instantiate();
		GetTree().Root.AddChild(_currentScene);
		GetTree().CurrentScene = _currentScene; // Allows for usage with ChangeSceneToFile
	}
}
