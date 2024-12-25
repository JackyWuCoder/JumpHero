using Godot;
using System;

public partial class LoadMenu : Control
{
	// Node component names
	private static readonly string LOAD_BAR_NODE_NAME = "LoadBar";

	// Signals
	[Signal] public delegate void OnFinishedLoadingEventHandler(string path);

	// Data members
	private MenuManager manager;
	private ProgressBar _loadBar;
	// ResourceLoader requires a passing of an array of 1 to get the progress
	private Godot.Collections.Array _progress = new();
	private bool isLoading = false;
	private string _scenePath = "";

	public override void _Ready()
	{
		manager = GetOwner<MenuManager>();
		_loadBar = GetNode<ProgressBar>(LOAD_BAR_NODE_NAME);
		_loadBar.Value = 0;
	}

    public override void _Process(double delta)
    {
		if (!isLoading) return;
    	ResourceLoader.ThreadLoadStatus status = ResourceLoader.LoadThreadedGetStatus(_scenePath, _progress);
		if (status == ResourceLoader.ThreadLoadStatus.Loaded) 
		{
			manager.TransitionScene(_scenePath);
			_loadBar.Value = 0;
			isLoading = false;
		}
		else _loadBar.Value = (float) _progress[0];
    }

	public void StartLoadScreen(string scenePath)
	{
		_scenePath = scenePath;
		isLoading = true;
	}
}
