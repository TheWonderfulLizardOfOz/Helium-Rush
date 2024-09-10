using Godot;
using System;

[Tool]
[GlobalClass]
public partial class Block : MapTileResource
{
	[Export]
	public int passability;
}
