using Godot;
using System;

public partial class Clock : Label
{
	[Export]
	//number of seconds a tick represent
	public int tickDuration;

	//number of ticks that has passed in this day
	private int time;

	public override void _Ready()
	{
		TickTimer.Instance.Timeout += Tick;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Tick(){
		time++;
		Text = GetTimeString();
	}

	public string GetTimeString(){
		string timeFormat = "{0}:{1}:{2}";
		int seconds = time * tickDuration;
		int minutes = seconds / 60;
		int hours = minutes / 60;
		return string.Format(timeFormat,hours%24, minutes%60, seconds%60);
	}
}
