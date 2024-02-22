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
			speed = 250;
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
			
			if (Position.X < x)
			{
				if (x - Position.X > 5)
				{
					var pizzaX = Position.X + speed * deltaTime;
					Position = new Vector2I((int)pizzaX, Position.Y);
				}
				else
				{
					var pizzaX = Position.X + 1;
					Position = new Vector2I(pizzaX, Position.Y);
				}
			}
			else if (Position.X > x)
			{
				if (Position.X - x > 5)
				{
					var pizzaX = Position.X - speed * deltaTime;
					Position = new Vector2I((int)pizzaX, Position.Y);
				}
				else
				{
					var pizzaX = Position.X - 1;
					Position = new Vector2I(pizzaX, Position.Y);
				}
			}

			if (Position.Y < y)
			{
				if (y - Position.Y > 5)
				{
					var pizzaY = Position.Y + speed * deltaTime;
					Position = new Vector2I(Position.X, (int)pizzaY);
				}
				else
				{
					var pizzaY = Position.Y + 1;
					Position = new Vector2I(Position.X, pizzaY);
				}
			}
			else if (Position.Y > y)
			{
				if (Position.Y - y > 5)
				{
					var pizzaY = Position.Y - speed * deltaTime;
					Position = new Vector2I(Position.X, (int)pizzaY);
				}
				else
				{
					var pizzaY = Position.Y - 1;
					Position = new Vector2I(Position.X, pizzaY);
				}
			}
		}
	}
}
