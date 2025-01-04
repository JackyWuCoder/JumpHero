using Godot;
using System;

public partial class SplashMenu : Control
{
	private static readonly string LOGO_NODE_NAME = "Logo";
	private static readonly string AUDIO_NODE_NAME = "TrashSounds";
	private Control _logo;
	private MenuManager manager;
	public override void _Ready()
	{
		manager = GetOwner<MenuManager>();
		_logo = GetNode<Control>(LOGO_NODE_NAME);
		Connect(SignalName.VisibilityChanged, Callable.From(OnVisibilityChanged));
		_logo.Modulate = new Color("ffffff00"); // Transparent
		_logo.GetNode<AnimatedSprite2D>("StudioLogo").Play("Fly");
	}

	private void OnVisibilityChanged()
	{
		if (!Visible)
		{
			GlobalVariables.Instance.FirstLaunch = false;
			return;
		}
		// Create logo fade-in then out
		const float fadeTime = 1.1f, delay = 0.75f;
		Tween fade = CreateTween();
		fade.TweenInterval(delay);
		fade.TweenProperty(_logo, nameof(Modulate).ToLower(), new Color("ffffff"), fadeTime); // Fade in
		fade.TweenCallback(Callable.From(() => GetNode<AudioStreamPlayer>(AUDIO_NODE_NAME).Play()));
		fade.TweenInterval(2.25f);
		fade.TweenProperty(_logo, nameof(Modulate).ToLower(), new Color("ffffff00"), fadeTime); // Fade out
		fade.TweenInterval(delay);
		fade.TweenCallback(Callable.From(() => manager.SwitchMenu(MenuManager.MAIN_MENU)));
		fade.Play();
	}
}
