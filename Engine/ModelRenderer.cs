using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace Editor.Engine;

internal class ModelRenderer : IRenderable, ISerializable
{
	public Model Model { get; set; }
	public Texture2D Texture { get; set; }
	public Effect Effect
	{
		get => effect;
		set
		{
			if (effect == value) return;

			effect = value;
			foreach (var mesh in Model.Meshes)
			{
				foreach (var part in mesh.MeshParts)
				{
					part.Effect = effect;
				}
			}
		}
	}
	public Vector3 Position { get; set; }
	public Vector3 EulerAngles { get; set; }
	public Color Tint { get; set; } = Color.Black;
	public float Scale { get; set; } = 1f;

	private Effect effect;

	public ModelRenderer() { }

	public ModelRenderer(ContentManager contentManager, string modelName, string textureName, string effectName, Vector3 position, float scale)
	{
		Create(contentManager, modelName, textureName, effectName, position, scale);
	}

	private void Create(ContentManager contentManager, string modelName, string textureName, string effectName, Vector3 position, float scale)
	{
		Model = contentManager.Load<Model>(modelName);
		Model.Tag = modelName;
		Texture = contentManager.Load<Texture2D>(textureName);
		Texture.Tag = textureName;
		Effect = contentManager.Load<Effect>(effectName);
		Effect.Tag = effectName;
		Position = position;
		Scale = scale;
	}

    public void Render()
    {
        foreach (ModelMesh modelMesh in Model.Meshes) modelMesh.Draw();
    }

    public void Serialize(BinaryWriter binaryWriter)
	{
		binaryWriter.Write(Model.Tag.ToString());
		binaryWriter.Write(Texture.Tag.ToString());
		binaryWriter.Write(Effect.Tag.ToString());
		Position.Serialize(binaryWriter);
		EulerAngles.Serialize(binaryWriter);
		binaryWriter.Write(Scale);
	}

	public void Deserialize(BinaryReader binaryReader, ContentManager contentManager)
	{
		string modelName = binaryReader.ReadString();
		string textureName = binaryReader.ReadString();
		string effectName = binaryReader.ReadString();
		Position = Vector3Extensions.Deserialize(binaryReader);
		EulerAngles = Vector3Extensions.Deserialize(binaryReader);
		Scale = binaryReader.ReadSingle();
		Create(contentManager, modelName, textureName, effectName, Position, Scale);
	}
}