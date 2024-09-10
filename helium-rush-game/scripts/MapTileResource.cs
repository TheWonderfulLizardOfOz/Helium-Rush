using Godot;
using System;

[Tool]
[GlobalClass]
public partial class MapTileResource : Resource
{
	[Export]
	public string name;
	[Export]
	public Vector2I atlasCoords;
}
