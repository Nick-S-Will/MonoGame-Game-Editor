using Microsoft.Xna.Framework.Content;
using System.IO;

namespace Editor.Engine;

internal interface ISerializable
{
	public void Serialize(BinaryWriter binaryWriter);
	public void Deserialize(BinaryReader binaryReader, ContentManager contentManager);
}
