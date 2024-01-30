using Godot;
using System;

public partial class BGM : AudioStreamPlayer2D
{
	[Export] public AudioStream pizzaTime;
	[Export] public AudioStreamPlayer2D bgm;
	[Export] public AudioStream lap2intro;
	[Export] public AudioStream lap2;
	[Export] public AudioStream lap3intro;
	[Export] public AudioStream lap3;
	[Export] public AudioStreamPlayer2D panicbgm;
	public int lap = 0;



	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_finished()
	{
		if (lap == 0)
		{
			lap = 1;
			bgm.Stream = pizzaTime;
			bgm.Play();
		}
		if (lap == 2)
		{
			bgm.Stream = lap2;
			bgm.Play();
		}
		if (lap == 3)
		{
			bgm.Stream = lap3;
			bgm.Play();
		}
	}

	public void NextLap()
	{
		if (lap < 2)
		{
			bgm.Stream = lap2intro;
			bgm.Play();
			bgm.VolumeDb = 0;
			panicbgm.Stop();
			lap = 2;
		}
		else
		{
			bgm.Stream = lap3intro;
			bgm.Play();
			lap = 3;
		}
		GD.Print(lap);
	}
}
