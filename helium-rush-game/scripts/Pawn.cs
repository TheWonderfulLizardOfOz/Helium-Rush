using Godot;
using System;
using System.Collections.Generic;
using System.Numerics;
using Vector2 = Godot.Vector2;

public partial class Pawn : Node2D
{
	[Export]
	int sleepDecayRate = 10;

	List<Need> needs = new List<Need>();

	NavigationAgent2D navigationAgent;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach (Node child in GetChildren()){
			if (child is Need){
				needs.Add(child as Need);
				GD.Print(child.Name);
			}
		}

		Timer timer = GetNode<Timer>("/root/Base/TickTimer");
		timer.Timeout += Tick;

		navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		navigationAgent.TargetPosition = GetGlobalMousePosition();
	}

	public void Move(){
		Vector2 delta;
		Vector2 nextPathPosition = navigationAgent.GetNextPathPosition();
		//Translate(nextPathPosition - currentPosition);
		delta.X = (int)Math.Clamp(Math.Round((nextPathPosition.X - GlobalPosition.X)/64),-1,1)*64;
		delta.Y = (int)Math.Clamp(Math.Round((nextPathPosition.Y - GlobalPosition.Y)/64),-1,1)*64;
		Translate(delta);
	}

	public void Tick()
	{	
		foreach (Need need in needs){
			need.Tick();
		}
	}
}
