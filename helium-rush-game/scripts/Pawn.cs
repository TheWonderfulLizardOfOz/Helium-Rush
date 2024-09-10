using Godot;
using System;
using System.Collections.Generic;
using System.Numerics;
using Vector2 = Godot.Vector2;

public partial class Pawn : Node2D
{

	List<Need> needs = new List<Need>();

	PathFinder pathFinder;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach (Node child in GetChildren()){
			if (child is Need){
				needs.Add(child as Need);
				GD.Print(child.Name);
			}
		}
		TickTimer.Instance.Timeout += Tick;
		pathFinder = GetNode<PathFinder>("PathFinder");
	}
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GetInput();
	}

	public void GetInput()
	{
		if (Input.IsActionJustReleased("left_mouse_click"))
		{
			pathFinder.BreadthFirstSearch(GlobalPosition, GetGlobalMousePosition());
		}
	}


	public void Move(){
		Vector2 nextPathPosition = pathFinder.GetNextPathPosition(GlobalPosition);
		if (nextPathPosition == Vector2.Zero)
		{
			return;
		}
		Translate(new Vector2(nextPathPosition.X - 32, nextPathPosition.Y - 32));
	}

	public void Tick()
	{	
		foreach (Need need in needs){
			need.Tick();
		}
		Move();
	}
}
