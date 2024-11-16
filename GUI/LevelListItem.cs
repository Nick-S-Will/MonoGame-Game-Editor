using Editor.Engine;

namespace Editor.GUI;

internal class LevelListItem
{
    public ModelRenderer Model { get; set; }

    public LevelListItem(ModelRenderer model)
    {
        Model = model;
    }

    public override string ToString()
    {
        return Model.Name;
    }
}
