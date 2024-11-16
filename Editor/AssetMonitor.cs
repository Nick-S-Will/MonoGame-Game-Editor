using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Editor.Editor;

internal class AssetMonitor
{
    internal enum AssetType
    {
        Model,
        Texture,
        Effect,
        Font,
        Sound
    }

    private static readonly Dictionary<string, AssetType> processorStringToAssetType = new() { ["\"ModelProcessor\""] = AssetType.Model, ["\"TextureProcessor\""] = AssetType.Texture, ["\"SongProcessor\""] = AssetType.Sound, ["\"SoundEffectProcessor\""] = AssetType.Sound, ["\"EffectProcessor\""] = AssetType.Effect };

    public event Action OnAssetsUpdated;

    public List<string> this[AssetType assetType] => assets[assetType];
       
    private readonly Dictionary<AssetType, List<string>> assets = new();
    private readonly FileSystemWatcher watcher;
    private readonly string metaInfo = string.Empty;

    internal AssetMonitor(string objectPath)
    {
        foreach (var assetType in Enum.GetValues<AssetType>()) assets.Add(assetType, new());

        watcher = new FileSystemWatcher(objectPath);
        watcher.Created += Create;
        watcher.Changed += Change;
        watcher.Deleted += Delete;
        watcher.Filter = "*.mgstats";
        watcher.IncludeSubdirectories = false;
        watcher.EnableRaisingEvents = true;
        metaInfo = Path.Combine(objectPath, ".mgstats");

        UpdateAssetDatabase();
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
        ClearAssets();
        OnAssetsUpdated?.Invoke();
    }

    private void ClearAssets()
    {
        foreach (var assetType in Enum.GetValues<AssetType>()) assets[assetType].Clear();
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
            if (fields[0] == "Source File") continue;

            Debug.Assert(processorStringToAssetType.TryGetValue(fields[2], out AssetType assetType), "Unhandled processor.");
            string assetName = Path.GetFileNameWithoutExtension(fields[1]);
            if (AddAsset(assetType, assetName)) hasChanged = true;
        }

        if (hasChanged) OnAssetsUpdated?.Invoke();
    }

    private bool AddAsset(AssetType assetType, string assetName)
    {
        if (assets[assetType].Contains(assetName)) return false;

        assets[assetType].Add(assetName);

        return true;
    }
}