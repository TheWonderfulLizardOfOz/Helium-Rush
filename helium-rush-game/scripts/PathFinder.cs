using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class PathFinder : Node2D
{
	// Called when the node enters the scene tree for the first time.
	private TileMapLayer floorLayer;
	private TileMapLayer blockLayer;

	private List<Vector3I> path= new List<Vector3I>();

	private MapGrid mapGrid;
	public MapGrid MapGrid {
		set {
			mapGrid = value;
		}
	}

	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void BreadthFirstSearch(Vector3 startPx, Vector3 targetPx) 
	{
		//convert px to atlasmap coords
		Vector3I target = mapGrid.PxToAtlas(targetPx);
		Vector3I start =  mapGrid.PxToAtlas(startPx);


		Queue<Vector3I> queue = new Queue<Vector3I>();
		HashSet<Vector3I> visited = new HashSet<Vector3I>() {start};
		Dictionary<Vector3I, Vector3I> parents = new Dictionary<Vector3I, Vector3I>();


		//TODO Check if target accessible
		if (start == target)
		{
			return;
		} else if (mapGrid.GetCell(target) == null)
		{
			return;
		}
		else if (GetWeight(mapGrid.GetCell(target)) == 0)
		{
			return;
		}


		queue.Enqueue(start);
		while (queue.Count > 0) 
		{
			Vector3I currentNode = queue.Dequeue();
			List<Vector3I> neighbours = GetAccessibleNeighbours(currentNode);
			if (neighbours == null)
				continue;
			foreach (Vector3I neighbour in neighbours) 
			{	
				if (!visited.Contains(neighbour))
				{
					if (neighbour == target)
					{	
						parents.Add(neighbour, currentNode);
						GetPath(parents, target, start);
						return;
					}
					else 
					{
						visited.Add(neighbour);
						parents.Add(neighbour, currentNode);
						queue.Enqueue(neighbour);
					}
				}
			}
		}
	}

	public List<Vector3I> GetAccessibleNeighbours(Vector3I node)
	{
		List<Vector3I> neighbours = new List<Vector3I>();
		List<Vector3I> accessibleNeighbours = new List<Vector3I>();
		neighbours.Add(new Vector3I(node.X, node.Y + 1, node.Z));
		neighbours.Add(new Vector3I(node.X, node.Y - 1, node.Z));
		neighbours.Add(new Vector3I(node.X + 1, node.Y, node.Z));
		neighbours.Add(new Vector3I(node.X - 1, node.Y, node.Z));

		foreach(Vector3I neighbour in neighbours)
		{
			if (mapGrid.GetCell(neighbour) != null && GetWeight(mapGrid.GetCell(neighbour)) != 0)
			{
				accessibleNeighbours.Add(neighbour);
			}
		}
		
		return accessibleNeighbours;
	}

	public void GetPath(Dictionary<Vector3I, Vector3I> parents, Vector3I end, Vector3I start) 
	{
		path = new List<Vector3I>();
		Vector3I current = end;
		while (current != start) 
		{
			path.Insert(0, current);
			current = parents[current];
		}
	}

	public Vector3 GetNextPathPosition(Vector3I currentPx)
	{	
		
		Vector3I current = mapGrid.PxToAtlas(currentPx);
		if (path.Count == 0) 
		{
			return Vector3I.Zero;
		}
		else 
		{
			Vector3I nextPosition = path[0];
			path.RemoveAt(0);
			return mapGrid.AtlasToPx(new Vector3I(nextPosition.X - current.X, nextPosition.Y - current.Y, nextPosition.Z));
		}
	}

	public int GetWeight(MapCell cell)
	{
		if (cell.Floor == null) 
		{
			return 0;
		} 
		else if (cell.Block == null)
		{
			return cell.Floor.walkability;
		} else
		{
			return Math.Min(cell.Block.passability, cell.Floor.walkability);
		}
	}

}
