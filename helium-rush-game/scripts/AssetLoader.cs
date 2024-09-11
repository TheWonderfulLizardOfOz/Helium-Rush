using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public partial class AssetLoader : Node
{
	public static AssetLoader Instance;

	private delegate void Load(string filename);

	public Dictionary<string, MapTileResource> tiles = new Dictionary<string, MapTileResource>();
	public Dictionary<string, PackedScene> entities = new Dictionary<string, PackedScene>();

	public override void _Ready()
	{
		Instance = this;
		//RecursiveLoad("res://tiles", LoadTile, "*.tres");
		//RecursiveLoad("res://entities", LoadEntity, "*.tscn");
		LoadTile("res://tiles/floor_grass.tres");
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
		Regex regex = new Regex(@"(.*?)(?=\.tres)");
		fileName = regex.Match(fileName).Groups[0].Value;
		tiles[fileName] = GD.Load(filePath) as MapTileResource;
	}

	private void LoadEntity(string filePath)
	{
		string fileName = Path.GetFileName(filePath);
		Regex regex = new Regex(@"(.*?)(?=\.tres)");
		fileName = regex.Match(fileName).Groups[0].Value;
		entities[fileName] = GD.Load(filePath) as PackedScene;
	}
}
