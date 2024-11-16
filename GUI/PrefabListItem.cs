namespace Editor.GUI;

internal class PrefabListItem
{
    public string Name { get; set; }

    public PrefabListItem(string name)
    {
        Name = name;
    }

    public override string ToString()
    {
        return Name;
    }
}