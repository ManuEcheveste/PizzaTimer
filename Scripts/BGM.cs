using Godot;
using System;

public partial class BGM : AudioStreamPlayer2D
{
	[Export] public AudioStream pizzaTime;
	[Export] public AudioStreamPlayer2D bgm;
	[Export] public AudioStream lap2;
	[Export] public AudioStream lap3;
	[Export] public AudioStreamPlayer2D panicbgm;
	public int lap = 0;

	public void NextLap()
	{
		if (lap < 2)
		{
			bgm.Stream = lap2;
			bgm.Play();
			bgm.VolumeDb = 0;
			panicbgm.Stop();
			lap = 2;
		}
		else
		{
			bgm.Stream = lap3;
			bgm.Play();
			lap = 3;
		}
		GD.Print(lap);
	}
}
