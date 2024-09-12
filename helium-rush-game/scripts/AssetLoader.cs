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
		LoadTiles("res://tiles/");
		LoadEntities("res://entities/");
	}


	private void LoadTiles(string folderPath)
	{
    DirAccess dir_access = DirAccess.Open(folderPath);
    if (dir_access == null) { return; }

    string[] files = dir_access.GetFiles();
    if (files == null) { return; }

    foreach(string fileName in files)
    {
			Resource loaded_resource = GD.Load<Resource>(folderPath + fileName);
			if (loaded_resource == null) { continue; }
			
			Regex regex = new Regex(@"(.*?)(?=\.tres)");
			GD.Print(fileName);
			tiles[regex.Match(fileName).Groups[0].Value] = loaded_resource as MapTileResource;
    }
	}

	private void LoadEntities(string folderPath)
	{
    DirAccess dir_access = DirAccess.Open(folderPath);
    if (dir_access == null) { return; }

    string[] files = dir_access.GetFiles();
    if (files == null) { return; }

    foreach(string fileName in files)
    {
			Resource loaded_resource = GD.Load<Resource>(folderPath + fileName);
			if (loaded_resource == null) { continue; }
			
			Regex regex = new Regex(@"(.*?)(?=\.tscn)");
			GD.Print(fileName);
			entities[regex.Match(fileName).Groups[0].Value] = loaded_resource as PackedScene;
    }
	}
}
