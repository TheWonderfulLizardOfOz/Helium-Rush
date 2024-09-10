using Godot;
using System;

public partial class MapCell : Node
{
	private Floor floor;
	public Floor Floor {
		get {
			return floor;
		}
		set {
			floor = value;
		}
	}
	private Block block;
	public Block Block {
		get {
			return block;
		}
		set {
			block = value;
		}
	}

	public event Action OnStep;

	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{

	}

	public void Tick()
	{

	}
}
