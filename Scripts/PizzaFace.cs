using Godot;
using System;
using System.IO;
using System.Linq.Expressions;

public partial class PizzaFace : Window
{
	[Export] AnimationPlayer animator;
	[Export] AnimationPlayer spawn;
	[Export] public bool canMove;
	[Export] public bool crash;

	private Vector2 targetPosition = Vector2.Zero;
	public override void _Ready()
	{
		animator.Play("Chase");
		int x = DisplayServer.MouseGetPosition().X - 75;
		int y = DisplayServer.MouseGetPosition().Y - 75;
		Position = new Vector2I(x, y);
		canMove = false;
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
			GD.Print("No file found");
		}
		else
		{
			crash = (bool)config.GetValue("Settings", "crash");
			GD.Print("Crash value: " + crash);
		}
		spawn.Play("Spawn");
	}

	public void _on_control_mouse_entered()
	{
		if (canMove == true)
		{
			GD.Print("TIME'S UP!");
			if (crash == true)
			{
				GD.Print("Crash value: " + crash);
				GD.Print("Crash");
				var output = new Godot.Collections.Array();
				OS.Execute("cmd.exe", new string[] { "/C", "shutdown /h" }, output);
				GetTree().Quit();
			}
			else
			{
				GD.Print("Crash value: " + crash);
				GD.Print("No crash");
				GetTree().Quit();
			}
		}
	}


	public override void _Process(double delta)
	{
		if (canMove == true)
		{
			int x = DisplayServer.MouseGetPosition().X - 75;
			int y = DisplayServer.MouseGetPosition().Y - 75;
			//GD.Print(DisplayServer.MouseGetPosition());
			if (Position.X < x)
			{
				if (x - Position.X > 5)
				{
					int pizzaX = Position.X + 4;
					Position = new Vector2I(pizzaX, Position.Y);
				}
				else
				{
					int pizzaX = Position.X + 1;
					Position = new Vector2I(pizzaX, Position.Y);
				}
			}
			else if (Position.X > x)
			{
				if (Position.X - x > 5)
				{
					int pizzaX = Position.X - 4;
					Position = new Vector2I(pizzaX, Position.Y);
				}
				else
				{
					int pizzaX = Position.X - 1;
					Position = new Vector2I(pizzaX, Position.Y);
				}
			}

			if (Position.Y < y)
			{
				if (y - Position.Y > 5)
				{
					int pizzaY = Position.Y + 4;
					Position = new Vector2I(Position.X, pizzaY);
				}
				else
				{
					int pizzaY = Position.Y + 1;
					Position = new Vector2I(Position.X, pizzaY);
				}
			}
			else if (Position.Y > y)
			{
				if (Position.Y - y > 5)
				{
					int pizzaY = Position.Y - 4;
					Position = new Vector2I(Position.X, pizzaY);
				}
				else
				{
					int pizzaY = Position.Y - 1;
					Position = new Vector2I(Position.X, pizzaY);
				}
			}
		}
	}
}
