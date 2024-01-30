using Godot;
using System;
using System.IO;

public partial class TimerScript : Control
{
	public float timer;
	public float maxTime;
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
	[Export] public AnimationPlayer pizzaFaceTimer;
	[Export] public TextureProgressBar fill;
	[Export] public HSlider john;
	[Export] public AnimationPlayer johnFace;
	[Export] public AudioStreamPlayer2D panicBGM;
	[Export] public AudioStreamPlayer2D bgm;
	[Export] public AudioStreamPlayer2D pizzaFaceSFX;
	[Export] public BGM bgmLogic;
	[Export] public RichTextLabel timeText;
	[Export] public PackedScene pizzaFace;

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
		
		if(Input.IsActionPressed("lap2"))
		{
			if(lap == 1)
			{
				lap++;
				lap1 = false;
				bgmLogic.NextLap();				
				GD.Print("Lap 2!");
			}			
		}
		if(Input.IsActionPressed("lap3"))
		{
			if(lap == 2)
			{
				lap++;
				bgmLogic.NextLap();
				GD.Print("Lap 3!");
				
				timer = 0.1f;
			}
		}
		
		if (pizzaTime == true)
		{
			minutes = timer/60;
			minutes = (float)Mathf.CeilToInt(minutes);
			seconds = timer%60;
			seconds = (float)Mathf.CeilToInt(seconds);
			;
			
			if(minutes > 0)
			{
				minutesText = (minutes -= 1).ToString();	
			}
			if(seconds < 10)
			{
				secondsText = "0"+seconds;
			}
			else
			{
				secondsText = seconds.ToString();
			}			
			if(secondsText == "60")
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
					showTime = true;
					pizzaFaceTimer.Play("ShowTime");
					pizzaFaceSFX.Play();
					PizzaFace scene = (PizzaFace)pizzaFace.Instantiate();
					GetTree().Root.GetChild(0).AddChild(scene);
				}
			}
		}

	}
}
