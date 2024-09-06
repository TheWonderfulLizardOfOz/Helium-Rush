using Godot;
using System;

public partial class Pawn : Node2D
{
	[Export]
	int sleepDecayRate = 10;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var timer = GetNode<Timer>("/root/Base/TickTimer");
		timer.Timeout += OnTick;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnTick()
	{
		
	}
}
