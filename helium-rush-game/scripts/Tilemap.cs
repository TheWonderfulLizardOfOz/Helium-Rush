using Godot;
using System;

public partial class Tilemap : Node2D
{
	public event Action OnTick;
	public override void _Ready()
	{
		TickTimer.Instance.Timeout += Tick;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Tick(){
		OnTick?.Invoke();
	}
}
