using Godot;
using System;

public partial class Timer : Node2D
{
	[Export] Texture2D zeroSPR;
	[Export] Texture2D oneSPR;
	[Export] Texture2D twoSPR;
	[Export] Texture2D threeSPR;
	[Export] Texture2D fourSPR;
	[Export] Texture2D fiveSPR;
	[Export] Texture2D sixSPR;
	[Export] Texture2D sevenSPR;
	[Export] Texture2D eightSPR;
	[Export] Texture2D nineSPR;

	[Export] Sprite2D secondsUnitSPR;
	[Export] Sprite2D secondsTenSPR;
	[Export] Sprite2D minutesUnitSPR;
	[Export] Sprite2D minutesTenSPR;
	[Export] Sprite2D hoursUnitSPR;
	[Export] Sprite2D hoursTenSPR;
	[Export] Sprite2D hoursDivisor;

	public int hours;
	public int minutes;
	public int seconds;

	private int hoursUnit;
	private int hoursTen;
	private int minutesUnit;
	private int minutesTen;
	private int secondsUnit;
	private int secondsTen;
	// Called when the node enters the scene tree for the first time.
	public void SetTime(float timer)
	{
		if(timer > 36000)
		{
			hoursTenSPR.Visible = true;
			hoursUnitSPR.Visible = true;
			hoursDivisor.Visible = true;
			minutesTenSPR.Visible = true;
			this.Position = new Vector2(190,this.Position.Y);				
		}		
		else if(timer > 3600)
		{			
			hoursTenSPR.Visible = false;
			hoursUnitSPR.Visible = true;
			hoursDivisor.Visible = true;
			minutesTenSPR.Visible = true;
			this.Position = new Vector2(175,this.Position.Y);	
		}
		else if(timer > 600)
		{
			hoursTenSPR.Visible = false;
			hoursUnitSPR.Visible = false;
			hoursDivisor.Visible = false;
			minutesTenSPR.Visible = true;
			this.Position = new Vector2(150,this.Position.Y);			
		}
		else
		{			
			hoursTenSPR.Visible = false;
			hoursUnitSPR.Visible = false;
			hoursDivisor.Visible = false;
			minutesTenSPR.Visible = false;
			this.Position = new Vector2(140,this.Position.Y);
		}
		hours = (int)timer / 3600;
		minutes = ((int)timer % 3600) / 60;
		seconds = (int)timer % 60;
		
		hoursUnit = hours % 10;
		hoursTen = hours / 10;
		minutesUnit = minutes % 10;
		minutesTen = minutes / 10;
		secondsUnit = seconds % 10;
		secondsTen = seconds / 10;

		switch (secondsUnit)
		{
			case 0:
				secondsUnitSPR.Texture = zeroSPR;
				break;

			case 1:
				secondsUnitSPR.Texture = oneSPR;
				break;

			case 2:
				secondsUnitSPR.Texture = twoSPR;
				break;

			case 3:
				secondsUnitSPR.Texture = threeSPR;
				break;

			case 4:
				secondsUnitSPR.Texture = fourSPR;
				break;

			case 5:
				secondsUnitSPR.Texture = fiveSPR;
				break;

			case 6:
				secondsUnitSPR.Texture = sixSPR;
				break;

			case 7:
				secondsUnitSPR.Texture = sevenSPR;
				break;

			case 8:
				secondsUnitSPR.Texture = eightSPR;
				break;

			case 9:
				secondsUnitSPR.Texture = nineSPR;
				break;
		}
		switch (secondsTen)
		{
			case 0:
				secondsTenSPR.Texture = zeroSPR;
				break;

			case 1:
				secondsTenSPR.Texture = oneSPR;
				break;

			case 2:
				secondsTenSPR.Texture = twoSPR;
				break;

			case 3:
				secondsTenSPR.Texture = threeSPR;
				break;

			case 4:
				secondsTenSPR.Texture = fourSPR;
				break;

			case 5:
				secondsTenSPR.Texture = fiveSPR;
				break;

			case 6:
				secondsTenSPR.Texture = sixSPR;
				break;

			case 7:
				secondsTenSPR.Texture = sevenSPR;
				break;

			case 8:
				secondsTenSPR.Texture = eightSPR;
				break;

			case 9:
				secondsTenSPR.Texture = nineSPR;
				break;
		}
		switch (minutesUnit)
		{
			case 0:
				minutesUnitSPR.Texture = zeroSPR;
				break;

			case 1:
				minutesUnitSPR.Texture = oneSPR;
				break;

			case 2:
				minutesUnitSPR.Texture = twoSPR;
				break;

			case 3:
				minutesUnitSPR.Texture = threeSPR;
				break;

			case 4:
				minutesUnitSPR.Texture = fourSPR;
				break;

			case 5:
				minutesUnitSPR.Texture = fiveSPR;
				break;

			case 6:
				minutesUnitSPR.Texture = sixSPR;
				break;

			case 7:
				minutesUnitSPR.Texture = sevenSPR;
				break;

			case 8:
				minutesUnitSPR.Texture = eightSPR;
				break;

			case 9:
				minutesUnitSPR.Texture = nineSPR;
				break;
		}
		switch (minutesTen)
		{
			case 0:
				minutesTenSPR.Texture = zeroSPR;
				break;

			case 1:
				minutesTenSPR.Texture = oneSPR;
				break;

			case 2:
				minutesTenSPR.Texture = twoSPR;
				break;

			case 3:
				minutesTenSPR.Texture = threeSPR;
				break;

			case 4:
				minutesTenSPR.Texture = fourSPR;
				break;

			case 5:
				minutesTenSPR.Texture = fiveSPR;
				break;

			case 6:
				minutesTenSPR.Texture = sixSPR;
				break;

			case 7:
				minutesTenSPR.Texture = sevenSPR;
				break;

			case 8:
				minutesTenSPR.Texture = eightSPR;
				break;

			case 9:
				minutesTenSPR.Texture = nineSPR;
				break;
		}
		switch (hoursUnit)
		{
			case 0:
				hoursUnitSPR.Texture = zeroSPR;
				break;

			case 1:
				hoursUnitSPR.Texture = oneSPR;
				break;

			case 2:
				hoursUnitSPR.Texture = twoSPR;
				break;

			case 3:
				hoursUnitSPR.Texture = threeSPR;
				break;

			case 4:
				hoursUnitSPR.Texture = fourSPR;
				break;

			case 5:
				hoursUnitSPR.Texture = fiveSPR;
				break;

			case 6:
				hoursUnitSPR.Texture = sixSPR;
				break;

			case 7:
				hoursUnitSPR.Texture = sevenSPR;
				break;

			case 8:
				hoursUnitSPR.Texture = eightSPR;
				break;

			case 9:
				hoursUnitSPR.Texture = nineSPR;
				break;
		}
		switch (hoursTen)
		{
			case 0:
				hoursTenSPR.Texture = zeroSPR;
				break;

			case 1:
				hoursTenSPR.Texture = oneSPR;
				break;

			case 2:
				hoursTenSPR.Texture = twoSPR;
				break;

			case 3:
				hoursTenSPR.Texture = threeSPR;
				break;

			case 4:
				hoursTenSPR.Texture = fourSPR;
				break;

			case 5:
				hoursTenSPR.Texture = fiveSPR;
				break;

			case 6:
				hoursTenSPR.Texture = sixSPR;
				break;

			case 7:
				hoursTenSPR.Texture = sevenSPR;
				break;

			case 8:
				hoursTenSPR.Texture = eightSPR;
				break;

			case 9:
				hoursTenSPR.Texture = nineSPR;
				break;
		}
	}

}
