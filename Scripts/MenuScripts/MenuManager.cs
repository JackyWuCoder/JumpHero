using Godot;
using System;
using System.Linq;

public partial class MenuManager : Control
{
	// Static constants to be called by each menu screen's manager
	public static readonly string MAIN_MENU = "MainMenu";
	public static readonly string OPTIONS_MENU = "OptionsMenu";
	public static readonly string INGAME_MENU = "InGameMenu";

    public override void _Ready()
    {
        SwitchMenu(MAIN_MENU); // On first boot-up, go to main menu
    }
    /*
		Menus are expected to use one of the static variables, each menu also has to match name with static constants.
		If after calling this function causes all menu elements to disappear, then name configuration on the
		menu root node is wrong and needs to be changed to the above static constants.
	*/
    public void SwitchMenu(string nextMenu)
	{
		foreach(Node child in GetChildren())
			if (child is Control menu) EnableMenu(menu, menu.Name == nextMenu);
	}

	public void LoadGameWorld(bool usingSave = false)
	{
		// TODO: Move the player to whatever the world passed in was, for now let it do nothing
		if (!usingSave) GlobalVariables.Instance.ClearSaveFile();
		SceneLoadManager.Instance.TransitionScene(SceneLoadManager.Scene.INGAME);
	}

	private void EnableMenu(Control menu, bool enable)
	{
		menu.Visible = enable;
		menu.SetProcess(enable);
		menu.SetProcessInput(enable);
	}
}
