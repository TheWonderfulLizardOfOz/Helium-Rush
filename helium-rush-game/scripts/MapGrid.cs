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

	private Dictionary<int, Pawn> entities = new Dictionary<int, Pawn>();
	
	private int nextEntityID = 0;

	public override void _Ready()
	{
		TickTimer.Instance.Timeout += Tick;

		Initiate();
	}

	private void Initiate()
	{
		for (int z=zRange.X;z<=zRange.Y;z++)
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

			for (int x=0;x<mapSize.X;x++)
			{
				for (int y=0;y<mapSize.Y;y++)
				{
					grid[new Vector3I(x,y,z)] = new MapCell();
				}
			}
		}

		for (int x = 0; x < mapSize.X; x++) 
		{
			for (int y = 0; y< mapSize.Y; y++)
			{
				PlaceFloor(new Vector3I(x, y, 0), (Floor) AssetLoader.Instance.tiles["floor_grass"]);
				if (GD.Randf() < 0.2f)
				{
					PlaceBlock(new Vector3I(x,y,0), (Block)AssetLoader.Instance.tiles["wall_wood"]);
				}
			}
		}

		Node2D entities = new Node2D();
		entities.Name = "EntityLayer";
		entities.ZIndex = 0;
		AddChild(entities);
		SpawnEntity(Vector3I.Zero, AssetLoader.Instance.entities["pawn"], this);
	}

	public void PlaceFloor(Vector3I position, Floor floor)
	{
		if (grid[position] == null)
		{
			GD.Print("Tried to place floor outside of grid");
			return;
		}
		floorLayers[position.Z].SetCell(new Vector2I(position.X, position.Y), 0, floor.atlasCoords);
		grid[position].Floor = floor;
	}

	public void RemoveFloor(Vector3I position, Floor floor)
	{
		floorLayers[position.Z].EraseCell(new Vector2I(position.X, position.Y));
		grid[position].Floor = null;
	}

	public void PlaceBlock(Vector3I position, Block block)
	{
		if (grid[position] == null)
		{
			GD.Print("Tried to place block outside of grid");
			return;
		}
		blockLayers[position.Z].SetCell(new Vector2I(position.X, position.Y), 0, block.atlasCoords);
		grid[position].Block = block;
	}

	public void RemoveBlock(Vector3I position, Block block)
	{
		blockLayers[position.Z].EraseCell(new Vector2I(position.X, position.Y));
		grid[position].Block = null;
	}

	public int SpawnEntity(Vector3I position, PackedScene entityScene, MapGrid mapGrid)
	{
		int id = nextEntityID++;
		Pawn entity = entityScene.Instantiate() as Pawn;
		entity.Position = floorLayers[position.Z].MapToLocal(new Vector2I(position.X, position.Y));
		entity.ZIndex = position.Z*2+1;
		entities[id] = entity;
		AddChild(entity);
		entity.SetMapGrid(mapGrid);
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

	public MapCell GetCell(Vector3I position)
	{
		if (grid.ContainsKey(position))
		{
			return grid[position];
		} else 
		{
			return null;
		}
	}

	public Vector3I PxToAtlas(Vector3 coords)
	{
		Vector2I xyCoords = floorLayers[(int) coords.Z].LocalToMap(new Vector2(coords.X, coords.Y));
		return new Vector3I(xyCoords.X, xyCoords.Y, (int)coords.Z);
	}

	public Vector3 AtlasToPx(Vector3I coords)
	{
		Vector2 xyCoords = floorLayers[coords.Z].MapToLocal(new Vector2I(coords.X, coords.Y));
		return new Vector3(xyCoords.X, xyCoords.Y, coords.Z);
	}
}
