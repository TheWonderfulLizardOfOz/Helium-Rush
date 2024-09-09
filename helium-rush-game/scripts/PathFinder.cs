using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class PathFinder : Node2D
{
	// Called when the node enters the scene tree for the first time.
	private TileMapLayer overGroundLayer;
	private List<Vector2I> path;
	public override void _Ready()
	{
		overGroundLayer = GetNode<TileMapLayer>("%OverGround");
		path = new List<Vector2I>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void BreadthFirstSearch(Vector2I start, Vector2 targetPx) 
	{
		Vector2I target = overGroundLayer.LocalToMap(targetPx);
		Queue<Vector2I> queue = new Queue<Vector2I>();
		HashSet<Vector2I> visited = new HashSet<Vector2I>() {start};
		Dictionary<Vector2I, Vector2I> parents = new Dictionary<Vector2I, Vector2I>();
		//target is impassable
		if (overGroundLayer.GetCellTileData(target) != null && (float) overGroundLayer.GetCellTileData(target).GetCustomData("passability") == 0)
		{
			return;
		} 
		else if (start == target)
		{
			path = new List<Vector2I>();
			return;
		}

		queue.Enqueue(start);
		while (queue.Count > 0) 
		{
			Vector2I currentNode = queue.Dequeue();
			List<Vector2I> neighbours = GetAccessibleNeighbours(currentNode);
			if (neighbours == null)
				continue;
			foreach (Vector2I neighbour in neighbours) 
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

	public List<Vector2I> GetAccessibleNeighbours(Vector2I node)
	{
		List<Vector2I> neighbours = new List<Vector2I>();
		List<Vector2I> accessibleNeighbours = new List<Vector2I>();
		neighbours.Add(new Vector2I(node.X, node.Y + 1));
		neighbours.Add(new Vector2I(node.X, node.Y - 1));
		neighbours.Add(new Vector2I(node.X + 1, node.Y));
		neighbours.Add(new Vector2I(node.X - 1, node.Y));

		foreach(Vector2I neighbour in neighbours)
		{
			if ((overGroundLayer.GetCellTileData(neighbour) == null || (float) overGroundLayer.GetCellTileData(neighbour).GetCustomData("passability") != 0) && neighbour.X < 10 && neighbour.Y < 10) {
				accessibleNeighbours.Add(neighbour);
			}
		}
		
		return accessibleNeighbours;
	}

	public void GetPath(Dictionary<Vector2I, Vector2I> parents, Vector2I end, Vector2I start) 
	{
		path = new List<Vector2I>();
		Vector2I current = end;
		while (current != start) 
		{
			path.Insert(0, current);
			current = parents[current];
		}
	}

	public Vector2I GetNextPathPosition(Vector2I current)
	{
		if (path.Count == 0) 
		{
			return Vector2I.Zero;
		}
		else 
		{
			Vector2I nextPosition = path[0];
			path.RemoveAt(0);
			return new Vector2I(nextPosition.X - current.X, nextPosition.Y - current.Y);
		}
	}
}
