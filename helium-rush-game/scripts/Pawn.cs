using Godot;
using System;
using System.Numerics;
using Vector2 = Godot.Vector2;

public partial class Pawn : Node2D
{
	[Export]
	int sleepDecayRate = 10;

	NavigationAgent2D navigationAgent;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var timer = GetNode<Timer>("/root/Base/TickTimer");
		navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		timer.Timeout += OnTick;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		navigationAgent.TargetPosition = GetGlobalMousePosition();
	}

	public void OnTick()
	{	
		Vector2 delta;
		Vector2 nextPathPosition = navigationAgent.GetNextPathPosition();
		//Translate(nextPathPosition - currentPosition);
		delta.X = (int)Math.Clamp(Math.Round((nextPathPosition.X - GlobalPosition.X)/64),-1,1)*64;
		delta.Y = (int)Math.Clamp(Math.Round((nextPathPosition.Y - GlobalPosition.Y)/64),-1,1)*64;
		Translate(delta);
	}
}
