using Godot;
using System;

public partial class Ground : TileMapLayer
{
	[Export]
	private TileMapLayer obstacleLayer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

    public override bool _UseTileDataRuntimeUpdate(Vector2I coords)
    {
      Godot.Collections.Array<Vector2I> cells = obstacleLayer.GetUsedCells();

			foreach (Vector2I cell in cells) 
			{
				if (cell == coords) 
				{
					return true;
				}
			}
			return false;
    }

    public override void _TileDataRuntimeUpdate(Vector2I coords, TileData tileData)
    {
      Godot.Collections.Array<Vector2I> cells = obstacleLayer.GetUsedCells();

			foreach(Vector2I cell in cells) 
			{
				if (cell == coords && (float) obstacleLayer.GetCellTileData(coords).GetCustomData("passability") == 0)
				{
					tileData.SetNavigationPolygon(0, null);
				}
			}
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
		{
		}
}
