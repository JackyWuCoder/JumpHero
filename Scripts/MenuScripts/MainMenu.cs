using Godot;
using System;
using System.Linq;

public partial class MainMenu : Control
{
	// Node members
	private static readonly string TITLE_NODE_NAME = "Title";
	private static readonly string CONTINUE_BUTTON_NODE_NAME = "Buttons/Continue";
	private static readonly string NEW_GAME_BUTTON_NODE_NAME = "Buttons/NewGame";
	private static readonly string OPTIONS_BUTTON_NODE_NAME = "Buttons/Options";
	private static readonly string QUIT_BUTTON_NODE_NAME = "Buttons/Quit";

	// Data members
	private MenuManager manager;
	private bool firstLoad = true;

	public override void _Ready()
	{
		Visible = false;
		manager = GetOwner<MenuManager>();
		GetNode<Button>(CONTINUE_BUTTON_NODE_NAME).Connect(Button.SignalName.Pressed, Callable.From(OnContinue));
		GetNode<Button>(NEW_GAME_BUTTON_NODE_NAME).Connect(Button.SignalName.Pressed, Callable.From(OnNewGame));
		GetNode<Button>(OPTIONS_BUTTON_NODE_NAME).Connect(Button.SignalName.Pressed, Callable.From(OnOptions));
		GetNode<Button>(QUIT_BUTTON_NODE_NAME).Connect(Button.SignalName.Pressed, Callable.From(OnQuit));
		Connect(SignalName.VisibilityChanged, Callable.From(OnVisibilityChanged));
	}

	private void OnContinue()
	{
		if (firstLoad) return; // Disable button when its animating in
		// TODO: Implement after file saving system is created, need to reload from player's last checkpoint
		GD.Print("YEAH GOODLUCK WITH THAT BUDDY");
		manager.LoadGameWorld();
	}

	private void OnNewGame()
	{
		if (firstLoad) return; // Disable button when its animating in
		manager.LoadGameWorld(usingSave: false);
	}

	private void OnOptions()
	{
		if (firstLoad) return; // Disable button when its animating in
		// TODO: Implement once majority of game is finished
		GD.Print("OPTIONS WAS PRESSED, HAVE FUN IMPLEMENTING THIS");

		// Uncomment below when options menu is actually added
		// manager.SwitchMenu(MenuManager.OPTIONS_MENU);
	}

	private void OnQuit()
	{
		if (firstLoad) return; // Disable button when its animating in
		GetTree().Quit();
	}

	private void OnVisibilityChanged()
	{
		if (Visible == true && firstLoad) ShowWithAnimation();
	}

	private void ShowWithAnimation()
	{
		// Get node components
		Control title = GetNode<Control>(TITLE_NODE_NAME);
		const string buttonsGroupNodeName = "Buttons";
		Control buttons = GetNode<Control>(buttonsGroupNodeName);

		// Get end points of each element
		Vector2 titleEndpoint = title.Position;
		
		// Change properties of elements for animation
		foreach(Control button in buttons.GetChildren().Cast<Control>())
		{
			button.Scale = Vector2.Zero;
			button.RotationDegrees = -180;
		}
		title.Position = new Vector2(title.Position.X, GetViewportRect().Size.Y);
		title.Scale = Vector2.Zero;
		title.SetProcess(false); // Disable visual effect until after animation is done

		// Create animation
		Tween animation = CreateTween();
		const float tweenTime = 0.2f;
		
		// Overshoot title
		Tween titleScale = CreateTween();
		titleScale.TweenProperty(title, nameof(Scale).ToLower(), new Vector2(0.8f, 1.4f), tweenTime * 2);
		animation.TweenProperty(title, nameof(Position).ToLower(), titleEndpoint + Vector2.Up * 50, tweenTime * 2);

		// Move to endpoint
		titleScale.TweenProperty(title, nameof(Scale).ToLower(), Vector2.One, tweenTime / 2);
		animation.TweenProperty(title, nameof(Position).ToLower(), titleEndpoint, tweenTime);
		animation.TweenCallback(Callable.From(() => title.SetProcess(true)));

		// Animate button popping in
		var buttonsList = buttons.GetChildren();
		for(int i = 0; i < buttonsList.Count; i++)
		{
			animation.TweenProperty(buttonsList[i], nameof(Scale).ToLower(), Vector2.One * 1.4f, tweenTime / 2);
			animation.TweenProperty(buttonsList[i], nameof(Scale).ToLower(), Vector2.One * 1f, tweenTime / 2);

			// Create second tween to spin button while scaling up at the same time
			Tween rotationTween = CreateTween();
			const float titleSequenceDelay = tweenTime * 3;
			rotationTween.TweenInterval(titleSequenceDelay + tweenTime * i);
			rotationTween.TweenProperty(buttonsList[i], nameof(Rotation).ToLower(), 0, tweenTime);
			rotationTween.Play();
		}
		// Enable button snap effects again
		animation.TweenCallback(Callable.From(() =>
		{
			foreach(Button button in buttons.GetChildren().Cast<Button>())
				button.EmitSignal(Button.SignalName.ThemeChanged);
		}));
		animation.TweenCallback(Callable.From(() => firstLoad = false));
		animation.Play();
	}
}
