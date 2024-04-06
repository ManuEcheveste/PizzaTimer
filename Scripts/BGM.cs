using Godot;
using System;

public partial class BGM : AudioStreamPlayer2D
{
	[Export] public AudioStreamPlayer2D bgm;
	[Export] public AudioStream lap2;
	[Export] public AudioStream lap3;
	[Export] public AudioStream lap2N;
	[Export] public AudioStream lap3N;
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
		public void NextLapN()
	{
		if (lap < 2)
		{
			bgm.Stream = lap2N;
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
