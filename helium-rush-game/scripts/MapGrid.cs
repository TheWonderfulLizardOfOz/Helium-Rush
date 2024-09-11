using Godot;
using System;
using System.Collections.Generic;

public partial class MapGrid : Node2D
{
	[Export]
	public Vector2I mapSize;
	[Export]
	public Vector2I zRange;
	[Export]
	public TileSet tileSet;

	private Dictionary<int, TileMapLayer> floorLayers = new Dictionary<int, TileMapLayer>();
	private Dictionary<int, TileMapLayer> blockLayers = new Dictionary<int, TileMapLayer>();

	private Dictionary<Vector3I, MapCell> grid = new Dictionary<Vector3I, MapCell>();

	private Node2D[] entities;
	private int nextEntityID = 0;

	public override void _Ready()
	{
		TickTimer.Instance.Timeout += Tick;

		Initiate();
	}

	private void Initiate()
	{
		for (int z=zRange.X;z<zRange.Y;z++)
		{
			TileMapLayer newFloorLayer = new TileMapLayer();
			newFloorLayer.Name = $"FloorLayer{z}";
			TileMapLayer newBlockLayer = new TileMapLayer();
			newBlockLayer.Name = $"BlockLayer{z}";

			floorLayers.Add(z, newFloorLayer);
			blockLayers.Add(z, newBlockLayer);

			newFloorLayer.TileSet = tileSet;
			newBlockLayer.TileSet = tileSet;

			newFloorLayer.ZIndex = z*2;
			newBlockLayer.ZIndex = z*2+1;

			AddChild(newFloorLayer);
			AddChild(newBlockLayer);

			for (int x=0;x<mapSize.X;x++){
				for (int y=0;y<mapSize.Y;y++){
					grid[new Vector3I(x,y,z)] = new MapCell();
				}
			}
		}
		Node2D entities = new Node2D();
		entities.Name = "EntityLayer";
		entities.ZIndex = 0;
		AddChild(entities);
	}

	public void PlaceFloor(Vector3I position, Floor floor)
	{
		if (grid[position] == null)
		{
			GD.Print("Tried to place floor outside of grid");
			return;
		}
		grid[position].Floor = floor;
	}

	public void RemoveFloor(Vector3I position, Floor floor)
	{
		throw new NotImplementedException("RemoveFloor() not implemented yet");
	}

	public void PlaceBlock(Vector3I position, Block block)
	{
		if (grid[position] == null)
		{
			GD.Print("Tried to place block outside of grid");
			return;
		}
		grid[position].Block = block;
	}

	public void RemoveBlock(Vector3I position, Block block)
	{
		throw new NotImplementedException("RemoveBlock() not implemented yet");
	}

	public int SpawnEntity(Vector3I position, PackedScene entityScene)
	{
		int id = nextEntityID++;
		Node2D entity = entityScene.Instantiate() as Node2D;
		entity.Position = new Vector2I(position.X, position.Y);
		entity.ZIndex = position.Z*2+1;
		entities[id] = entity;
		AddChild(entity);
		return id;
	}

	public void DespawnEntity(int id)
	{
		throw new NotImplementedException("DespawnEntity() not implemented yet");
	}

	public override void _Process(double delta)
	{

	}

	public void Tick()
	{
		
	}
}
