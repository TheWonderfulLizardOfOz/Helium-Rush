using Godot;
using Vector2 = Godot.Vector2;
using System;
using System.Numerics;
using System.Collections.Generic;
using System.Collections;

public partial class CameraController : Camera2D
{
	[Export]
	public int speed = 8;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GetInput();
	}

	public void GetInput() 
	{
		Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");
		Move(inputDirection * speed);
	}

	public void Move(Vector2 deltaMovement)
	{
		Translate(deltaMovement);
	}
}
