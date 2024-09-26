using Editor.Engine;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.IO;

namespace Editor.Editor;

internal class Project : ISerializable
{
	private const string FileExtension = ".oce";

	public Level CurrentLevel { get; set; } = null;
	public List<Level> Levels { get; set; } = new();
	public string Folder { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;

	public Project() { }

	public Project(ContentManager contentManager, string name)
	{
		Folder = Path.GetDirectoryName(name);
		Name = Path.GetFileName(name);
		if (!Name.ToLower().EndsWith(FileExtension)) Name += FileExtension;

		AddLevel(contentManager);
	}

	private void AddLevel(ContentManager contentManager)
	{
		CurrentLevel = new();
		CurrentLevel.LoadContent(contentManager);
		Levels.Add(CurrentLevel);
	}

	public void Render()
	{
		CurrentLevel.Renderer();
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
