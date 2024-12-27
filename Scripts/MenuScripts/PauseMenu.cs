using Godot;
using JumpHero;
using System;

public partial class PauseMenu : Control
{
	private static readonly string CONTINUE_BUTTON_NODE_NAME = "Continue";
	private static readonly string SAVE_BUTTON_NODE_NAME = "Save";
	private static readonly string OPTIONS_BUTTON_NODE_NAME = "Options";
	private static readonly string QUIT_BUTTON_NODE_NAME = "Quit";
	public override void _Ready()
	{
		Visible = false;
		GetNode<Button>(CONTINUE_BUTTON_NODE_NAME).Connect(Button.SignalName.Pressed, Callable.From(OnContinue));
		GetNode<Button>(SAVE_BUTTON_NODE_NAME).Connect(Button.SignalName.Pressed, Callable.From(OnSave));
		GetNode<Button>(OPTIONS_BUTTON_NODE_NAME).Connect(Button.SignalName.Pressed, Callable.From(OnOptions));
		GetNode<Button>(QUIT_BUTTON_NODE_NAME).Connect(Button.SignalName.Pressed, Callable.From(OnQuit));
	}

    public override void _Input(InputEvent @event)
    {
        if (!Input.IsActionPressed(ProjectInputs.PAUSE)) return;
		GetTree().Paused = true;
		Visible = true;
    }

    private void OnContinue()
	{
		GetTree().Paused = false;
	}

	private void OnSave()
	{
		// TODO: Implement saving
	}

	private void OnOptions()
	{
		// TODO: Implement last, when options menu is made
	}

	private void OnQuit()
	{
		// TODO: Go back to main menu
	}
}
