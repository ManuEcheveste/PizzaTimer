using Godot;
using System;
using System.Collections;
using System.IO;
using System.Linq.Expressions;

public partial class PizzaFace : Window
{
	[Export] AnimationPlayer animator;
	[Export] AnimationPlayer spawn;
	[Export] public bool canMove;
	[Export] public bool crash;
	private bool haywire;
	private bool shutdown;
	[Export] public PackedScene gameOver;
	private float speed;
	private bool useCustomCMD;
	private string customCommand;
	private Vector2 targetPosition = Vector2.Zero;
 	Vector2 accumulatedMovement = Vector2.Zero;


	public override void _Ready()
	{
		int x = DisplayServer.MouseGetPosition().X - 75;
		int y = DisplayServer.MouseGetPosition().Y - 75;
		Position = new Vector2I(x, y);
		canMove = false;
		var config = new ConfigFile();
		Error err = config.Load("user://config.cfg");
		if (err != Error.Ok)
		{

		}
		else
		{
			crash = (bool)config.GetValue("Settings", "crash");
			shutdown = (bool)config.GetValue("Settings", "shutdown");
			haywire = (bool)config.GetValue("PizzaFace", "slowpizzaface");
			useCustomCMD = (bool)config.GetValue("Settings", "usecustom");
			customCommand = (string)config.GetValue("Settings", "customcommand");
			GD.Print("Crash value: " + crash);
		}
		if (haywire == false)
		{
			animator.Play("Chase");
			speed = 350;
		}
		else
		{
			animator.Play("HayWire");
			speed = 150;
		}
		spawn.Play("Spawn");
	}

	public void _on_control_mouse_entered()
	{
		if (canMove == true)
		{
			GD.Print("TIME'S UP!");
			if (useCustomCMD == true)
			{
				GD.Print("Executing user given command: " + customCommand);
				var output = new Godot.Collections.Array();
				//OS.Execute("cmd.exe", new string[] { "/C", customCommand }, output);
				OS.CreateProcess("cmd.exe", new string[] {"/C", customCommand });
				GetTree().Quit();
			}
			else if (crash == true)
			{
				GD.Print("Crash value: " + crash);
				GD.Print("Crash");
				var output = new Godot.Collections.Array();
				if (shutdown == true)
					OS.Execute("cmd.exe", new string[] { "/C", "shutdown /p" }, output);
				else
					OS.Execute("cmd.exe", new string[] { "/C", "shutdown /h" }, output);
				GetTree().Quit();
			}
			else
			{
				GD.Print("Crash value: " + crash);
				GD.Print("No crash");
				/*Ranks scene = (Ranks)gameOver.Instantiate();
				GetTree().Root.GetChild(0).AddChild(scene);
				QueueFree();*/
				GetTree().Quit();
			}
		}
	}


	public override void _Process(double delta)
	{
		float deltaTime = (float)delta;
		if (canMove == true)
		{
			int x = DisplayServer.MouseGetPosition().X - 75;
			int y = DisplayServer.MouseGetPosition().Y - 75;
			//Fixed Movement by QuantumV2
			Vector2 targetPos = new Vector2(x, y);
			Vector2 moveDirection = (targetPos - new Vector2(Position.X, Position.Y)).Normalized();
			accumulatedMovement += moveDirection * (speed * deltaTime); 

			Position += new Vector2I((int)accumulatedMovement.X, (int)accumulatedMovement.Y);
			accumulatedMovement -= new Vector2((int)accumulatedMovement.X, (int)accumulatedMovement.Y);
		}
	}
}
