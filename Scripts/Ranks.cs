using Godot;
using System;

public partial class Ranks : Control
{
	// Called when the node enters the scene tree for the first time.

	[Export] public AnimationPlayer animator;
	[Export] public AnimationPlayer pizzaOwned;
	[Export] public Window window;
	[Export] public bool isGameOver;

	public void Exit()
	{
		GetTree().Quit();
	}

	public override void _Ready()
	{
		if (isGameOver == true)
		{
			animator.Play("FatCursor");
		}

	}

	public void PizzaFaceOwned(bool owned)
	{
		if (owned == true)
		{
			pizzaOwned.Play("Owned");
		}
	}

	public void SetRanks(int ranks) //-1 = No Rank, 0 = D, 1 = C, 2 = B, 3 = A, 4 = S, 5 = P, 6 = Lapped
	{
		var text = window.Title;
		var play = animator.Play;
		switch (ranks)
		{
			case -1:
				play("Exit");
				text = " ";
				break;
			case 0:
				play("D");
				text = "Good job! That was... AWFUL";
				break;
			case 1:
				play("C");
				text = "That was... bad";
				break;
			case 2:
				play("B");
				text = "Oh!... OK";
				break;
			case 3:
				play("A");
				text = "Very... NICE";
				break;
			case 4:
				play("S");
				text = "PERFECT";
				break;
			case 5:
				play("P");
				text = " ";
				break;
			case 6:
				play("Lapped");
				text = "Lapped";
				break;
		}
	}
}
