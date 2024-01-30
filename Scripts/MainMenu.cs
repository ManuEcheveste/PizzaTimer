using Godot;
using System;
using System.Linq.Expressions;

public partial class MainMenu : Button
{
	[Export] public SpinBox hoursText;
	[Export] public SpinBox secondsText;
	[Export] public SpinBox minutesText;
	[Export] public CheckButton musicButton;
	[Export] public CheckButton crashButton;
	[Export] public AnimationPlayer timerSpawn;
	[Export] public AudioStreamPlayer2D johnOST;
	[Export] public AudioStream jonhGutted;
	[Export] public AudioStreamPlayer2D bgm;
	[Export] public TimerScript timer;
	[Export] public AnimationPlayer johnPillar;
	[Export] public PackedScene pizzaTime;
	public float hours;
	public float minutes;
	public float seconds;
	public float maxTime;
	public bool music;
	public bool crash;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Started");
		johnPillar.Play("JohnIdle");
		var config = new ConfigFile();
		Error err = config.Load("user://config.cfg");
		if (err != Error.Ok)
		{
			config.SetValue("Timer", "seconds", 30);
			config.SetValue("Timer", "minutes", 1);
			config.SetValue("Timer", "hours", 0);
			config.SetValue("Settings", "music", true);
			config.SetValue("Settings", "crash", true);
			config.Save("user://config.cfg");
		}
		else
		{
			secondsText.Value = (float)config.GetValue("Timer", "seconds");
			minutesText.Value = (float)config.GetValue("Timer", "minutes");
			hoursText.Value = (float)config.GetValue("Timer", "hours");
			musicButton.ButtonPressed = (bool)config.GetValue("Settings", "music");
			crashButton.ButtonPressed = (bool)config.GetValue("Settings", "crash");
			music = (bool)config.GetValue("Settings", "music");
			crash = (bool)config.GetValue("Settings", "crash");
			GD.Print("Crash value: " + crash);
			if (music == false)
			{
				var sfx_index = AudioServer.GetBusIndex("Music");
				AudioServer.SetBusVolumeDb(sfx_index, -80);
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_john_pressed()
	{
		GD.Print("It's Pizza Time!");
		seconds = (float)secondsText.Value;
		minutes = (float)minutesText.Value;
		hours = (float)hoursText.Value;
		maxTime = hours * 3600;
		maxTime += minutes * 60;
		maxTime += seconds;
		var config = new ConfigFile();
		config.SetValue("Timer", "seconds", seconds);
		config.SetValue("Timer", "minutes", minutes);
		config.SetValue("Timer", "hours", hours);
		config.SetValue("Settings", "music", music);
		config.SetValue("Settings", "crash", crash);
		config.Save("user://config.cfg");
		timerSpawn.Play("TimerSpawn");
		johnOST.Stop();
		johnOST.Bus = "Master";
		johnOST.Stream = jonhGutted;
		johnOST.Play();
		bgm.Play();
		timer.timer = maxTime;
		timer.maxTime = maxTime;
		timer.pizzaTime = true;
		timer.ItsPizzaTime();
		DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, true);
		DisplayServer.WindowSetSize(new Vector2I(450, 140));
		Vector2I winPos = GetWindow().Position;
		DisplayServer.WindowSetPosition(new Vector2I(winPos.X, winPos.Y + 160));
		PizzaTime scene = (PizzaTime)pizzaTime.Instantiate();
		GetTree().Root.GetChild(0).AddChild(scene);
		scene.Position = new Vector2I(DisplayServer.WindowGetPosition().X + 100, DisplayServer.WindowGetPosition().Y + 500);
	}

	public void _on_mouse_entered()
	{
		johnPillar.Play("Scared");
	}

	public void _on_mouse_exited()
	{
		johnPillar.Play("JohnIdle");
	}

	public void _on_music_toggled(bool toggled)
	{
		if (toggled == false)
		{
			var sfx_index = AudioServer.GetBusIndex("Music");
			AudioServer.SetBusVolumeDb(sfx_index, -80);
			music = false;
		}
		else
		{
			var sfx_index = AudioServer.GetBusIndex("Music");
			AudioServer.SetBusVolumeDb(sfx_index, 0);
			music = true;
		}
	}
	
	public void _on_crash_toggled(bool toggled)
	{
		if(toggled == false)
		{
			crash = false;
		}
		else
		{
			crash = true;
		}
	}
}
