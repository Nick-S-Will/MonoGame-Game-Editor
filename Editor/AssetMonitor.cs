using System;
using System.Collections.Generic;
using System.IO;

namespace Editor.Editor;

internal class AssetMonitor
{
    internal enum AssetTypes
    {
        Model,
        Texture,
        Font,
        Audio,
        Effect
    }

    public event Action OnAssetsUpdated;

    public Dictionary<AssetTypes, List<string>> Assets { get; private set; } = new();

    private readonly FileSystemWatcher watcher;
    private readonly string metaInfo = string.Empty;

    internal AssetMonitor(string objectPath)
    {
        watcher = new FileSystemWatcher(objectPath);
        watcher.Created += Create;
        watcher.Changed += Change;
        watcher.Deleted += Delete;
        watcher.Filter = "*.mgstats";
        watcher.IncludeSubdirectories = false;
        watcher.EnableRaisingEvents = true;
        metaInfo = Path.Combine(objectPath, ".mgstats");
    }

    private void Create(object sender, FileSystemEventArgs e)
    {
        UpdateAssetDatabase();
    }

    private void Change(object sender, FileSystemEventArgs e)
    {
        UpdateAssetDatabase();
    }

    private void Delete(object sender, FileSystemEventArgs e)
    {
        if (Assets.Count == 0) return;

        Assets.Clear();
        OnAssetsUpdated?.Invoke();
    }

    private void UpdateAssetDatabase()
    {
        using var inputStream = new FileStream(metaInfo, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var streamReader = new StreamReader(inputStream);
        string[] content = streamReader.ReadToEnd().Split(Environment.NewLine);
        bool hasChanged = false;
        foreach (string line in content)
        {
            if (string.IsNullOrEmpty(line)) continue;

            string[] fields = line.Split(',');
            if (fields[0] == "Source File" || fields[2] != "\"ModelProcessor\"") continue;

            if (!Assets.ContainsKey(AssetTypes.Model)) Assets.Add(AssetTypes.Model, new());

            string assetName = Path.GetFileNameWithoutExtension(fields[1]);
            if (Assets[AssetTypes.Model].Contains(assetName)) continue;

            Assets[AssetTypes.Model].Add(assetName);
            hasChanged = true;
        }

        if (hasChanged) OnAssetsUpdated?.Invoke();
    }
}