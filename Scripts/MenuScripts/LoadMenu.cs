using Godot;
using System;

public partial class LoadMenu : Control
{
	// Node component names
	private static readonly string LOAD_BAR_NODE_NAME = "LoadBar";

	// Signals
	[Signal] public delegate void OnFinishedLoadingEventHandler(string path);

	// Data members
	private ProgressBar _loadBar;
	private Godot.Collections.Array _progress = new(); // ResourceLoader requires a passing of an array of 1 to get the progress
	private bool isLoading = false;
	private string _scenePath;

	public override void _Ready()
	{
		_loadBar = GetNode<ProgressBar>(LOAD_BAR_NODE_NAME);
		ToggleMenu(false);
	}

    public override void _Process(double delta)
    {
		if (!isLoading) return;
    	ResourceLoader.ThreadLoadStatus status = ResourceLoader.LoadThreadedGetStatus(_scenePath, _progress);
		if (status == ResourceLoader.ThreadLoadStatus.Loaded) 
		{
			ToggleMenu(false);
			EmitSignal(SignalName.OnFinishedLoading, _scenePath);
		}
		else _loadBar.Value = (float) _progress[0];
    }

	public void StartLoadScreen(string scenePath)
	{
		_scenePath = scenePath;
		ToggleMenu(true);
	}

	private void ToggleMenu(bool enable)
	{
		SetProcess(enable);
		Visible = enable;
		isLoading = enable;
		_loadBar.Value = 0;
	}
}
