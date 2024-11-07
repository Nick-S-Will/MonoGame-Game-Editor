using Editor.Engine;
using Editor.GUI;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Editor.Editor;

internal class Project : ISerializable
{
	private const string FileExtension = ".oce";

	public event Action OnAssetsUpdated;

	public Level CurrentLevel { get; private set; }
	public List<Level> Levels { get; private set; } = new();
	public string Folder { get; private set; } = string.Empty;
	public string Name { get; private set; } = string.Empty;
	public AssetMonitor AssetMonitor { get; private set; }
	public string ContentFolder => string.IsNullOrEmpty(Folder) ? string.Empty : Path.Combine(Folder, "Content");
    public string AssetFolder => string.IsNullOrEmpty(ContentFolder) ? string.Empty : Path.Combine(ContentFolder, "bin");
    public string ObjectFolder => string.IsNullOrEmpty(ContentFolder) ? string.Empty : Path.Combine(ContentFolder, "obj");
	public string ContentPath => string.IsNullOrEmpty(ContentFolder) ? string.Empty : Path.Combine(ContentFolder, "Content.mgcb");

    public Project() { }

	public Project(ContentManager contentManager, GraphicsDevice graphicsDevice)
	{
		AddLevel(contentManager, graphicsDevice);
    }

    private void AddLevel(ContentManager contentManager, GraphicsDevice graphicsDevice)
    {
        CurrentLevel = new();
        CurrentLevel.LoadContent(contentManager, graphicsDevice);
        Levels.Add(CurrentLevel);
    }

    public void SetPath(string path)
	{
		Folder = Path.GetDirectoryName(path);
		Name = Path.GetFileName(path);
		if (!Name.ToLower().EndsWith(FileExtension)) Name += FileExtension;

		if (!Directory.Exists(ContentFolder))
		{
			Directory.CreateDirectory(ContentFolder);
			Directory.CreateDirectory(AssetFolder);
			Directory.CreateDirectory(ObjectFolder);
			File.Copy("ContentTemplate.mgcb", ContentPath);
		}
		AssetMonitor = new(ObjectFolder);
        AssetMonitor.OnAssetsUpdated += UpdateAssets;
	}

    private void UpdateAssets()
    {
		OnAssetsUpdated?.Invoke();
    }

	public void Update(float delta)
	{
		CurrentLevel?.Update(delta);
	}

	public void Render()
	{
		CurrentLevel.Render();
	}

	public void Serialize(BinaryWriter binaryWriter)
	{
		binaryWriter.Write(Levels.Count);
		int levelIndex = Levels.IndexOf(CurrentLevel);
        foreach (var level in Levels) level.Serialize(binaryWriter);
		binaryWriter.Write(levelIndex);
		binaryWriter.Write(Folder);
		binaryWriter.Write(Name);
    }

	public void Deserialize(BinaryReader binaryReader, ContentManager contentManager)
	{
		int levelCount = binaryReader.ReadInt32();
        for (int i = 0; i < levelCount; i++)
        {
			Level level = new();
			level.Deserialize(binaryReader, contentManager);
			Levels.Add(level);
		}
		int levelIndex = binaryReader.ReadInt32();
		CurrentLevel = Levels[levelIndex];
		Folder = binaryReader.ReadString();
		Name = binaryReader.ReadString();
	}
}
