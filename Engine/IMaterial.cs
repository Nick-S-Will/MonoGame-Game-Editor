using Microsoft.Xna.Framework.Graphics;

namespace Editor.Engine;

internal interface IMaterial
{
    public Texture2D Texture { get; set; }
    public Effect Effect { get; set; }
}
