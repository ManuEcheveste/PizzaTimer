using Godot;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

public partial class MainMenu : Button
{
	[Export] public bool isWindows;
	[Export] public SpinBox hoursText;
	[Export] public SpinBox secondsText;
	[Export] public SpinBox minutesText;
	[Export] public CheckButton musicButton;
	[Export] public CheckButton crashButton;
	[Export] public Button crashOptionsButton;
	[Export] public CheckButton shutdownButton;
	[Export] public CheckButton lap3SpawnButton;
	[Export] public CheckButton slowPizzaFaceButton;
	[Export] public CheckButton spawnPizzaFaceButton;
	[Export] public CheckButton welcomeButton;
	[Export] public Window crashWindow;
	[Export] public Window welcomeWindow;
	[Export] public Window settingsWindow;
	[Export] public AnimationPlayer timerSpawn;
	[Export] public AudioStreamPlayer2D johnOST;
	[Export] public AudioStream jonhGutted;
	[Export] public AudioStreamPlayer2D bgm;
	[Export] public TimerScript timer;
	[Export] public AnimationPlayer johnPillar;
	[Export] public PackedScene pizzaTime;
	[Export] public AudioStreamPlayer2D noiseSFX;
	[Export] public AudioStream noise01;
	[Export] public AudioStream noise02;
	[Export] public AudioStream noise03;
	[Export] public AudioStream noise04;
	[Export] public AudioStream noise05;
	[Export] public AudioStream noise06;
	public float hours;
	public float minutes;
	public float seconds;
	public float maxTime;
	public bool music;
	public bool crash;
	private bool shutdown;
	private bool lap3Spawn;
	private bool slowPizzaFace;
	private bool spawnPizzaFace;
	private bool customCMD;
	private string command;
	private bool showWelcome;
	[Export] public float version;
	private float reportedVersion;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Started");
		johnPillar.Play("JohnIdle");
		var config = new ConfigFile();
		Error err = config.Load("user://config.cfg");
		if (err != Error.Ok)
		{
			GD.Print("Could not find any config file. Creating one");
			CreateFile();
			welcomeWindow.Visible = true;
		}
		else
		{
			if (config.HasSectionKey("Pizza Timer", "version"))
			{
				reportedVersion = (float)config.GetValue("Pizza Timer", "version");
				GD.Print("Reported version: " + reportedVersion);
			}
			else
			{
				GD.Print("Could not find Program Version on the file. Creating a new one.");
				reportedVersion = 0;
				welcomeWindow.Visible = true;
				CreateFile();
			}
			if (reportedVersion >= version)
			{
				showWelcome = (bool)config.GetValue("Pizza Timer", "welcome");
				welcomeButton.ButtonPressed = showWelcome;
				secondsText.Value = (float)config.GetValue("Timer", "seconds");
				minutesText.Value = (float)config.GetValue("Timer", "minutes");
				hoursText.Value = (float)config.GetValue("Timer", "hours");
				music = (bool)config.GetValue("Settings", "music");
				crash = (bool)config.GetValue("Settings", "crash");
				shutdown = (bool)config.GetValue("Settings", "shutdown");
				lap3Spawn = (bool)config.GetValue("PizzaFace", "lap3spawn");
				slowPizzaFace = (bool)config.GetValue("PizzaFace", "slowpizzaface");
				spawnPizzaFace = (bool)config.GetValue("PizzaFace", "spawn");
				musicButton.ButtonPressed = music;
				crashButton.ButtonPressed = crash;
				shutdownButton.ButtonPressed = shutdown;
				lap3SpawnButton.ButtonPressed = lap3Spawn;
				slowPizzaFaceButton.ButtonPressed = slowPizzaFace;
				spawnPizzaFaceButton.ButtonPressed = spawnPizzaFace;
				GD.Print("Crash value: " + crash);
				if (showWelcome == true)
				{
					welcomeWindow.Visible = showWelcome;
				}
				if (music == false)
				{
					var sfx_index = AudioServer.GetBusIndex("Music");
					AudioServer.SetBusVolumeDb(sfx_index, -80);
				}
				if (spawnPizzaFace == false)
				{
					crashButton.Disabled = true;
					crashOptionsButton.Disabled = true;
					lap3SpawnButton.Disabled = true;
				}
				else
				{
					lap3SpawnButton.Disabled = false;
					if (isWindows == true)
					{
						crashButton.Disabled = false;
						crashOptionsButton.Disabled = false;
					}
				}
			}
			else
			{
				if (reportedVersion != 0)
				{
					GD.Print("Running a newer version of the program. Resetting values");
					CreateFile();
				}
			}
		}
	}

	private void CreateFile()
	{
		GD.Print("Creating file...");
		var config = new ConfigFile();
		Error err = config.Load("user://config.cfg");
		config.SetValue("Pizza Timer", "version", version);
		config.SetValue("Pizza Timer", "welcome", true);
		config.SetValue("Timer", "seconds", 30);
		config.SetValue("Timer", "minutes", 1);
		config.SetValue("Timer", "hours", 0);
		config.SetValue("Settings", "music", true);
		if (isWindows == true)
			config.SetValue("Settings", "crash", true);
		else
		{
			config.SetValue("Settings", "crash", false);
			crashButton.Disabled = true;
			crashButton.ButtonPressed = false;
		}
		config.SetValue("Settings", "shutdown", false);
		config.SetValue("PizzaFace", "lap3spawn", true);
		config.SetValue("PizzaFace", "slowpizzaface", false);
		config.SetValue("PizzaFace", "spawn", true);
		config.SetValue("Settings", "useCustom", false);
		config.SetValue("Settings", "customCommand", "");
		config.Save("user://config.cfg");
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
		config.SetValue("Pizza Timer", "version", version);
		config.SetValue("Pizza Timer", "welcome", showWelcome);
		config.SetValue("Timer", "seconds", seconds);
		config.SetValue("Timer", "minutes", minutes);
		config.SetValue("Timer", "hours", hours);
		config.SetValue("Settings", "music", music);
		config.SetValue("Settings", "crash", crash);
		config.SetValue("Settings", "shutdown", shutdown);
		config.SetValue("Settings", "useCustom", customCMD);
		config.SetValue("Settings", "customCommand", "");
		config.SetValue("PizzaFace", "lap3spawn", lap3Spawn);
		config.SetValue("PizzaFace", "slowpizzaface", slowPizzaFace);
		config.SetValue("PizzaFace", "spawn", spawnPizzaFace);
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
		DisplayServer.WindowSetSize(new Vector2I(450, 140));
		//Default 450, 300
		Vector2I winPos = GetWindow().Position;
		DisplayServer.WindowSetPosition(new Vector2I(winPos.X, winPos.Y + 160));
		PizzaTime scene = (PizzaTime)pizzaTime.Instantiate();
		GetTree().Root.GetChild(0).AddChild(scene);
		scene.Position = new Vector2I(DisplayServer.WindowGetPosition().X + 100, DisplayServer.WindowGetPosition().Y + 500);
	}

	public void _on_settings_pressed()
	{
		settingsWindow.Visible = true;
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
		if (toggled == false)
		{
			crash = false;
		}
		else
		{
			crash = true;
		}
	}

	public void _on_shutdown_toggled(bool toggled)
	{
		if (toggled == false)
		{
			shutdown = false;
		}
		else
		{
			shutdown = true;
		}
	}


	public void _on_show_welcome_toggled(bool toggled)
	{
		if (toggled == false)
		{
			showWelcome = false;
		}
		else
		{
			showWelcome = true;
		}
	}
	public void _on_lap_3_pizza_face_toggled(bool toggled)
	{
		if (toggled == false)
		{
			lap3Spawn = false;
		}
		else
		{
			lap3Spawn = true;
		}
	}
	public void _on_slow_toggled(bool toggled)
	{
		if (toggled == false)
		{
			slowPizzaFace = false;
		}
		else
		{
			slowPizzaFace = true;
		}
	}
	public void _on_spawn_toggled(bool toggled)
	{
		if (toggled == false)
		{
			spawnPizzaFace = false;
			lap3SpawnButton.Disabled = true;
			crashButton.Disabled = true;
			crashOptionsButton.Disabled = true;
		}
		else
		{
			spawnPizzaFace = true;
			lap3SpawnButton.Disabled = false;
			if (isWindows == true)
			{
				crashButton.Disabled = false;
				crashOptionsButton.Disabled = false;
			}
		}
	}
	public void _on_report_bugs_pressed()
	{
		OS.ShellOpen("https://github.com/ManuEcheveste/PizzaTimer/issues");
	}



	public void _on_crash_settings_pressed()
	{
		crashWindow.Visible = true;
	}
	public void _on_crash_settings_close_requested()
	{
		crashWindow.Visible = false;
	}
	public void _on_settings_close_requested()
	{
		settingsWindow.Visible = false;
	}
	public void _on_welcome_window_close_requested()
	{
		welcomeWindow.Visible = false;
	}
	public void _on_noise_button_pressed()
	{
		var rng = new RandomNumberGenerator();
		int sfx = rng.RandiRange(1, 6);
		switch (sfx)
		{
			case 1:
				noiseSFX.Stream = noise01;
				break;

			case 2:
				noiseSFX.Stream = noise02;
				break;

			case 3:
				noiseSFX.Stream = noise03;
				break;

			case 4:
				noiseSFX.Stream = noise04;
				break;

			case 5:
				noiseSFX.Stream = noise05;
				break;

			case 6:
				noiseSFX.Stream = noise06;
				break;
		}
		GD.Print("Noise value: " + sfx);
		noiseSFX.Play();
	}
}
