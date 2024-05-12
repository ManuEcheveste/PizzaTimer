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
	[Export] public Button noiseButton;
	[Export] public CheckButton musicButton;
	[Export] public CheckButton crashButton;
	[Export] public Button crashOptionsButton;
	[Export] public CheckButton shutdownButton;
	[Export] public CheckButton lap3SpawnButton;
	[Export] public CheckButton slowPizzaFaceButton;
	[Export] public CheckButton spawnPizzaFaceButton;
	[Export] public CheckButton useCustomCMDButton;
	[Export] public Button setCMDButton;
	[Export] public TextEdit customCommand;
	[Export] public Window commandWindow;
	[Export] public CheckButton welcomeButton;
	[Export] public Window crashWindow;
	[Export] public Window welcomeWindow;
	[Export] public Window settingsWindow;
	[Export] public AnimationPlayer timerSpawn;
	[Export] public AnimationPlayer pizzaFaceAnimator;
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
	[Export] public AudioStream peppino01;
	[Export] public AudioStream peppino02;
	[Export] public AudioStream peppino03;
	[Export] public AudioStream peppino04;
	[Export] public AudioStream peppino05;
	[Export] public AudioStream peppino06;
	[Export] public AudioStream noiseBGM;
	[Export] public Texture2D noiseSPR;
	[Export] public Texture2D peppinoSPR;
	[Export] public Theme miniPizza;
	[Export] public Theme miniTower;

	public float hours;
	public float minutes;
	public float seconds;
	[Export] public float maxTime;
	public bool music;
	public bool crash;
	public bool noise;
	private bool shutdown;
	private bool lap3Spawn;
	private bool slowPizzaFace = false;
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
		var config = new ConfigFile();
		Error err = config.Load("user://config.cfg");
		if (isWindows == false)
		{
			crashButton.Disabled = true;
			crashButton.ButtonPressed = false;
			crashOptionsButton.Disabled = true;
			useCustomCMDButton.Disabled = true;
			customCMD = false;
			setCMDButton.Disabled = true;
		}
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
				ReadData();
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

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("mute"))
		{
			if (timer.pizzaTime == true)
			{
				var sfx_index = AudioServer.GetBusIndex("Music");
				if (AudioServer.GetBusVolumeDb(sfx_index) == 0)
				{
					AudioServer.SetBusVolumeDb(sfx_index, -80);
				}
				else
				{
					AudioServer.SetBusVolumeDb(sfx_index, 0);
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
		config.SetValue("Pizza Timer", "noise", false);
		config.SetValue("Timer", "seconds", 16);
		config.SetValue("Timer", "minutes", 2);
		config.SetValue("Timer", "hours", 0);
		config.SetValue("Settings", "music", true);
		if (isWindows == true)
		{
			config.SetValue("Settings", "crash", true);
			config.SetValue("Settings", "shutdown", false);
		}
		else
		{
			config.SetValue("Settings", "crash", false);
			config.SetValue("Settings", "shutdown", false);
			crashButton.Disabled = true;
			crashButton.ButtonPressed = false;
			crashOptionsButton.Disabled = true;
			useCustomCMDButton.Disabled = true;
			customCMD = false;
			setCMDButton.Disabled = true;
		}
		config.SetValue("PizzaFace", "lap3spawn", true);
		config.SetValue("PizzaFace", "slowpizzaface", false);
		config.SetValue("PizzaFace", "spawn", true);
		config.SetValue("Settings", "usecustom", false);
		config.SetValue("Settings", "customcommand", "");
		config.Save("user://config.cfg");
		ReadData();
	}

	private void ReadData()
	{
		var config = new ConfigFile();
		Error err = config.Load("user://config.cfg");
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
		slowPizzaFaceButton.ButtonPressed = slowPizzaFace;
		spawnPizzaFace = (bool)config.GetValue("PizzaFace", "spawn");
		customCMD = (bool)config.GetValue("Settings", "usecustom");
		command = (string)config.GetValue("Settings", "customcommand");
		noise = (bool)config.GetValue("Pizza Timer", "noise");
		if (noise == true)
		{
			noiseButton.Icon = noiseSPR;
		}
		else
		{
			noiseButton.Icon = peppinoSPR;
		}
		customCommand.Text = command;
		musicButton.ButtonPressed = music;
		crashButton.ButtonPressed = crash;
		shutdownButton.ButtonPressed = shutdown;
		lap3SpawnButton.ButtonPressed = lap3Spawn;
		spawnPizzaFaceButton.ButtonPressed = spawnPizzaFace;
		useCustomCMDButton.ButtonPressed = customCMD;
		customCommand.Text = command;

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
		if (customCMD == false)
		{
			setCMDButton.Disabled = true;
		}
		if (spawnPizzaFace == false)
		{
			crashOptionsButton.Theme = miniTower;
			slowPizzaFaceButton.Disabled = true;
		}
		else
		{
			crashOptionsButton.Theme = miniPizza;
			slowPizzaFaceButton.Disabled = false;
			if (isWindows == true)
			{
				crashButton.Disabled = false;
				crashOptionsButton.Disabled = false;
			}
		}
		if (spawnPizzaFace == false)
		{
			johnPillar.Play("Tower");
		}
		else
		{
			johnPillar.Play("JohnIdle");
		}
		//IsTimerArgument();
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
		config.SetValue("Pizza Timer", "noise", noise);
		config.SetValue("Timer", "seconds", seconds);
		config.SetValue("Timer", "minutes", minutes);
		config.SetValue("Timer", "hours", hours);
		config.SetValue("Settings", "music", music);
		config.SetValue("Settings", "crash", crash);
		config.SetValue("Settings", "shutdown", shutdown);
		config.SetValue("Settings", "usecustom", customCMD);
		config.SetValue("Settings", "customcommand", command);
		config.SetValue("PizzaFace", "lap3spawn", lap3Spawn);
		config.SetValue("PizzaFace", "slowpizzaface", slowPizzaFace);
		config.SetValue("PizzaFace", "spawn", spawnPizzaFace);
		config.Save("user://config.cfg");
		timerSpawn.Play("TimerSpawn");
		if (spawnPizzaFace == false)
			pizzaFaceAnimator.Play("Tower");
		johnOST.Stop();
		johnOST.Bus = "Master";
		johnOST.Stream = jonhGutted;
		johnOST.Play();
		if (noise == true)
			bgm.Stream = noiseBGM;
		bgm.Play();
		timer.timer = maxTime;
		timer.maxTime = maxTime;
		timer.pizzaTime = true;
		timer.noise = noise;
		timer.ItsPizzaTime();
		DisplayServer.WindowSetSize(new Vector2I(450, 140));
		DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, true);
		//Default 450, 300
		Vector2I winPos = GetWindow().Position;
		var displaySizeX = DisplayServer.ScreenGetSize().X;
		var displaySizeY = DisplayServer.ScreenGetSize().Y;
		DisplayServer.WindowSetPosition(new Vector2I(displaySizeX / 2 - 200, displaySizeY - 150));
		PizzaTime scene = (PizzaTime)pizzaTime.Instantiate();
		GetTree().Root.GetChild(0).AddChild(scene);
		scene.Position = new Vector2I(displaySizeX / 2 - 100, displaySizeY + 100);
		GD.Print("Timer: " + maxTime);
	}

	public void AutoStart()
	{
		GD.Print("It's Pizza Time!");/*
		seconds = maxTime % 60;
		minutes = maxTime % 3600 / 60;
		hours = maxTime / 3600;*/
		timerSpawn.Play("TimerSpawn");
		if (spawnPizzaFace == false)
			pizzaFaceAnimator.Play("Tower");
		johnOST.Stop();
		johnOST.Bus = "Master";
		johnOST.Stream = jonhGutted;
		johnOST.Play();
		if (noise == true)
			bgm.Stream = noiseBGM;
		bgm.Play();
		timer.timer = maxTime;
		timer.maxTime = maxTime;
		timer.pizzaTime = true;
		timer.noise = noise;
		timer.ItsPizzaTime();
		DisplayServer.WindowSetSize(new Vector2I(450, 140));
		DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.Borderless, true);
		//Default 450, 300
		Vector2I winPos = GetWindow().Position;
		var displaySizeX = DisplayServer.ScreenGetSize().X;
		var displaySizeY = DisplayServer.ScreenGetSize().Y;
		DisplayServer.WindowSetPosition(new Vector2I(displaySizeX / 2 - 200, displaySizeY - 150));
		welcomeWindow.Visible = false;
		//PizzaTime scene = (PizzaTime)pizzaTime.Instantiate();
		//GetTree().Root.GetChild(0).AddChild(scene);
		//scene.Position = new Vector2I(displaySizeX / 2 - 100, displaySizeY + 100);
		GD.Print("Timer: " + maxTime);
	}

	public void _on_settings_pressed()
	{
		settingsWindow.Visible = true;
	}

	public void _on_mouse_entered()
	{
		if (spawnPizzaFace == true)
			johnPillar.Play("Scared");
	}

	public void _on_mouse_exited()
	{
		if (spawnPizzaFace == true)
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
			slowPizzaFaceButton.Disabled = true;
			crashOptionsButton.Theme = miniTower;
			johnPillar.Play("Tower");

		}
		else
		{
			johnPillar.Play("JohnIdle");
			spawnPizzaFace = true;
			crashOptionsButton.Theme = miniPizza;
			slowPizzaFaceButton.Disabled = false;
			if (isWindows == true)
			{
				crashButton.Disabled = false;
				crashOptionsButton.Disabled = false;
			}
		}
	}

	public void _on_use_custom_cmd_toggled(bool toggled)
	{
		if (toggled == false)
		{
			customCMD = false;
			setCMDButton.Disabled = true;
		}
		else
		{
			customCMD = true;
			setCMDButton.Disabled = false;
		}
	}

	public void _on_set_custom_cmd_pressed()
	{
		commandWindow.Visible = true;
	}

	public void _on_ok_pressed()
	{
		command = customCommand.Text;
		commandWindow.Visible = false;
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
	public void _on_controls_pressed()
	{
		welcomeWindow.Visible = true;
	}
	public void _on_welcome_window_close_requested()
	{
		welcomeWindow.Visible = false;
	}
	public void _on_noise_button_pressed()
	{
		var rng = new RandomNumberGenerator();
		int sfx = rng.RandiRange(1, 6);
		if (noise == false)
		{
			noise = true;
			noiseButton.Icon = noiseSPR;
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
		}
		else
		{
			noise = false;
			noiseButton.Icon = peppinoSPR;
			switch (sfx)
			{
				case 1:
					noiseSFX.Stream = peppino01;
					break;

				case 2:
					noiseSFX.Stream = peppino02;
					break;

				case 3:
					noiseSFX.Stream = peppino03;
					break;

				case 4:
					noiseSFX.Stream = peppino04;
					break;

				case 5:
					noiseSFX.Stream = peppino05;
					break;

				case 6:
					noiseSFX.Stream = peppino06;
					break;
			}
		}
		GD.Print("Noise value: " + sfx);
		noiseSFX.Play();
	}
	public void IsTimerArgument()
	{
		GD.Print("Looking for arguments");
		var arguments = OS.GetCmdlineArgs();

		for (int i = 0; i < arguments.Length; i++)
		{
			var argument = arguments[i];

			if (argument == "--timer" && i + 1 < arguments.Length)
			{
				GD.Print("Detected --timer argument");
				if (int.TryParse(arguments[i + 1], out int time))
				{
					maxTime = time;
					AutoStart();
				}
				break;
			}
		}
	}
}
