using Godot;
using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace JumpHero
{
	public partial class SoundManager : AudioStreamPlayer2D
	{
		[Export] protected Godot.Collections.Array<AudioStream> audioFiles;
		protected Godot.Collections.Dictionary<string, AudioStream> audioMap = new();
		protected Queue audioQueue = new();

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			// Sets file name as key and audio stream as value
			// Names for programming will need to be held in an external static class
			foreach(AudioStream audio in audioFiles) 
				audioMap.Add(ExtractResourceName(audio.ResourcePath), audio);

			// Add signal
			Connect(SignalName.Finished, Callable.From(() => PlayNextQueued()));
		}

		public void QueueAudio(string audioName)
		{
			if (!audioMap.ContainsKey(audioName))
			{
				GD.PushError($"Error: {audioName} does not exist in the audio map");
				return;
			}
			if (Playing) audioQueue.Enqueue(audioMap[audioName]);
			else SwitchStreamAndPlay(audioName);
		}

		public void PlayAudio(string audioName)
		{
			// check if audio name is valid
			if (!audioMap.ContainsKey(audioName))
				GD.PushError($"Error: {audioName} does not exist in the audio map");
			else SwitchStreamAndPlay(audioName);
		}

		private void SwitchStreamAndPlay(string audioName)
		{
			Stream = audioMap[audioName];
			PitchScale = (float) GD.RandRange(0.95f, 1.05f); // Adds slight variation to sound clips
			Play();
		}

		private void PlayNextQueued()
		{
			if (audioQueue.Count == 0) return;
			AudioStream audioClip = audioQueue.Dequeue() as AudioStream;
			Stream = audioClip;
			Play();
		}

		// Required since .ResourceName is returning null for some reason
		private string ExtractResourceName(string filePath)
		{
			// Extracts file name from path without the extension
			const string regexPattern = @"([^\/]+)(?=\.\w+$)";
			return Regex.Match(filePath, regexPattern).Value;
		}
	}
}
