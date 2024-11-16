using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System;
using Editor.Extensions;

namespace Editor.Engine;

internal class ModelRenderer : IRenderable, ISoundEmitter, ISerializable
{
	private const string Empty = "Empty";

	public string Name => Model.Tag.ToString();
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
	public Color Tint { get; set; } = Color.Black;
	public Dictionary<ISoundEmitter.Sound, KeyValuePair<string, SoundEffectInstance>> SoundEffects { get; private set; }
	public Vector3 Position { get; set; }
	public Vector3 EulerAngles { get; set; }
	public float Scale { get; set; } = 1f;

	private Effect effect;

	public ModelRenderer() { }

	public ModelRenderer(ContentManager contentManager, string modelName, string textureName, string effectName, Vector3 position, Vector3 eulerAngles, float scale)
	{
		Create(contentManager, modelName, textureName, effectName, Color.Black, position, eulerAngles, scale);
	}

	private void Create(ContentManager contentManager, string modelName, string textureName, string effectName, Color tint, Vector3 position, Vector3 eulerAngles, float scale)
	{
		Model = contentManager.Load<Model>(modelName);
		Model.Tag = modelName;
		Texture = contentManager.Load<Texture2D>(textureName);
		Texture.Tag = textureName;
		Effect = contentManager.Load<Effect>(effectName);
		Effect.Tag = effectName;
		Tint = tint;

		SoundEffects = new();

		Position = position;
		EulerAngles = eulerAngles;
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

        foreach (var sound in Enum.GetValues<ISoundEmitter.Sound>())
        {
			var hasSound = SoundEffects.TryGetValue(sound, out var soundInstance);
			string soundName = hasSound ? soundInstance.Key : Empty;
            binaryWriter.Write(soundName);
        }

        Position.Serialize(binaryWriter);
		EulerAngles.Serialize(binaryWriter);
		binaryWriter.Write(Scale);
	}

	public void Deserialize(BinaryReader binaryReader, ContentManager contentManager)
	{
		string modelName = binaryReader.ReadString();
		string textureName = binaryReader.ReadString();
		string effectName = binaryReader.ReadString();

        Dictionary<ISoundEmitter.Sound, KeyValuePair<string, SoundEffectInstance>> soundEffects = new();
        foreach (var sound in Enum.GetValues<ISoundEmitter.Sound>())
        {
			string soundName = binaryReader.ReadString();
			if (soundName == Empty) continue;

			soundEffects[sound] = ISoundEmitter.CreateSoundEffect(contentManager, soundName);
        }

        Position = VectorExtensions.Deserialize(binaryReader);
		EulerAngles = VectorExtensions.Deserialize(binaryReader);
		Scale = binaryReader.ReadSingle();

		Create(contentManager, modelName, textureName, effectName, Color.Black, Position, EulerAngles, Scale);
		SoundEffects = soundEffects;
	}
}