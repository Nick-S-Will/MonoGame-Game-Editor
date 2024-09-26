using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace Editor.Engine;

internal class Level : ISerializable
{
	private readonly List<ModelRenderer> modelRenderers = new();
	private readonly Camera camera = new(new Vector3(0f, 2f, 2f), 16/9);

	public Camera Camera => camera;

	public void LoadContent(ContentManager contentManager)
	{
		ModelRenderer teapot = new(contentManager, "Teapot", "Metal", "MyShader", Vector3.Zero, 1f);
		AddModel(teapot);
	}

	public void AddModel(ModelRenderer teapot)
	{
		modelRenderers.Add(teapot);
	}

	public void Renderer()
	{
        foreach (var renderer in modelRenderers)
        {
			renderer.Render(camera.View, camera.Projection);
        }
    }

	public void Serialize(BinaryWriter binaryWriter)
	{
		binaryWriter.Write(modelRenderers.Count);
        foreach (var renderer in modelRenderers) renderer.Serialize(binaryWriter);
		camera.Serialize(binaryWriter);
    }

	public void Deserialize(BinaryReader binaryReader, ContentManager contentManager)
	{
		int modelCount = binaryReader.ReadInt32();
		for (int i = 0; i < modelCount; i++)
		{
			ModelRenderer renderer = new();
			renderer.Deserialize(binaryReader, contentManager);
			modelRenderers.Add(renderer);
		}
		camera.Deserialize(binaryReader, contentManager);
	}
}
