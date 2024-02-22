using Godot;
using System;
using System.IO;

public partial class TimerScript : Control
{
	public float timer;
	public float maxTime;
	public float hours;
	public float minutes;
	private string minutesText;
	public float seconds;
	private string secondsText;
	private float deltaTime;
	public bool showTime;
	public bool panic;
	public bool lap1;
	private int lap;
	public bool pizzaTime;
	private bool pizzaFaceActive;
	private bool canSpawnPizzaFace;
	private bool lap3SpawnPizzaFace;
	[Export] public AnimationPlayer pizzaFaceTimer;
	[Export] public Timer timerLogic;
	[Export] public TextureProgressBar fill;
	[Export] public HSlider john;
	[Export] public AnimationPlayer johnFace;
	[Export] public AudioStreamPlayer2D panicBGM;
	[Export] public AudioStreamPlayer2D bgm;
	[Export] public AudioStreamPlayer2D pizzaFaceSFX;
	[Export] public AudioStreamPlayer2D sfx;
	[Export] public AudioStream lapsSFX;
	[Export] public BGM bgmLogic;
	[Export] public RichTextLabel timeText;
	[Export] public PackedScene pizzaFace;
	[Export] public PackedScene ranks;
	[Export] public PackedScene lapsFlag;
	public PizzaFace pizzaFaceObj;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		pizzaFaceTimer.Play("PizzaFace");
		johnFace.Play("John");
		lap = 1;
		showTime = false;
		lap1 = true;
		panic = false;
		pizzaTime = false;
		pizzaFaceActive = false;
	}

	public void ItsPizzaTime()
	{
		fill.MaxValue = timer;
		fill.Value = 0;
		john.MaxValue = timer;
		john.Value = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		deltaTime = (float)delta;

		if (pizzaTime == true)
		{
			minutes = timer / 60;
			minutes = (float)Mathf.CeilToInt(minutes);
			seconds = timer % 60;
			seconds = (float)Mathf.CeilToInt(seconds);
			timerLogic.SetTime(timer);

			if (minutes > 0)
			{
				minutesText = (minutes -= 1).ToString();
			}
			if (seconds < 10)
			{
				secondsText = "0" + seconds;
			}
			else
			{
				secondsText = seconds.ToString();
			}
			if (secondsText == "60")
			{
				secondsText = "00";
			}
			timeText.Text = "[center]" + minutesText + ":" + secondsText + "[/center]";
			if (timer < 56 && panic == false && lap1 == true)
			{
				panic = true;
				panicBGM.Play();
			}
			if (panic == true)
			{
				if (lap1 == true)
				{
					bgm.VolumeDb -= 5 * deltaTime;
					if (panicBGM.VolumeDb < 0)
					{
						panicBGM.VolumeDb += deltaTime;
					}
				}
			}
			if (timer > 0)
			{
				timer -= deltaTime;
				fill.Value = maxTime - timer;
				john.Value = maxTime - timer;
			}
			else
			{
				if (showTime == false)
				{
					var config = new ConfigFile();
					Error err = config.Load("user://config.cfg");
					if (err != Error.Ok)
					{

					}
					else
					{
						canSpawnPizzaFace = (bool)config.GetValue("PizzaFace", "spawn");
					}
					if (canSpawnPizzaFace == true)
					{
						showTime = true;
						pizzaFaceTimer.Play("ShowTime");
						pizzaFaceSFX.Play();
						PizzaFace scene = (PizzaFace)pizzaFace.Instantiate();
						GetTree().Root.GetChild(0).AddChild(scene);
						pizzaFaceObj = scene;
						pizzaFaceActive = true;
					}
					else
					{
						bgm.Stop();
						panicBGM.Stop();
						showTime = true;
						pizzaFaceTimer.Play("Dispawn");
						Ranks scene = (Ranks)ranks.Instantiate();
						GetTree().Root.GetChild(0).AddChild(scene);
						scene.SetRanks(-1);
					}
				}
			}


			if (Input.IsActionPressed("lap2"))
			{
				if (lap == 1)
				{
					lap++;
					lap1 = false;
					bgmLogic.NextLap();
					GD.Print("Lap 2!");
					LapsLogic lapFlag = (LapsLogic)lapsFlag.Instantiate();
					GetTree().Root.GetChild(0).AddChild(lapFlag);
					lapFlag.SpawnFlag(2);
					sfx.Stream = lapsSFX;
					sfx.Play();
				}
			}
			if (Input.IsActionPressed("lap3"))
			{
				if (lap == 2)
				{
					var config = new ConfigFile();
					Error err = config.Load("user://config.cfg");
					if (err != Error.Ok)
					{

					}
					else
					{
						lap3SpawnPizzaFace = (bool)config.GetValue("PizzaFace", "lap3spawn");
					}
					lap++;
					bgmLogic.NextLap();
					GD.Print("Lap 3!");					
					LapsLogic lapFlag = (LapsLogic)lapsFlag.Instantiate();
					GetTree().Root.GetChild(0).AddChild(lapFlag);
					lapFlag.SpawnFlag(3);
					sfx.Stream = lapsSFX;
					sfx.Play();
					if (lap3SpawnPizzaFace == true)
					{
						timer = 0.1f;
					}
				}
			}

			if (Input.IsActionPressed("exit"))
			{
				if (pizzaFaceActive == true)
				{
					pizzaFaceSFX.Stop();
					pizzaFaceObj.QueueFree();
				}
				else
				{
					pizzaFaceTimer.Play("Dispawn");
				}
				pizzaTime = false;
				bgm.Stop();
				panicBGM.Stop();
				Ranks scene = (Ranks)ranks.Instantiate();
				GetTree().Root.GetChild(0).AddChild(scene);
				scene.SetRanks(-1);
				scene.PizzaFaceOwned(pizzaFaceActive);
			}

			if (Input.IsActionPressed("drank"))
			{
				if (pizzaFaceActive == true)
				{
					pizzaFaceSFX.Stop();
					pizzaFaceObj.QueueFree();
				}
				else
				{
					pizzaFaceTimer.Play("Dispawn");
				}
				pizzaTime = false;
				bgm.Stop();
				panicBGM.Stop();
				Ranks scene = (Ranks)ranks.Instantiate();
				GetTree().Root.GetChild(0).AddChild(scene);
				scene.SetRanks(0);
				scene.PizzaFaceOwned(pizzaFaceActive);
			}
			if (Input.IsActionPressed("crank"))
			{
				if (pizzaFaceActive == true)
				{
					pizzaFaceSFX.Stop();
					pizzaFaceObj.QueueFree();
				}
				else
				{
					pizzaFaceTimer.Play("Dispawn");
				}
				pizzaTime = false;
				bgm.Stop();
				panicBGM.Stop();
				Ranks scene = (Ranks)ranks.Instantiate();
				GetTree().Root.GetChild(0).AddChild(scene);
				scene.PizzaFaceOwned(pizzaFaceActive);
				scene.SetRanks(1);
			}
			if (Input.IsActionPressed("brank"))
			{
				if (pizzaFaceActive == true)
				{
					pizzaFaceSFX.Stop();
					pizzaFaceObj.QueueFree();
				}
				else
				{
					pizzaFaceTimer.Play("Dispawn");
				}
				pizzaTime = false;
				bgm.Stop();
				panicBGM.Stop();
				Ranks scene = (Ranks)ranks.Instantiate();
				GetTree().Root.GetChild(0).AddChild(scene);
				scene.PizzaFaceOwned(pizzaFaceActive);
				scene.SetRanks(2);
			}
			if (Input.IsActionPressed("arank"))
			{
				if (pizzaFaceActive == true)
				{
					pizzaFaceSFX.Stop();
					pizzaFaceObj.QueueFree();
				}
				else
				{
					pizzaFaceTimer.Play("Dispawn");
				}
				pizzaTime = false;
				bgm.Stop();
				panicBGM.Stop();
				Ranks scene = (Ranks)ranks.Instantiate();
				GetTree().Root.GetChild(0).AddChild(scene);
				scene.PizzaFaceOwned(pizzaFaceActive);
				scene.SetRanks(3);
			}
			if (Input.IsActionPressed("srank"))
			{
				if (pizzaFaceActive == true)
				{
					pizzaFaceSFX.Stop();
					pizzaFaceObj.QueueFree();
				}
				else
				{
					pizzaFaceTimer.Play("Dispawn");
				}
				pizzaTime = false;
				bgm.Stop();
				panicBGM.Stop();
				Ranks scene = (Ranks)ranks.Instantiate();
				GetTree().Root.GetChild(0).AddChild(scene);
				scene.PizzaFaceOwned(pizzaFaceActive);
				scene.SetRanks(4);
			}
			if (Input.IsActionPressed("prank"))
			{
				if (pizzaFaceActive == true)
				{
					pizzaFaceSFX.Stop();
					pizzaFaceObj.QueueFree();
				}
				else
				{
					pizzaFaceTimer.Play("Dispawn");
				}
				pizzaTime = false;
				bgm.Stop();
				panicBGM.Stop();
				Ranks scene = (Ranks)ranks.Instantiate();
				GetTree().Root.GetChild(0).AddChild(scene);
				scene.PizzaFaceOwned(pizzaFaceActive);
				scene.SetRanks(5);
			}
			if (Input.IsActionPressed("lapped"))
			{
				if (lap == 3)
				{
					pizzaFaceSFX.Stop();
					pizzaFaceObj.QueueFree();
					pizzaTime = false;
					bgm.Stop();
					panicBGM.Stop();
					Ranks scene = (Ranks)ranks.Instantiate();
					GetTree().Root.GetChild(0).AddChild(scene);
					scene.PizzaFaceOwned(pizzaFaceActive);
					scene.SetRanks(6);
				}

			}

		}

	}
}
