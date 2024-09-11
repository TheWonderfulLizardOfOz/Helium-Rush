using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class AssetLoader : Node
{
	public static AssetLoader Instance;

	private delegate void Load(string filename);

	public Dictionary<string, MapTileResource> tiles;
	public Dictionary<string, PackedScene> entities;

	public override void _Ready()
	{
		Instance = this;
		RecursiveLoad("res://tiles", LoadTile, "*.tres");
		RecursiveLoad("res://entities", LoadEntity, "*.tscn");
	}

	private void RecursiveLoad(string folderPath, Load loader, string searchPattern)
	{
		try
		{
			foreach (string filePath in Directory.GetFiles(folderPath,searchPattern))
			{
				loader(filePath);
			}

			foreach (string directory in Directory.GetDirectories(folderPath))
			{
				RecursiveLoad(directory, loader, searchPattern);
			}
		}
		catch (Exception ex)
		{
			GD.Print($"An error occurred while loading {folderPath}: {ex.Message}");
		}
	}

	private void LoadTile(string filePath)
	{
    string fileName = Path.GetFileName(filePath);
		tiles[fileName] = ResourceLoader.Load(filePath) as MapTileResource;
	}

	private void LoadEntity(string filePath)
	{
		string fileName = Path.GetFileName(filePath);
		entities[fileName] = ResourceLoader.Load(filePath) as PackedScene;
	}
}
