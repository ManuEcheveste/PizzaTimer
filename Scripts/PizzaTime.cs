using Godot;
using System;

public partial class PizzaTime : Window
{
	[Export] AnimationPlayer animator;
	private float timer;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animator.Play("pizza");
		timer = 10;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		float deltaTime = (float)delta;
		Position = new Vector2I(Position.X, Position.Y - 6);
		if(timer > 0)
		{
			timer -= deltaTime;
		}
		else
		{
			QueueFree();
		}
	}
}
