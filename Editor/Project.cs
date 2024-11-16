﻿using Editor.Engine;
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

	public AssetMonitor AssetMonitor { get; private set; }
	public Level CurrentLevel { get; private set; }
	public string Folder { get; private set; } = string.Empty;
	public string Name { get; private set; } = string.Empty;
	public string ContentFolder => string.IsNullOrEmpty(Folder) ? string.Empty : Path.Combine(Folder, "Content");
    public string AssetFolder => string.IsNullOrEmpty(ContentFolder) ? string.Empty : Path.Combine(ContentFolder, "bin");
    public string ObjectFolder => string.IsNullOrEmpty(ContentFolder) ? string.Empty : Path.Combine(ContentFolder, "obj");
	public string ContentPath => string.IsNullOrEmpty(ContentFolder) ? string.Empty : Path.Combine(ContentFolder, "Content.mgcb");

	private readonly List<Level> levels = new();

    public Project() { }

	public Project(ContentManager contentManager, GraphicsDevice graphicsDevice)
	{
		AddLevel(contentManager, graphicsDevice);
    }

    private void AddLevel(ContentManager contentManager, GraphicsDevice graphicsDevice)
    {
        CurrentLevel = new();
        CurrentLevel.LoadContent(contentManager, graphicsDevice);
        levels.Add(CurrentLevel);
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
		binaryWriter.Write(Folder);
		binaryWriter.Write(Name);
		binaryWriter.Write(levels.Count);
        foreach (var level in levels) level.Serialize(binaryWriter);
		int levelIndex = levels.IndexOf(CurrentLevel);
		binaryWriter.Write(levelIndex);
    }

	public void Deserialize(BinaryReader binaryReader, ContentManager contentManager)
	{
		Folder = binaryReader.ReadString();
		Name = binaryReader.ReadString();
		SetPath(Path.Combine(Folder, Name));
		contentManager.RootDirectory = Folder + @"\Content\bin";

        int levelCount = binaryReader.ReadInt32();
        for (int i = 0; i < levelCount; i++)
        {
			Level level = new();
			level.Deserialize(binaryReader, contentManager);
			levels.Add(level);
		}
		int levelIndex = binaryReader.ReadInt32();
		CurrentLevel = levels[levelIndex];
    }
}
