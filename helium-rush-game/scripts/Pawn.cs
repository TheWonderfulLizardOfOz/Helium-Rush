using Godot;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using Vector2 = Godot.Vector2;
using Vector3 = Godot.Vector3;

public partial class Pawn : Node2D
{

	List<Need> needs = new List<Need>();
	public MapGrid mapGrid;

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
			pathFinder.BreadthFirstSearch(GetPosition3(), new Vector3I((int) GetGlobalMousePosition().X, (int) GetGlobalMousePosition().Y, 0));
		}
	}


	public void Move(){
		Vector3 nextPathPosition = pathFinder.GetNextPathPosition(GetPosition3());
		if (nextPathPosition == Vector3.Zero)
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

	public void SetMapGrid(MapGrid mapGrid)
	{
		this.mapGrid = mapGrid;
		pathFinder.MapGrid = mapGrid;
	}

	public Vector3I GetPosition3()
	{
		return new Vector3I((int) Position.X, (int) Position.Y, ZIndex/2);
	}
}
