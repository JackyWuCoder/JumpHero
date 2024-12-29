using Godot;
using JumpHero;
using System;
using System.Linq;

public partial class PauseMenu : Control
{
	private static readonly string BACKGROUND_NODE_NAME = "MenuBackground";
	private static readonly string PAUSE_LABEL_NODE_NAME = "Pause";
	private static readonly string BUTTONS_GROUP_NODE_NAME = "Buttons";
	private static readonly string CONTINUE_BUTTON_NODE_NAME = "Buttons/Continue";
	private static readonly string SAVE_BUTTON_NODE_NAME = "Buttons/Save";
	private static readonly string OPTIONS_BUTTON_NODE_NAME = "Buttons/Options";
	private static readonly string QUIT_BUTTON_NODE_NAME = "Buttons/Quit";
	private bool _isAnimating = false; // Used to tell if animation is still playing
	private Control _pauseLabel;
	private Control _menuBackground;
	private Control _buttonsGroup;

	public override void _Ready()
	{
		// Set required processing modes
		ProcessMode = ProcessModeEnum.Always;
		Visible = false;
		_buttonsGroup = GetNode<Control>(BUTTONS_GROUP_NODE_NAME);
		_buttonsGroup.ProcessMode = ProcessModeEnum.Disabled;
		_menuBackground = GetNode<Control>(BACKGROUND_NODE_NAME);
		_pauseLabel = GetNode<Control>(PAUSE_LABEL_NODE_NAME);

		// Connect button signals
		GetNode<Button>(CONTINUE_BUTTON_NODE_NAME).Connect(Button.SignalName.Pressed, Callable.From(OnContinue));
		GetNode<Button>(SAVE_BUTTON_NODE_NAME).Connect(Button.SignalName.Pressed, Callable.From(OnSave));
		GetNode<Button>(OPTIONS_BUTTON_NODE_NAME).Connect(Button.SignalName.Pressed, Callable.From(OnOptions));
		GetNode<Button>(QUIT_BUTTON_NODE_NAME).Connect(Button.SignalName.Pressed, Callable.From(OnQuit));
	}

    public override void _Input(InputEvent @event)
    {
		if (_isAnimating) return;
		if (Input.IsActionJustPressed(ProjectInputs.PAUSE))
		{
			if (!Visible) PauseWithAnimation();
			else ResumeWithAnimation();
		}
    }

    private void OnContinue()
	{
		ResumeWithAnimation();
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
		GetTree().Paused = false;
		SceneLoadManager.Instance.TransitionScene(SceneLoadManager.Scene.MENUS, true);
	}

	// Pause screen animations section
	private void PauseWithAnimation()
	{
		_isAnimating = true;
		GetTree().Paused = true;
		// Get endpoints for each element
		Vector2 labelEndpoint = _pauseLabel.Position, backgroundEndpoint = _menuBackground.Position;

		// Position UI out of viewport
		_menuBackground.Position = new Vector2(-_menuBackground.Size.X, _menuBackground.Position.Y);
		_menuBackground.RotationDegrees = 7.7f;
		_pauseLabel.Position = new Vector2(_pauseLabel.Position.X, -_pauseLabel.Size.Y);

		// Scale button elements down to hide
		foreach(Button button in _buttonsGroup.GetChildren().Cast<Button>()) 
			button.Scale = Vector2.Zero;
		
		Visible = true; // Toggle to show animation
		// Create tween to move back into screen
		const float animationTime = 0.1f;
		Tween animation = CreateTween();
		animation.TweenProperty(_menuBackground, nameof(Position).ToLower(), backgroundEndpoint, animationTime);
		animation.TweenProperty(_menuBackground, nameof(Rotation).ToLower(), 0, animationTime);
		animation.TweenProperty(_pauseLabel, nameof(Position).ToLower(), labelEndpoint, animationTime);
		foreach(Button button in _buttonsGroup.GetChildren().Cast<Button>())
		{
			animation.TweenProperty(button, nameof(Scale).ToLower(), Vector2.One * 1.2f, animationTime / 2.5f);
			animation.TweenProperty(button, nameof(Scale).ToLower(), Vector2.One * 1f, animationTime / 2.5f);
		}
		
		// Setup callback
		animation.TweenCallback(Callable.From(() => 
		{
			_buttonsGroup.ProcessMode = ProcessModeEnum.WhenPaused;
			_isAnimating = false;
		}));
		animation.Play();
	}

	private void ResumeWithAnimation()
	{
		_isAnimating = true;
		float endHeight = GetViewportRect().Size.Y;
		
		// Revert button snap rotations
		foreach(Button button in _buttonsGroup.GetChildren())
			button.EmitSignal(Button.SignalName.MouseExited);

		// Slide pause window downwards
		const float animationTime = 0.2f;
		Tween animation = CreateTween();
		animation.TweenProperty(this, nameof(Position).ToLower(), new Vector2(0, endHeight), animationTime);
		animation.TweenCallback(Callable.From(() =>
		{
			_isAnimating = false;
			_buttonsGroup.ProcessMode = ProcessModeEnum.Pausable;
			Visible = false;
			Position = Vector2.Zero;
			GetTree().Paused = false;
		}));
		animation.Play();
	}
}
