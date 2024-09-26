using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace Editor.Engine;

internal class ModelRenderer : ISerializable
{
	public Model Model { get; set; }
	public Texture Texture { get; set; }
	public Effect Effect 
	{
		get => effect;
		set
		{
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
	public Matrix Transform => Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) * Matrix.CreateTranslation(Position);
	public Vector3 Position = Vector3.Zero, Rotation = Vector3.Zero;
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
		Texture = contentManager.Load<Texture>(textureName);
		Texture.Tag = textureName;
		Effect = contentManager.Load<Effect>(effectName);
		Effect.Tag = effectName;
		Position = position;
		Scale = scale;
	}

	public void Render(Matrix view, Matrix projection)
	{
		Position.X += .001f;
		Rotation.Y += .005f;

		Effect.Parameters["World"]?.SetValue(Transform);
		Effect.Parameters["WorldViewProjection"].SetValue(Transform * view * projection);
		Effect.Parameters["Texture"].SetValue(Texture);

		foreach (var mesh in Model.Meshes) mesh.Draw();
	}

	public void Serialize(BinaryWriter binaryWriter)
	{
		binaryWriter.Write(Model.Tag.ToString());
		binaryWriter.Write(Texture.Tag.ToString());
		binaryWriter.Write(Effect.Tag.ToString());
		Position.Serialize(binaryWriter);
		Rotation.Serialize(binaryWriter);
		binaryWriter.Write(Scale);
	}

	public void Deserialize(BinaryReader binaryReader, ContentManager contentManager)
	{
		string modelName = binaryReader.ReadString();
		string textureName = binaryReader.ReadString();
		string effectName = binaryReader.ReadString();
		Position = Vector3Extensions.Deserialize(binaryReader);
		Rotation = Vector3Extensions.Deserialize(binaryReader);
		Scale = binaryReader.ReadSingle();
		Create(contentManager, modelName, textureName, effectName, Position, Scale);
	}
}