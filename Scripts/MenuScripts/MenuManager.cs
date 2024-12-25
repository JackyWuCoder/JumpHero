using Godot;
using System;
using System.Linq;

public partial class MenuManager : Control
{
	// Static constants to be called by each menu screen's manager
	public static readonly string MAIN_MENU = "MainMenu";
	public static readonly string OPTIONS_MENU = "OptionsMenu";
	public static readonly string INGAME_MENU = "InGameMenu";
	private static readonly string LOADING_MENU = "LoadMenu"; // Only used by menu manager when loading resources (i.e. the level)
	private static readonly string GAME_SCENE_PATH = "Scenes/Levels/GameWorld.tscn";

	// Data members
	private LoadMenu _loadScreen;
	
    public override void _Ready()
    {
        SwitchMenu(MAIN_MENU); // On first boot-up, go to main menu
		_loadScreen = GetNode<LoadMenu>(LOADING_MENU);
		_loadScreen.Connect(LoadMenu.SignalName.OnFinishedLoading, Callable.From((string path) => TransitionScene(path)));
    }
    /*
		Menus are expected to use one of the static variables, each menu also has to match name with static constants.
		If after calling this function causes all menu elements to disappear, then name configuration on the
		menu root node is wrong and needs to be changed to the above static constants.
	*/
    public void SwitchMenu(string nextMenu)
	{
		foreach(Control menu in GetChildren().Cast<Control>())
		{
			if (menu.Name == nextMenu) EnableMenu(menu, true);
			else EnableMenu(menu, false);
		}
	}

	public void LoadGameWorld(int world = 1)
	{
		// TODO: Move the player to whatever the world passed in was, for now let it do nothing
		ResourceLoader.LoadThreadedRequest(GAME_SCENE_PATH);
		_loadScreen.StartLoadScreen(GAME_SCENE_PATH);
		SwitchMenu(LOADING_MENU);
	}

	public void TransitionScene(string resourcePath)
	{
		// TODO: Find a better way of managing different scenes that's not done in the menus
		PackedScene scene = (PackedScene) ResourceLoader.LoadThreadedGet(resourcePath);
		GetTree().ChangeSceneToPacked(scene);
	}

	private void EnableMenu(Control menu, bool enable)
	{
		menu.Visible = enable;
		menu.SetProcess(enable);
		menu.SetProcessInput(enable);
	}
}
