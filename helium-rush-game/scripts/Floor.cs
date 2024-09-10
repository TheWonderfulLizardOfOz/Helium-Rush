using Godot;
using System;

[Tool]
[GlobalClass]
public partial class Floor : MapTileResource
{
	[Export]
	public int walkability;
}
