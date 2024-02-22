using Godot;
using System;

public partial class LapsLogic : Window
{
	[Export] public Texture2D lap2SPR;
	[Export] public Texture2D lap3SPR;
	[Export] public Sprite2D spr;	
	private int spawnAni;
	private bool useTimer;
	private float timer = 2;
	public void SpawnFlag(int lap)
	{
		var displaySizeX = DisplayServer.ScreenGetSize().X;
		var displaySizeY = DisplayServer.ScreenGetSize().Y;
		switch (lap)
		{
			case 2:
				spr.Texture = lap2SPR;
				break;
			case 3:
				spr.Texture = lap3SPR;
				break;
		}
		Position = new Vector2I(displaySizeX / 2 - 100, -160);
	}

	public override void _Process(double delta)
	{
		float deltaTime = (float)delta;
		if (useTimer == true)
		{
			if (timer > 0)
			{
				timer -= deltaTime;
			}
			else
			{
				useTimer = false;
				spawnAni = 2;
			}
		}
		
		if (Position.Y < 0 && spawnAni == 0)
		{
			int speed = 150;
			var time = Position.Y + speed * deltaTime;
			Position = new Vector2I(Position.X, (int)time);
		}
		else if (Position.Y >= 0 && spawnAni == 0)
		{
			useTimer = true;
			spawnAni = 1;
		}
		else if (Position.Y > -180 && spawnAni == 2)
		{			
			int speed = 150;
			var time = Position.Y - speed * deltaTime;
			Position = new Vector2I(Position.X, (int)time);
		}
		else if (Position.Y < -160 && spawnAni == 2)
		{
			QueueFree();
		}
		
		var rng = new RandomNumberGenerator();
		int posX = rng.RandiRange(-1,1);
		int posY = rng.RandiRange(-1,1);
		spr.Position = new Vector2 (120 + posX, 80 + posY);
		
	}
}