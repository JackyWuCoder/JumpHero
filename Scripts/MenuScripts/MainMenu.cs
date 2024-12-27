using Godot;
using System;

public partial class MainMenu : Control
{
	private static readonly string CONTINUE_BUTTON_NODE_NAME = "Buttons/Continue";
	private static readonly string NEW_GAME_BUTTON_NODE_NAME = "Buttons/NewGame";
	private static readonly string OPTIONS_BUTTON_NODE_NAME = "Buttons/Options";
	private static readonly string QUIT_BUTTON_NODE_NAME = "Buttons/Quit";
	private MenuManager manager;
	public override void _Ready()
	{
		manager = GetOwner<MenuManager>();
		GetNode<Button>(CONTINUE_BUTTON_NODE_NAME).Connect(Button.SignalName.Pressed, Callable.From(OnContinue));
		GetNode<Button>(NEW_GAME_BUTTON_NODE_NAME).Connect(Button.SignalName.Pressed, Callable.From(OnNewGame));
		GetNode<Button>(OPTIONS_BUTTON_NODE_NAME).Connect(Button.SignalName.Pressed, Callable.From(OnOptions));
		GetNode<Button>(QUIT_BUTTON_NODE_NAME).Connect(Button.SignalName.Pressed, Callable.From(OnQuit));
	}

	private void OnContinue()
	{
		// TODO: Implement after file saving system is created, need to reload from player's last checkpoint
		GD.Print("YEAH GOODLUCK WITH THAT BUDDY");
		// TODO: Add some sort of file saving such that it saves what world the player was on, then pass to load game world function
		manager.LoadGameWorld(1);
	}

	private void OnNewGame()
	{
		// TODO: Transition to new scene that is the game level
		GD.Print("NEW GAME WAS PRESSED WOW");
		manager.LoadGameWorld(1);
	}

	private void OnOptions()
	{
		// TODO: Implement once majority of game is finished
		GD.Print("OPTIONS WAS PRESSED, HAVE FUN IMPLEMENTING THIS");

		// Uncomment below when options menu is actually added
		// manager.SwitchMenu(MenuManager.OPTIONS_MENU);
	}

	private void OnQuit()
	{
		GetTree().Quit();
	}
}
