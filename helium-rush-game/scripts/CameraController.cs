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

	[Export]
	public float zoominesss = 1.5f;

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
		if (Input.IsActionJustPressed("zoom_in")) {
			Zoom /= zoominesss;
		}
		else if (Input.IsActionJustPressed("zoom_out")){
			Zoom *= zoominesss;
		}
		Vector2 newZoom;
		newZoom.X = Math.Clamp(Zoom.X, (float) Math.Pow(zoominesss, -8), (float) Math.Pow(zoominesss, 8));
		newZoom.Y = Math.Clamp(Zoom.Y, (float) Math.Pow(zoominesss, -8), (float) Math.Pow(zoominesss, 8));
		Zoom = newZoom;
	}

	public void Move(Vector2 deltaMovement)
	{
		Translate(deltaMovement);
	}
}
