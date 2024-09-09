using Godot;
using System;
using System.Diagnostics;

public partial class TickTimer : Timer
{
	const float ONETPS = 2;
	const float TWOTPS = 4;
	const float THREETPS = 8;

	public static TickTimer Instance;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("pause")) {
			Paused = !Paused;
		}
		if (@event.IsActionPressed("one_speed")){
			WaitTime = 1 / ONETPS;
		}
		if (@event.IsActionPressed("two_speed")){
			WaitTime = 1 / TWOTPS;
		}
		if (@event.IsActionPressed("three_speed")){
			WaitTime = 1 / THREETPS;
		}
	}

}
