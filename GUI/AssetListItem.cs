namespace Editor.GUI;

internal class AssetListItem
{
    public string Name { get; set; }
    public AssetMonitor.AssetType Type { get; set; }

    public AssetListItem(string name, AssetMonitor.AssetType assetType)
    {
        Name = name;
        Type = assetType;
    }

    public override string ToString()
    {
        return Name;
    }
}
