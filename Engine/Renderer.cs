using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Editor.Engine;

internal class Renderer
{
    private static readonly Lazy<Renderer> instance = new(() => new Renderer());
    public static Renderer Instance => instance.Value;

    internal Camera Camera { get; set; }
    internal Light Light { get; set; }

    private Renderer() { }

    public void Render(IRenderable renderable)
    {
        SetShaderParameters(renderable);
        renderable.Render();
    }

    private void SetShaderParameters(IRenderable renderable)
    {
        Effect effect = renderable.Effect;
        effect.Parameters["WorldViewProjection"]?.SetValue(renderable.Transform * Camera.View * Camera.Projection);
        effect.Parameters["Texture"]?.SetValue(renderable.Texture);
        effect.Parameters["TextureTiling"]?.SetValue(15f);
        effect.Parameters["LightDirection"]?.SetValue(renderable.Position - Light.Position);
        effect.Parameters["Tint"]?.SetValue(new Vector3(renderable.Tint.R / 255f, renderable.Tint.G / 255f, renderable.Tint.B / 255f));
    }
}
